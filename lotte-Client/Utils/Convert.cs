using lotte_Client.Models.Data;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotte_Client.Utils
{
    public class Converter
    {
        public static ContentsType ToContentsType(string type)
        {
            if (type.ToLower() == "adver")
            {
                return ContentsType.Adver;
            }
            if (type.ToLower() == "media")
            {
                return ContentsType.Media;
            }

            return ContentsType.None;
        }

        public static List<Adver> ToAdverList(List<AdInfo> adverInfoList)
        {
            var adverList = new List<Adver>();

            foreach (var adverInfo in adverInfoList)
            {
                var adver = new Adver
                {
                    OrderNo = adverInfo.OrderNo,
                    ID = adverInfo.ID,
                    Title = adverInfo.Title,
                    Contents = adverInfo.ContentsList,
                    LayoutType = adverInfo.LayoutType,
                    SoundPosition = adverInfo.SoundPosition,
                };
                adverList.Add(adver);
            }

            return adverList;
        }

        public static ContentsType ToFileType(string fileType)
        {
            if (fileType.ToLower() == "video")
            {
                return ContentsType.Video;
            }
            if (fileType.ToLower() == "image")
            {
                return ContentsType.Image;
            }

            return ContentsType.None;
        }

        public static LayoutType ToLayoutType(string layoutType)
        {
            if (layoutType == "1")
            {
                return LayoutType.VideoImage;
            }

            return LayoutType.None;
        }
    }
}
