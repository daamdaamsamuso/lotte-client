using LotteCinemaLibraries.Common.Class;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Enum;
using LotteCinemaService.WebAPI.Helper;
using System;
using System.Collections.Generic;

namespace LotteCinemaLibraries.Common.ViewModel
{
    public class ContentViewModel : ContentViewModelBase
    {
        #region Variable

        private Action _downloadCallBack;
        private Action<DownloadItem> _downloadFileCallBack;
        
        #endregion

        #region Property

        #region ServerUri

        public Uri ServerUri { get; set; }

        #endregion

        #region TheaterCode

        public string TheaterCode { get; set; }

        #endregion

        #region ItemID

        public int ItemID { get; set; }

        #endregion

        #region PatternInfoList

        public List<PatternInfo> PatternInfoList { get; set; }

        #endregion

        #region AdInfoList

        public List<AdInfo> AdInfoList { get; set; }

        #endregion

        #region SpecialAdInfoList

        public List<AdInfo> SpecialAdInfoList { get; set; }

        #endregion

        #region MediaInfoList

        public List<MediaInfo> MediaInfoList { get; set; }

        #endregion

        #region AdSkinMapInfoList

        public List<SkinMapInfo> AdSkinMapInfoList { get; set; }

        #endregion

        #region SpecialAdSkinMapInfoList

        public List<SkinMapInfo> SpecialAdSkinMapInfoList { get; set; }

        #endregion

        #region MediaSkinMapInfoList

        public List<SkinMapInfo> MediaSkinMapInfoList { get; set; }

        #endregion

        #endregion

        #region Constructor

        public ContentViewModel() :
            base()
        {
        }

        #endregion
         
        #region Public Method

        public void Download(Action downloadCallBack, Action<DownloadItem> downloadFileCallBack)
        {
            this._downloadCallBack = downloadCallBack;
            this._downloadFileCallBack = downloadFileCallBack;

            base.Download();
        }

        public void AddDownloadItem(DownloadItem item)
        {
            this.downloadList.Add(item);
        }

        #endregion

        #region Protected Method

