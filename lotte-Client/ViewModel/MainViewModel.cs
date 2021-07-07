using lotte_Client.Config;
using LotteCinemaLibraries.Common.Class;
using LotteCinemaService.WebAPI.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lotte_Client.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        // KIOSK
        public static readonly string LOCAL_KO_AD_PATH = "AD";
        public static readonly string FTP_KO_AD_PATH = "AD";

        public static readonly string FTP_SERVER_NAME = "127.0.0.1";
        public static readonly string FTP_USER_NAME = "gofogo";
        public static readonly string FTP_PASSWORD = "2";


        public delegate void DownloadCompletedDelegate();
        public event DownloadCompletedDelegate DownloadCompleted;

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

        private ContentViewModel _contentViewModel;
        public ContentViewModel ContentViewModel
        {
            get { return this._contentViewModel ?? (this._contentViewModel = new ContentViewModel()); }
        }

        public void StartDownload()
        {
            ContentViewModel.ServerUri = new Uri(ConfigValue.Instance.WebServerUri);
            ContentViewModel.ItemID = 999;
            ContentViewModel.LOCAL_AD_PATH = LOCAL_KO_AD_PATH;
            ContentViewModel.FTP_AD_PATH = FTP_KO_AD_PATH;

            ContentViewModel.FTP_SERVER_NAME = "127.0.0.1";
            ContentViewModel.FTP_USER_NAME = "gofogo";
            ContentViewModel.FTP_PASSWORD = "2";

            ContentViewModel.CompletedDownload += ContentViewModel_CompletedDownload;
            Action downAction = DoDown;
            Action<DownloadItem> downFileAction = DoDownFile;
            ContentViewModel.Download(downAction, downFileAction);
        }

        private void ContentViewModel_CompletedDownload(object sender, RoutedEventArgs e)
        {
            if (DownloadCompleted != null)
            {
                DownloadCompleted();
            }
        }

        private void DoDown()
        {
            Debug.WriteLine("FileDonwload Complete");
        }

        private void DoDownFile(DownloadItem item)
        {
            
        }
    }
}
