using FTP;
using LotteCinemaLibraries.Common.Class;
using LotteCinemaService.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace LotteCinemaLibraries.Common.ViewModel
{
    public class ContentViewModelBase : INotifyPropertyChanged
    {
        #region Variable 

        public string LOCAL_MEDIA_PATH;
        public string LOCAL_AD_PATH;
        public string LOCAL_SPECIAL_AD_PATH;
        public string LOCAL_SKIN_PATH;

        public string FTP_MEDIA_PATH;
        public string FTP_AD_PATH;
        public string FTP_SPECIAL_AD_PATH;
        public string FTP_SKIN_PATH;

        public string FTP_SERVER_NAME;
        public string FTP_USER_NAME;
        public string FTP_PASSWORD;

        protected Ftp ftp;
        protected List<DownloadItem> downloadList;
        protected int downloadIndex;

        #endregion

        #region Property

        #region DownloadVisible

        private Visibility downloadVisible = Visibility.Collapsed;
        public Visibility DownloadVisible
        {
            get { return this.downloadVisible; }
            set
            {
                if (this.downloadVisible != value)
                {
                    this.downloadVisible = value;
                    this.RaisePropertyChanged("DownloadVisible");
                }
            }
        }

        #endregion

        #region DownloadMessage

        private string downloadMessage = "                           ";
        public string DownloadMessage
        {
            get { return this.downloadMessage; }
            set
            {
                if (this.downloadMessage != value)
                {
                    this.downloadMessage = value;
                    this.RaisePropertyChanged("DownloadMessage");
                }
            }
        }

        #endregion

        #region DownloadCount

        private int downloadCount;
        public int DownloadCount
        {
            get { return this.downloadCount; }
            set
            {
                if (this.downloadCount != value)
                {
                    this.downloadCount = value;
                    this.RaisePropertyChanged("DownloadCount");
                }
            }
        }

        #endregion

        #region DownloadTotalCount

        private int downloadTotalCount;
        public int DownloadTotalCount
        {
            get { return this.downloadTotalCount; }
            set
            {
                if (this.downloadTotalCount != value)
                {
                    this.downloadTotalCount = value;
                    this.RaisePropertyChanged("DownloadTotalCount");
                }
            }
        }

        #endregion

        #region DownloadPercentage

        private double downloadPercentage;
        public double DownloadPercentage
        {
            get { return this.downloadPercentage; }
            set
            {
                if (this.downloadPercentage != value)
                {
                    this.downloadPercentage = value;
                    this.RaisePropertyChanged("DownloadPercentage");
                }
            }
        }

        #endregion

        #region DownloadFileName

        private string downloadFileName;
        public string DownloadFileName
        {
            get { return this.downloadFileName; }
            set
            {
                if (this.downloadFileName != value)
                {
                    this.downloadFileName = value;
                    this.RaisePropertyChanged("DownloadFileName");
                }
            }
        }

        #endregion

        #region DownloadByte

        private long downloadByte;
        public long DownloadByte
        {
            get { return this.downloadByte; }
            set
            {
                if (this.downloadByte != value)
                {
                    this.downloadByte = value;
                    this.RaisePropertyChanged("DownloadByte");
                }
            }
        }

        #endregion

        #region DownloadTotalByte

        private long downloadTotalByte;
        public long DownloadTotalByte
        {
            get { return this.downloadTotalByte; }
            set
            {
                if (this.downloadTotalByte != value)
                {
                    this.downloadTotalByte = value;
                    this.RaisePropertyChanged("DownloadTotalByte");
                }
            }
        }

        #endregion

        #region DownloadProgressVisible

        private Visibility downloadProgressVisible = Visibility.Collapsed;
        public Visibility DownloadProgressVisible
        {
            get { return this.downloadProgressVisible; }
            set
            {
                if (this.downloadProgressVisible != value)
                {
                    this.downloadProgressVisible = value;
                    this.RaisePropertyChanged("DownloadProgressVisible");
                }
            }
        }

        #endregion

        #endregion

        #region Event

        public event RoutedEventHandler CompletedDownload;

        #endregion

        #region Constructor

        public ContentViewModelBase()
        {
        }

        #endregion

        #region Public Method

        public void Download()
        {
            if (this.ftp == null)
            {
                this.ftp = new Ftp(FTP_SERVER_NAME, FTP_USER_NAME, FTP_PASSWORD);
                this.ftp.DownloadProgressChanged += ftp_DownloadProgressChanged;
                this.ftp.DownloadFileAsyncCompleted += ftp_DownloadFileAsyncCompleted;
            }

            this.downloadList = new List<DownloadItem>();

            this.DownloadVisible = Visibility.Visible;
            this.DownloadProgressVisible = Visibility.Collapsed;

            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                OnDownload();

                if (this.downloadList.Count > 0)
                {
                    this.DownloadVisible = Visibility.Visible;
                    this.DownloadProgressVisible = Visibility.Visible;
                    this.DownloadTotalCount = this.downloadList.Count;

                    DownloadFile();
                }
                else
                {
                    Completed();
                }
            }));
        }

        public void CheckDownloadFiles(string localDirPath, string ftpDirPath, List<ContentsInfo> contentsInfos)
        {
            if (contentsInfos == null || contentsInfos.Count == 0) return;

            var groups = from c in contentsInfos
                         group c by new { c.GroupID, c.FileName };

            this.DownloadTotalCount = groups.Count();

            bool isDownload = false;
            int index = 1;

            DirectoryInfo rootDirInfo = new DirectoryInfo(localDirPath);

            if (!rootDirInfo.Exists)
            {
                rootDirInfo.Create();
            }

            foreach (var group in groups)
            {
                this.DownloadCount = index++;

                var ftpFilePath = Path.Combine(ftpDirPath, group.Key.GroupID, group.Key.FileName);
                var localFilePath = Path.Combine(Environment.CurrentDirectory, localDirPath, group.Key.GroupID, group.Key.FileName);
                var fileSize = this.ftp.GetFileSize(ftpFilePath);

                foreach (var content in group)
                {
                    content.FileSize = fileSize;
                    content.LocalFilePath = localFilePath;
                }

                if (fileSize < 0) continue;

                var subDirPath = Path.GetDirectoryName(localFilePath);
                DirectoryInfo subDirInfo = new DirectoryInfo(subDirPath);

                isDownload = false;

                if (!rootDirInfo.Exists)
                {
                    subDirInfo.Create();
                    isDownload = true;
                }
                else
                {
                    if (!subDirInfo.Exists)
                    {
                        subDirInfo.Create();
                        isDownload = true;
                    }
                    else
                    {
                        FileInfo fi = new FileInfo(localFilePath);

                        if (fi.Exists)
                        {
                            if (fi.Length != fileSize)
                            {
                                fi.Delete();
                                isDownload = true;
                            }
                        }
                        else
                        {
                            isDownload = true;
                        }
                    }
                }

                if (isDownload)
                {
                    DownloadItem downloadFile = new DownloadItem
                    {
                        FtpPath = ftpFilePath,
                        LocalPath = localFilePath,
                        FileName = group.Key.FileName,
                    };

                    this.downloadList.Add(downloadFile);
                }
            }
        }

        public void CheckDownloadFiles(List<ContentsInfo> contentsInfos)
        {
            if (contentsInfos == null || contentsInfos.Count == 0) return;

            var groups = from c in contentsInfos
                         group c by new { c.GroupID, c.LocalFilePath, c.FtpFilePath };

            this.DownloadTotalCount = groups.Count();
            bool isDownload = false;
            int index = 1;

            foreach (var group in groups)
            {
                this.DownloadCount = index++;
                var fileSize = this.ftp.GetFileSize(group.Key.FtpFilePath);

                foreach (var content in group)
                {
                    content.FileSize = fileSize;
                    content.LocalFilePath = Path.Combine(Environment.CurrentDirectory, content.LocalFilePath);
                }

                if (fileSize < 0) continue;

                var dirPath = Path.GetDirectoryName(group.Key.LocalFilePath);
                DirectoryInfo di = new DirectoryInfo(dirPath);

                isDownload = false;

                if (!di.Exists)
                {
                    di.Create();
                    isDownload = true;
                }
                else
                {
                    FileInfo fi = new FileInfo(group.Key.LocalFilePath);

                    if (fi.Exists)
                    {
                        if (fi.Length != fileSize)
                        {
                            fi.Delete();
                            isDownload = true;
                        }
                    }
                    else
                    {
                        isDownload = true;
                    }
                }

                if (isDownload)
                {
                    DownloadItem downloadFile = new DownloadItem
                    {
                        FtpPath = group.Key.FtpFilePath,
                        LocalPath = group.Key.LocalFilePath,
                        FileName = Path.GetFileName(group.Key.FtpFilePath)
                    };

                    this.downloadList.Add(downloadFile);
                }
            }
        }

        #endregion

        #region Virtual Method

        protected virtual void OnDownload()
        {
        }

        protected virtual void OnDownloadFile(DownloadItem item)
        {
        }

        protected virtual void OnDownloadFileAsyncCompleted(Exception error)
        {
        }

        protected virtual void OnCompleted()
        {
        }

        #endregion

        #region Private Method

        private void DownloadFile()
        {
            var downloadItem = this.downloadList[this.downloadIndex++];

            this.DownloadPercentage = 0;
            this.DownloadCount = this.downloadIndex;
            this.DownloadFileName = downloadItem.FileName;

            OnDownloadFile(downloadItem);

            this.ftp.DownloadFileAsyncWC(downloadItem.FtpPath, downloadItem.LocalPath);
        }

        private void Completed()
        {
            this.DownloadVisible = Visibility.Collapsed;

            OnCompleted();

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var onCompletedDownload = CompletedDownload;
                if (onCompletedDownload != null)
                {
                    onCompletedDownload(this, null);
                }
            }));
        }

        #endregion

        #region Event Handler

        private void ftp_DownloadFileAsyncCompleted(object sender, FtpAsyncCompletedEventArgs e)
        {
            OnDownloadFileAsyncCompleted(e.Error);

            if (this.downloadIndex < this.downloadList.Count)
            {
                DownloadFile();
            }
            else
            {
                Completed();
            }
        }

        private void ftp_DownloadProgressChanged(object sender, FtpDownloadProgressChangedEventArgs e)
        {
            this.DownloadPercentage = e.ProgressPercentage;
            this.DownloadByte = e.BytesReceived;
            this.DownloadTotalByte = e.TotalBytesToReceive;
        }

        #endregion

        #region RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var onPropertyChanged = PropertyChanged;

            if (onPropertyChanged != null)
            {
                onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}