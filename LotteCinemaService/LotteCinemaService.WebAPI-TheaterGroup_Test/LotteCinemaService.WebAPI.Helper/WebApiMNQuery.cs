using LotteCinemaService.Model.Common.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.WebAPI.Helper
{
    public static class WebApiMNQuery
    {
        public static string SelectMiniNoticeItem()
        {
            return string.Format("MN/MiniNoticeInfo");
        }
        public static string InsertMiniNoticeItem(MiniNoticeInfoRaw content)
        {
            return string.Format("MN/InsertMiniNoticeInfo?content={0}", content);
        }
        public static string DeleteDatabaseItem(string id)
        {
            return string.Format("MN/DeleteMiniNoticeInfo?ID={0}", id);
        }
        
        public static string SelectMiniNoticeSchedule(string cinemacode)
        {
            return string.Format("MN/MiniNoticeScheduleInfo?CinemaCode={0}", cinemacode);
        }
        public static string InsertMiniNoticeSchedule(MiniNoticeScheduleInfoRaw content)
        {
            return string.Format("MN/InsertMiniNoticeScheduleInfo?content={0}", content);
        }
        public static string DeleteMiniNoticeSchedule(string CinemaCode, string DidItem)
        {
            return string.Format("MN/DeleteMiniNoticeScheduleInfo?CinemaCode={0}&DidItem={1}", CinemaCode, DidItem);
        }
    }
}
