using LotteCinemaService.Model.Common.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.WebAPI.Helper
{
    public static class WebApiMBQuery
    {

        public static string InsertContent(MenuDIDInfoRaw content)
        {
            return string.Format("MB/SetContentsInfo?Conente={0}", content);

        }
        public static string DeleteContent(string ID)
        {
            return string.Format("MB/DeleteContentInfo?ID={0}", ID);
        }

        public static string GetItemByID(string ID)
        {
            return string.Format("MB/GetItemByID?ID={0}", ID);
        }


        public static string UpdateContent(MenuDIDInfoRaw content)
        {
            return string.Format("MB/UpdateContent?Content={0}", content);
        }
        public static string GetMenuDIDContentsList(string CinemaCode, string MenuDIDItem)
        {
            return string.Format("MB/ContentsInfo?CinemaCode={0}&MenuDIDItem={1}", CinemaCode, MenuDIDItem);
        }
        public static string InsertNewMenuboardItem(NewMenuInfoRaw content)
        {
            return string.Format("MB/InsertNewMenuInfo?content={0}", content);
        }

        public static string UpdateNewMenuboardItem(NewMenuInfoRaw content)
        {
            return string.Format("MB/UpdateNewMenuInfo?content={0}", content);
        }
        public static string InsertNewMenuboardSchedule(NewMenuScheduleInfoRaw content)
        {
            return string.Format("MB/InsertNewMenuScheduleInfo?content={0}", content);
        }
        public static string DeleteNewMenuboardSchedule(string Cinemacode, string DidItem)
        {
            return string.Format("MB/DeleteNewMenuScheduleInfo?CinemaCode={0}&DidItem={1}", Cinemacode, DidItem);
        }

        public static string GetNewMenuItem()
        {
            return string.Format("MB/NewMenuInfo");
        }

        public static string GetNewMenuSchedule(string CinemaCode)
        {

            return string.Format("MB/NewMenuScheduleInfo?CinemaCode={0}", CinemaCode);
        }
        public static string DeleteNewMenuItem(string id)
        {
            return string.Format("MB/DeleteNewMenuInfo?ID={0}", id);

        }
    }
}
