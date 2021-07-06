using LotteCinemaLibraries.Common.Class;
using LotteCinemaLibraries.Common.ViewModel;
using LotteCinemaService.Model.Common;
using LotteCinemaService.WebAPI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotte_Client.ViewModel
{
   public class ContentViewModel : ContentViewModelBase
    {
        private Action _downloadCallBack;
        private Action<DownloadItem> _downloadFileCallBack;

        public Uri ServerUri { get; set; }
        public string TheaterCode { get; set; }
        public int ItemID { get; set; }
        public List<AdInfo> AdInfoList { get; set; }
        public ContentViewModel() : base()
        {
        }

        public void Download(Action downloadCallBack, Action<DownloadItem> downloadFileCallBack)
        {
            _downloadCallBack = downloadCallBack;
            _downloadFileCallBack = downloadFileCallBack;

            Download();
        }

        public void AddDownloadItem(DownloadItem item)
        {
            downloadList.Add(item);
        }

        protected override void OnDownload()
        {
            var groupInfoList = new List<GroupInfo>();
            var query = WebApiCommonQuery.GetAdInfoList_test(this.TheaterCode, this.ItemID, false);
            AdInfoList = WebApiHelper.GetResultJson<List<AdInfo>>(query, this.ServerUri);
            DownloadMessage = "광고 파일을 확인 중입니다.";

            foreach (var adverInfo in AdInfoList)
            {
                groupInfoList.Add(adverInfo);
            }

            if (groupInfoList.Count > 0)
            {
                CheckDownloadFiles(LOCAL_AD_PATH, FTP_AD_PATH, groupInfoList);
                groupInfoList.Clear();
            }
            if (_downloadCallBack != null)
            {
                _downloadCallBack.Invoke();
            }

        }

        protected override void OnDownloadFile(DownloadItem item)
        {
            if (!string.IsNullOrWhiteSpace(LOCAL_AD_PATH) && item.LocalPath.Contains(LOCAL_AD_PATH))
            {
                DownloadMessage = "광고를 다운로드 중입니다.";
            }

            if (_downloadFileCallBack != null)
            {
                _downloadFileCallBack.Invoke(item);
            }
        }

        private void CheckDownloadFiles(string localDirPath, string ftpDirPath, IEnumerable<GroupInfo> groupInfos)
        {
            var contentsInfoList = new List<ContentsInfo>();

            foreach (var groupInfo in groupInfos)
            {
                foreach (var contents in groupInfo.ContentsList)
                {
                        contentsInfoList.Add(contents);
                }
            }
            CheckDownloadFiles(localDirPath, ftpDirPath, contentsInfoList);
        }



    }
    }