        protected override void OnDownload()
        {
            List<GroupInfo> groupInfoList = new List<GroupInfo>();

            var query = WebApiCommonQuery.GetPatternInfoList(this.TheaterCode, this.ItemID);
            this.PatternInfoList = WebApiHelper.GetResultJson<List<PatternInfo>>(query, this.ServerUri);

            query = WebApiCommonQuery.GetMediaInfoList(this.TheaterCode, this.ItemID);
            this.MediaInfoList = WebApiHelper.GetResultJson<List<MediaInfo>>(query, this.ServerUri);

            query = WebApiCommonQuery.GetAdInfoList(this.TheaterCode, this.ItemID, false);
            this.AdInfoList = WebApiHelper.GetResultJson<List<AdInfo>>(query, this.ServerUri);

            query = WebApiCommonQuery.GetAdInfoList(this.TheaterCode, this.ItemID, true);
            this.SpecialAdInfoList = WebApiHelper.GetResultJson<List<AdInfo>>(query, this.ServerUri);

            query = WebApiCommonQuery.GetSkinMapInfoList(this.TheaterCode, this.ItemID, (int)ContentsType.Adver);
            this.AdSkinMapInfoList = WebApiHelper.GetResultJson<List<SkinMapInfo>>(query, this.ServerUri);

            query = WebApiCommonQuery.GetSkinMapInfoList(this.TheaterCode, this.ItemID, (int)ContentsType.Media);
            this.MediaSkinMapInfoList = WebApiHelper.GetResultJson<List<SkinMapInfo>>(query, this.ServerUri);

            query = WebApiCommonQuery.GetSkinMapInfoList(this.TheaterCode, this.ItemID, (int)ContentsType.SpecialAdver);
            this.SpecialAdSkinMapInfoList = WebApiHelper.GetResultJson<List<SkinMapInfo>>(query, this.ServerUri);

            this.DownloadMessage = "미디어 파일을 확인 중입니다.";

            foreach (var mediaInfo in this.MediaInfoList)
            {
                groupInfoList.Add(mediaInfo);
            }

            if (groupInfoList.Count > 0)
            {
                CheckDownloadFiles(LOCAL_MEDIA_PATH, FTP_MEDIA_PATH, groupInfoList);
                groupInfoList.Clear();
            }

            this.DownloadMessage = "광고 파일을 확인 중입니다.";

            foreach (var adverInfo in this.AdInfoList)
            {
                groupInfoList.Add(adverInfo);
            }

            if (groupInfoList.Count > 0)
            {
                CheckDownloadFiles(LOCAL_AD_PATH, FTP_AD_PATH, groupInfoList);
                groupInfoList.Clear();
            }

            this.DownloadMessage = "스폐셜 광고 파일을 확인 중입니다.";

            foreach (var specialAdverInfo in this.SpecialAdInfoList)
            {
                groupInfoList.Add(specialAdverInfo);
            }

            if (groupInfoList.Count > 0)
            {
                CheckDownloadFiles(LOCAL_SPECIAL_AD_PATH, FTP_SPECIAL_AD_PATH, groupInfoList);
                groupInfoList.Clear();
            }

            this.DownloadMessage = "스킨 파일을 확인 중입니다.";

            List<ContentsInfo> contentsInfos = new List<ContentsInfo>();

            foreach (var adverSkinMapInfo in this.AdSkinMapInfoList)
            {
                foreach (var contentsInfo in adverSkinMapInfo.ContentsList)
                {
                    contentsInfos.Add(contentsInfo);
                }
            }

            foreach (var specialAdverSkinMapInfo in this.SpecialAdSkinMapInfoList)
            {
                foreach (var contentsInfo in specialAdverSkinMapInfo.ContentsList)
                {
                    contentsInfos.Add(contentsInfo);
                }
            }

            foreach (var mediaSkinMapInfo in this.MediaSkinMapInfoList)
            {
                foreach (var contentsInfo in mediaSkinMapInfo.ContentsList)
                {
                    contentsInfos.Add(contentsInfo);
                }
            }

            if (contentsInfos.Count > 0)
            {
                CheckDownloadFiles(LOCAL_SKIN_PATH, FTP_SKIN_PATH, contentsInfos);
            }

            if (this._downloadCallBack != null)
            {
                this._downloadCallBack.Invoke();
            }
        }

        protected override void OnDownloadFile(DownloadItem item)
        {
            if (!string.IsNullOrWhiteSpace(LOCAL_AD_PATH) && item.LocalPath.Contains(LOCAL_AD_PATH))
            {
                this.DownloadMessage = "광고를 다운로드 중입니다.";
            }
            else if (!string.IsNullOrWhiteSpace(LOCAL_SPECIAL_AD_PATH) && item.LocalPath.Contains(LOCAL_SPECIAL_AD_PATH))
            {
                this.DownloadMessage = "스폐셜 광고를 다운로드 중입니다.";
            }
            else if (!string.IsNullOrWhiteSpace(LOCAL_MEDIA_PATH) && item.LocalPath.Contains(LOCAL_MEDIA_PATH))
            {
                this.DownloadMessage = "미디어 파일을 다운로드 중입니다.";
            }
            else if (!string.IsNullOrWhiteSpace(LOCAL_SKIN_PATH) && item.LocalPath.Contains(LOCAL_SKIN_PATH))
            {
                this.DownloadMessage = "스킨 파일을 다운로드 중입니다.";
            }
            else
            {
                this.DownloadMessage = "컨텐츠를 다운로드 중입니다.";
            }

            if (this._downloadFileCallBack != null)
            {
                this._downloadFileCallBack.Invoke(item);
            }
        }

        #endregion

        #region Private Method

        private void CheckDownloadFiles(string localDirPath, string ftpDirPath, List<GroupInfo> groupInfos)
        {
            List<ContentsInfo> contentsInfoList = new List<ContentsInfo>();

            foreach (var groupInfo in groupInfos)
            {
                foreach (var contents in groupInfo.ContentsList)
                {
                    contentsInfoList.Add(contents);
                }
            }

            CheckDownloadFiles(localDirPath, ftpDirPath, contentsInfoList);
        }

        #endregion
    }
}