using System;
using System.Collections.Generic;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Common.Raw;
using System.Text;

namespace LotteCinemaService.WebAPI.Helper
{
    public static class WebApiCommonQuery
    {
        public static string GetAdTime()
        {
            return "Common/AdTime";
        }
         
        public static string GetCurrentTime()
        {
            return "Common/CurrentTime";
        }


        public static string GetMovieShowingContent(string theater,string playdate,string itemID)
        {
            return string.Format("Common/MovieShowingContent?theater={0}&playdate={1}&itemID={2}", theater, playdate, itemID);
        }

        public static string GetMovieContentsSetting(string theater, string itemID,string id)
        {
            return string.Format("Common/MovieContentsSetting?theater={0}&itemID={1}&id={2}", theater, itemID,id);
        }

        public static string GetMovieShowing(string theater, string itemID)
        {
            return string.Format("Common/MovieShowing?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetCheckMovieTimeList(string theater)
        {
            return string.Format("Common/MovieTime?theater={0}", theater);
        }

        public static string GetMovieInfo()
        {
            return string.Format("Common/MovieInfo");
        }

        public static string GetPatternInfoList(string theater, int itemID)
        {
            return string.Format("Common/PatternInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetMovieInfoList(string theater1, string theater2)
        {
            return string.Format("Common/MovieInfoList?theater1={0}&theater2={1}", theater1, theater2);
        }

        public static string GetAdInfoList(string theater, int itemID, bool isSpecial)
        {
            return string.Format("Common/AdInfoList?theater={0}&itemID={1}&isSpecial={2}", theater, itemID, isSpecial);
        }

        public static string GetTestAdInfoList(string theater, int itemID, bool isSpecial)
        {
            return string.Format("Common/TESTAdInfoList?theater={0}&itemID={1}&isSpecial={2}", theater, itemID, isSpecial);
        }

        public static string GetSubAdInfoList(string theater, int itemID)
        {
            return string.Format("Common/SubAdInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetAdInfo(string itemID, string contentsType,string theater)
        {
            return string.Format("Common/AdInfo?itemID={0}&contentsType={1}&theater={2}", itemID, contentsType,theater);
        }

        public static string GetMediaInfoList(string theater, int itemID)
        {
            return string.Format("Common/MediaInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetSkinMapInfoList(string theater, int itemID, int contentType)
        {
            return string.Format("Common/SkinMapInfoList?theater={0}&itemID={1}&contentType={2}", theater, itemID, contentType);
        }

        public static string GetTestSkinMapInfoList(string theater, int itemID, int contentType)
        {
            return string.Format("Common/TESTSkinMapInfoList?theater={0}&itemID={1}&contentType={2}", theater, itemID, contentType);
        }

        public static string GetNoticeInfoList(string theater, int itemID)
        {
            return string.Format("Common/NoticeInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetDigitalSignInfoList(string theater, int contentsType)
        {
            return string.Format("Common/DigitalSignInfoList?theater={0}&contentsType={1}&isVisible={2}", theater, contentsType.ToString("00"), "2");
        }

        public static string GetDigitalSignInfoItem(string noticeID)
        {
            return string.Format("DS/NoticeInfoItem?noticeID={0}", noticeID);
        }

        public static string GetPlannedMovieInfoList(string theater, int itemID)
        {
            return string.Format("Common/PlannedMovieInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetRecommandMovieInfoList(string theater, int itemID)
        {
            return string.Format("Common/RecommandMovieInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string DeletePlannedMovieInfoList(string theater, int itemID)
        {
            return string.Format("Common/PlannedMovieInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string DeleteMovieShowingInfoList(string theater, int itemID)
        {
            return string.Format("Common/MovieShowing?theater={0}&itemID={1}", theater, itemID);
        }

        public static string DeleteRecommandMovieInfoList(string theater, int itemID)
        {
            return string.Format("Common/RecommandMovieInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string DeleteSchedule(string theater, int itemID, string contentsType)
        {
            return string.Format("Common/ScheduleInfo?theater={0}&itemID={1}&contentsType={2}", theater, itemID, contentsType);
        }

        public static string DeleteCheckMovieTime(int id)
        {
            return string.Format("Common/CheckMovieTime?id={0}", id);
        }

        public static string DeleteAD(string id)
        {
            return string.Format("Common/AdInfo?id={0}", id);
        }

        public static string DeleteTransparentAD(string id)
        {
            return string.Format("Common/TransparentAdInfo?id={0}", id);
        }

        public static string DeleteMedia(string id)
        {
            return string.Format("Common/MediaInfo?id={0}", id);
        }

        public static string GetPopupNoticeInfo(string theater, int itemID)
        {
            return string.Format("Common/PopupNoticeInfo?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetMovieTimeCellInfoList(string theater1, string theater2)
        {
            return string.Format("Common/MovieTimeCellInfoList?theater1={0}&theater2={1}", theater1, theater2);
        }

        public static string GetTheaterFloorInfoList(string theater1, string theater2)
        {
            return string.Format("Common/TheaterFloorInfoList?theater1={0}&theater2={1}", theater1, theater2);
        }

        public static string GetEventInfoList(string theater, string itemID)
        {
            return string.Format("Common/EventInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetEventList(string theater, string itemID)
        {
            return string.Format("Common/EventList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetAccountList(string theater, string itemID,bool isSpecial,string BeginDate,string EndDate)
        {
            return string.Format("Common/AccountList?theater={0}&itemID={1}&isSpecial={2}&BeginDate={3}&EndDate={4}", theater, itemID, isSpecial,BeginDate,EndDate);
        }

        public static string GetTheaterGroupList()
        {
            return string.Format("Common/TheaterGroupList");
        }

        public static string GetTransparencyADAccountList(string theater,string BeginDate, string EndDate)
        {
            return string.Format("Common/TransparencyADAccountList?theater={0}&BeginDate={1}&EndDate={2}", theater,BeginDate, EndDate);
        }

        public static string InsertContent(ContentsInfoRaw content)
        {
            return string.Format("Common/SetContentsInfo?content={0}", content);
        }

        public static string UpdateContent(ContentsInfoRaw content)
        {
            return string.Format("Common/UpdateContentsInfo?content={0}", content);
        }
        public static string UpdateEventContent(ContentsInfoRaw content)
        {
            return string.Format("Common/UpdateEventContentsInfo?content={0}", content);
        }

        public static string GetSkinInfoList(string itemID,string ContentsType)
        {
            return string.Format("Common/SkinInfoList?ItemCode={0}&ContentsType={1}", itemID,ContentsType);
        }

        public static string InsertLimitedAD(AdInfoRaw ad)
        {
            return string.Format("Common/SetLimitedAdverInfo?ad={0}", ad);
        }

        public static string Insert_TransparentAD(AdInfoRaw ad)
        {
            return string.Format("Common/SetTransparentAdverInfo?ad={0}", ad);
        }

        public static string InsertAD(AdInfoRaw ad)
        {
            return string.Format("Common/SetAdverInfo?ad={0}", ad);
        }

        public static string InsertMedia(MediaInfoRaw media)
        {
            return string.Format("Common/MediaInfo?media={0}", media);
        }
        public static string InsertMedia1()
        {
            return string.Format("Common/Test");
        }

        public static string InsertSkin(SkinInfoRaw skin)
        {
            return string.Format("Common/SetSkinInfo?skin={0}", skin);
        }

        public static string UpdateSkin(SkinInfoRaw skin)
        {
            return string.Format("Common/UpdateSkinInfo?skin={0}", skin);
        }

        public static string InsertEventList(EventInfoRaw eventItem)
        {
            return string.Format("Common/SetEventInfo?eventItem={0}", eventItem);
        }

        public static string UpdateEventList(EventInfoRaw eventItem)
        {
            return string.Format("Common/UpdateEventInfo?eventItem={0}", eventItem);
        }

        public static string InsertDSItemList(DigitalSignInfoRaw dsItem)
        {
            return string.Format("Common/SetDigitalSignInfo?DSItem={0}", dsItem);
        }

          public static string ModifyDSItemList(DigitalSignInfoRaw dsItem)
        {
            return string.Format("Common/UpdateDigitalSignInfo?DSItem={0}", dsItem);
        }

        public static string InsertPopupItemList(PopupNoticeInfoRaw item)
        {
            return string.Format("Common/SetPopupNoticeInfo?item={0}", item);
        }

        public static string UpdatePopupItemList(PopupNoticeInfoRaw item)
        {
            return string.Format("Common/UpdatePopupNoticeInfo?item={0}", item);
        }

        public static string InsertNoticeList(NoticeInfoRaw notice)
        {
            return string.Format("Common/SetNoticeInfo?notice={0}", notice);
        }

        public static string UpdateNoticeList(NoticeInfoRaw notice)
        {
            return string.Format("Common/UpdateNoticeInfo?notice={0}", notice);
        }

        public static string DeleteNoticeList(string noticeID)
        {
            return string.Format("DS/NoticeInfo?noticeID={0}", noticeID);
        }
         public static string DeleteOneNoticeList(string noticeID)
        {
            return string.Format("Common/NoticeInfo?noticeID={0}", noticeID);
        }

        public static string DeletePopupNoticeList(string noticeID)
        {
            return string.Format("Common/PopupNoticeInfo?noticeID={0}", noticeID);
        }

        public static string UpdateNoticeUSEYN(string noticeID)
        {
            return string.Format("Common/UpdateNoticeUSEYN?noticeID={0}", noticeID);
        }

        public static string UpdateDB_DSNoticeUSEYN(string noticeID)
        {
            return string.Format("Common/UpdateDSNoticeUSEYN?noticeID={0}", noticeID);
        }

        public static string UpdateDB_PopupNoticeUSEYN(string noticeID)
        {
            return string.Format("Common/UpdatePopupNoticeUSEYN?noticeID={0}", noticeID);
        }

        public static string UpdateDigitalSignSkin(string skinID)
        {
            return string.Format("Common/UpdateDigitalSignSkin?skinID={0}", skinID);
        }

        public static string InsertSkinMap(SkinMapInfoRaw skinmap)
        {
            return string.Format("Common/SetSkinMapInfo?skinmap={0}", skinmap);
        }

        public static string UpdateSkinMap(SkinMapInfoRaw skinmap)
        {
            return string.Format("Common/UpdateSkinMapInfo?skinmap={0}", skinmap);
        }

        public static string InsertSchedule(ScheduleInfoRaw schedule)
        {
            return string.Format("Common/SetScheduleInfo?schedule={0}", schedule);
        }

        public static string GetContractInfoList(string itemCode, ContractInfoProcedure contract)
        {
            return string.Format("Common/ContractInfoList?itemCode={0}&AdvertiserName={1}&ContractID={2}&ContractName={3}", itemCode, contract.AdvertiserName, contract.ContractID, contract.ContractName);
        }

        public static string GetSiteInfo(string startChar)
        {
            return string.Format("Common/SiteInfo?startChar={0}", startChar);
        }

        public static string SetItemStatus()
        {
            return "Common/SetItemStatus";
        }

        public static string SetAdLog()
        {
            return "Common/SetAdLog";
        }

        public static string InsertPlannedMovie(PlannedMovieInfoRaw movie) 
        {
            return string.Format("Common/PlannedMovieInfoList?movie={0}", movie);
        }

        public static string InsertCheckMovieTime(MovieTimeCellInfo movietimecell)
        {
            return string.Format("Common/CheckMovieTimeInfo?movietimecell={0}", movietimecell);
        }

        public static string InsertMovieShowing(PlannedMovieInfoRaw movie)
        {
            return string.Format("Common/MovieShowing?movie={0}", movie);
        }

        public static string InsertRecommandMovie(RecommandedMovieInfoRaw movie)
        {
            return string.Format("Common/RecommandMovieInfoList?movie={0}", movie);
        }

        public static string GetContentInfo(int category,string itemID)
        {
            return string.Format("Common/ContentsInfo?category={0}&itemID={1}", category,itemID);
        }

        public static string GetContentInfo(string groupid)
        {
            return string.Format("Common/ContentsInfo?groupid={0}", groupid);
        }

        public static string GetScheduleInfo(string itemCode, string contentsType,string theater)
        {
            return string.Format("Common/ScheduleInfo?itemID={0}&contentsType={1}&theater={2}", itemCode,contentsType,theater);
        }

        public static string GetMediaInfo(string itemID, string theater)
        {
            return string.Format("Common/MediaInfo?itemID={0}&theater={1}", itemID, theater);
        }

        public static string GetCurtainContentsTime()
        {
            return string.Format("Common/CurtainContentsTime");
        }

        public static string getDigitalCurtainInfoList(int contentType)
        {
            return string.Format("DC/DigitalCurtainInfoList?contentType={0}",contentType);
        }

        public static string DigitalCurtainDate_Update(DigitalCurtainInfoRaw date)
        {
            return string.Format("DC/DigitalCurtainDate_Update?date={0}", date);
        }

        public static string DigitalCurtainWeather_Update(DigitalCurtainInfoRaw weather)
        {
            return string.Format("DC/DigitalCurtainWeather_Update?weather={0}", weather);
        }

        public static string ISSpecialImage_Insert(ISSpecialImageInfoRaw item)
        {
            return string.Format("IS/ISSpecialImage?item={0}", item);
        }

        public static string GetSpecialImageInfo(string theater, string itemID,DateTime begin,DateTime end)
        {
            return string.Format("IS/SpecialImageInfo?theater={0}&ItemID={1}&begin={2}&end={3}", theater, itemID, begin.ToString("yyyy-MM-dd 23:59"), end.ToString("yyyy-MM-dd 23:59"));
        }

        public static string GetISMenuInfo(string theater)
        {
            return string.Format("IS/ISMenuInfoList?theater={0}", theater);
        }

        public static string UpdateISMenuInfo(List<ISMenuInfoRaw> items)
        {
            return string.Format("IS/ISMenuInfoList?items={0}", items);
        }
        public static string UpdateDB_ESEventInfo(List<ESEventInfomationRawInfo> items)
        {
            return string.Format("Common/ESEventInfoList?items={0}", items);
        }


        

        public static string ISSpecialImage_Delete(string isID)
        {
            return string.Format("IS/SpecialImageInfo?isID={0}", isID);
        }

        public static string DeleteEventList(string eventid)
        {
            return string.Format("Common/EventInfo?eventid={0}", eventid);
        }

        public static string GetUserInfo(string id, string pw)
        {
            return string.Format("Common/UserInfo?id={0}&pw={1}", id,pw);
        }

        public static string DeleteSkin(string skinID)
        {
            return string.Format("Common/SkinInfo?skinID={0}", skinID);
        }

        public static string DeleteSkinMap(string Code)
        {
            return string.Format("Common/SkinMapInfo?Code={0}", Code);
        }

        public static string DeleteDGContent(string id)
        {
            return string.Format("DG/MediaInfo?id={0}", id);
        }

        public static string UpdateMedia(MediaInfoRaw media)
        {
            return string.Format("Common/UpdateMediaInfo?media={0}", media);
        }

        public static string GetPopupNoticeInfoItem(string noticeID)
        {
            return string.Format("Common/PopupNoticeInfoItem?noticeID={0}", noticeID);
        }

        public static string GetTransparentADInfoItem(string ID)
        {
            return string.Format("Common/TransparentAdInfoItem?id={0}", ID);
        }

        public static string GetADInfoItem(string ID)
        {
            return string.Format("Common/AdInfoItem?id={0}", ID);
        }

        public static string GetSkinInfo(string ID)
        {
            return string.Format("Common/SkinMapInfo?id={0}", ID);
        }

        public static string UpdateADContent(ContentsInfoRaw content)
        {
            return string.Format("Common/UpdateADContentsInfo?content={0}", content);
        }

        public static string UpdateAD(AdInfoRaw ad)
        {
            return string.Format("Common/UpdateAdverInfo?ad={0}", ad);
        }

        public static string UpdateADContentPositionInfo(ContentsInfoRaw content)
        {
            return string.Format("Common/UpdateADContentPositionInfo?content={0}", content);
        }

        public static string UpdateLimitedAD(AdInfoRaw ad)
        {
            return string.Format("Common/UpdateLimitedAdverInfo?ad={0}", ad);
        }

        public static string UpdateTransparentAD(AdInfoRaw ad)
        {
            return string.Format("Common/UpdateTransparentAdverInfo?ad={0}", ad);
        }

        public static string UpdateCheckMovieTime(bool checkStatus,int id)
        {
            return string.Format("Common/UpdateCheckMovieTime?checkStatus={0}&id={1}", checkStatus,id);
        }

        public static string GetMovieContentsInfoList()
        {
            return string.Format("Common/MovieContentsInfoList");
        }

        public static string SetMovieContentsInfo(MovieContentsInfoRaw info)
        {
            return string.Format("Common/SetMovieContentsInfo?info={0}", info);
        }

        public static string DeleteMovieContentsInfo(int seq)
        {
            return string.Format("Common/MovieContentsInfo?seq={0}", seq);
        }

        public static string GetMovieContentsUploadAvailableInfoList(DateTime startDate, DateTime endDate)
        {
            return string.Format("Common/MovieContentsUploadAvailableInfoList?startDate={0}&endDate={1}", startDate.ToShortDateString(), endDate.ToShortDateString());
        }

        public static string GetESEventInfomationList(string cinema01, string cinema02, string BeginDate, string EndDate)
        {
            return string.Format("Common/ESEventInfomationList?cinemaCode01={0}&cinemaCode02={1}&beginDate={2}&endDate={3}", cinema01,cinema02,BeginDate,EndDate);
        }

        public static string GetESEventInfomationLog(int seq)
        {
            return string.Format("Common/ESEventLog_List?seq={0}", seq);
        }

        public static string UpdateESEventInformation(ESEventInfomationRawInfo eventInfomationRawInfo)
        {
            return string.Format("Common/UpdateESEventInformation?eventInfomationRawInfo={0}", eventInfomationRawInfo);
        }

        public static string GetADStatusInfo(string ADCode, string Advertiser, string StartDate, string EndDate,int statusCode,string theater,string itemcode)
        {
            return string.Format("Common/ADStatusInfo?adcode={0}&advertiser={1}&startdate={2}&enddate={3}&statusCode={4}&theater={5}&itemcode={6}", ADCode, Advertiser, StartDate, EndDate,statusCode,theater,itemcode);
        }

        public static string GetADStatusInfo(string ADCode, string Advertiser, int StatusCode)
        {
            return string.Format("Common/ADStatusInfo?adcode={0}&advertiser={1}&StatusCode={2}", ADCode, Advertiser,StatusCode);
        }

        public static string UpdateMovieContentsSetting(string theater, string itemid, bool IsAuto,string id)
        {
            return string.Format("Common/UpdateMovieContentsSetting?code={0}{1}&isAuto={2}&id={3}", theater, itemid, IsAuto,id);
        }
    }
}