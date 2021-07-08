using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using LotteCinemaLibraries.Config;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Enum;
using LotteCinemaService.WebAPI.Common.Models;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class TMBController : ApiController
    {
        private CommonManager _cmManager;
        private TMBManager _tmbManager;

        public TMBController()
        {
            this._cmManager = new CommonManager(Settings.SERVER_DID_CONNECTION_STRING);
            this._tmbManager = new TMBManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        [GET("TMB/SubAdInfoList?{theater}")] 
        public List<AdInfo> GetSubAdInfoList(string theater)
        {
            List<AdInfo> list = new List<AdInfo>();

            var result = this._cmManager.GetScheduleList(theater, ((int)ItemID.TMB).ToString(), ContentsType.SubAdver);

            var groups = (from s in result
                          group s by new { s.OrderNo, s.ID, s.Title, s.LayoutType, s.SoundPosition, s.BeginDate, s.EndDate }).ToList();

            foreach (var group in groups)
            {
                AdInfo adverInfo = new AdInfo
                {
                    OrderNo = group.Key.OrderNo,
                    ID = group.Key.ID,
                    Title = group.Key.Title,
                    LayoutType = (LayoutType)group.Key.LayoutType,
                    SoundPosition = group.Key.SoundPosition,
                    ContentsList = new List<ContentsInfo>(),
                    BeginDate = group.Key.BeginDate,
                    EndDate = group.Key.EndDate
                };

                foreach (var item in group)
                {
                    ContentsInfo contentsInfo = new ContentsInfo
                    {
                        ContentsID = item.ContentsID,
                        GroupID = group.Key.ID,
                        FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                        FileType = item.FileType,
                        ItemPositionID = item.ItemPositionID,
                        ContentsType = item.ContentsType
                    };

                    var configFile = ConfigHelper.GetFilePath(group.Key.ID, contentsInfo.FileName, ItemID.TMB, ContentsType.SubAdver);
                    contentsInfo.FtpFilePath = configFile.FtpFilePath;
                    contentsInfo.LocalFilePath = configFile.LocalFilePath;

                    adverInfo.ContentsList.Add(contentsInfo);
                }

                list.Add(adverInfo);
            }

            result.Clear();
            return list;
        }

        [GET("TMB/AdInfoList?{theater}&{isSpecial}")]
        public List<AdInfo> GetAdInfoList(string theater, bool isSpecial)
        {
            List<AdInfo> adList = new List<AdInfo>();
            List<SkinMapInfo> skinList = new List<SkinMapInfo>();

            var itemID = ((int)ItemID.TMB).ToString();
            ContentsType type = isSpecial ? ContentsType.SpecialAdver : ContentsType.Adver;
            var skinResult = this._cmManager.GetSkinMapInfoList(theater, itemID, type);

            if (isSpecial)
            {
                var adResult = this._cmManager.GetAdInfoList(theater, itemID, ContentsType.SpecialAdver);

                var groups = (from s in adResult
                              group s by new { s.ID, s.Title, s.LayoutType, s.SoundPosition, s.BeginDate, s.EndDate }).ToList();

                foreach (var group in groups)
                {
                    AdInfo adverInfo = new AdInfo
                    {
                        ID = group.Key.ID,
                        Title = group.Key.Title,
                        LayoutType = group.Key.LayoutType,
                        SoundPosition = group.Key.SoundPosition,
                        BeginDate = group.Key.BeginDate,
                        EndDate = group.Key.EndDate,
                        ContentsList = new List<ContentsInfo>()
                    };

                    foreach (var item in group)
                    {
                        ContentsInfo contentsInfo = new ContentsInfo
                        {
                            ContentsID = item.ContentsID,
                            GroupID = group.Key.ID,
                            FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                            FileType = item.FileType,
                            FileSize = item.FileSize,
                            ItemPositionID = item.ItemPositionID,
                            ContentsType = item.ContentsType
                        };

                        var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, ItemID.TMB, type);

                        contentsInfo.FtpFilePath = configFile.FtpFilePath;
                        contentsInfo.LocalFilePath = configFile.LocalFilePath;

                        adverInfo.ContentsList.Add(contentsInfo);
                    }

                    adList.Add(adverInfo);
                }

                adResult.Clear();
            }
            else
            {
                var adResult = this._cmManager.GetScheduleList(theater, itemID, ContentsType.Adver);

                var adGroups = (from s in adResult
                                group s by new { s.OrderNo, s.ID, s.Title, s.LayoutType, s.SoundPosition, s.BeginDate, s.EndDate }).ToList();

                foreach (var group in adGroups)
                {
                    AdInfo adInfo = new AdInfo
                    {
                        OrderNo = group.Key.OrderNo,
                        ID = group.Key.ID,
                        Title = group.Key.Title,
                        LayoutType = (LayoutType)group.Key.LayoutType,
                        SoundPosition = group.Key.SoundPosition,
                        ContentsList = new List<ContentsInfo>(),
                        BeginDate = group.Key.BeginDate,
                        EndDate = group.Key.EndDate
                    };

                    foreach (var item in group)
                    {
                        ContentsInfo contentsInfo = new ContentsInfo
                        {
                            ContentsID = item.ContentsID,
                            GroupID = group.Key.ID,
                            FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                            FileType = item.FileType,
                            ItemPositionID = item.ItemPositionID,
                            ContentsType = item.ContentsType
                        };

                        var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, ItemID.TMB, type);

                        contentsInfo.FtpFilePath = configFile.FtpFilePath;
                        contentsInfo.LocalFilePath = configFile.LocalFilePath;

                        adInfo.ContentsList.Add(contentsInfo);
                    }

                    adList.Add(adInfo);
                }

                adResult.Clear();
            }
            
            var skinGroups = (from s in skinResult
                          group s by new { s.GroupID, s.SkinID }).ToList();

            foreach (var group in skinGroups)
            {
                SkinMapInfo skinMapInfo = new SkinMapInfo
                {
                    GroupID = group.Key.GroupID,
                    SkinID = group.Key.SkinID,
                    ContentsType = ContentsType.Skin,
                    ContentsList = new List<ContentsInfo>(),
                };

                foreach (var item in group)
                {
                    ContentsInfo contentsInfo = new ContentsInfo
                    {
                        ContentsID = item.ContentsID,
                        GroupID = group.Key.SkinID,
                        FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                        FileType = item.FileType,
                        ItemPositionID = item.ItemPositionID,
                        ContentsType = item.ContentsType
                    };

                    var configFile = ConfigHelper.GetFilePath(contentsInfo.GroupID, contentsInfo.FileName, ItemID.TMB, ContentsType.Skin);

                    contentsInfo.FtpFilePath = configFile.FtpFilePath;
                    contentsInfo.LocalFilePath = configFile.LocalFilePath;

                    skinMapInfo.ContentsList.Add(contentsInfo);
                }

                skinList.Add(skinMapInfo);
            }

            List<int> positionList = new List<int>();
            int itemCount = 5;

            foreach (var ad in adList)
            {
                if (ad.ContentsList.Count < itemCount)
                {
                    positionList.Clear();

                    foreach (var contents in ad.ContentsList)
                    {
                        if (!positionList.Contains(contents.ItemPositionID))
                        {
                            positionList.Add(contents.ItemPositionID);
                        }
                    }

                    for (int i = 1; i <= itemCount; i++)
                    {
                        if (!positionList.Contains(i))
                        {
                            var skin = (from s in skinList
                                       where s.GroupID == ad.ID
                                        select s).FirstOrDefault();

                            if (skin != null)
                            {
                                var contents = (from c in skin.ContentsList
                                                where c.ItemPositionID == i
                                                select c).FirstOrDefault();

                                if (contents != null)
                                {
                                    contents.GroupID = ad.ID;
                                    ad.ContentsList.Add(contents);
                                }
                                else
                                {
                                    ad.ContentsList.Add(new ContentsInfo { ItemPositionID = i });
                                }
                            }
                            else
                            {
                                ad.ContentsList.Add(new ContentsInfo { ItemPositionID = i });
                            }
                        }
                    }
                }
            }

            return adList;
        }

        [GET("TMB/SpecialType?{theater}")]
        public int GetSpecialType(string theater)
        {
            var result = this._cmManager.GetScheduleList(theater, ((int)ItemID.TMB).ToString(), ContentsType.Pattern);

            if (result.Count > 0)
            {
                if (result[0].ID == ((int)ContentsType.SpecialAdver).ToString())
                {
                    return result[0].OrderNo;
                }
            }

            return 0;
        }

        #region SetItemStatus ( 아이템 상태 )
        [HttpPost]
        [POST("TMB/SetItemStatus?{status}")]
        public void SetItemStatus(TMBItemStatusInfo status)
        {
            ItemStatusInfo itemStatus = new ItemStatusInfo
            {
                CinemaCode = status.CinemaCode,
                ScreenCode = status.ScreenCode,
                BeginTime = DateTime.Parse(status.BeginTime),
                ContentsID = status.ContentsID
            };

            this._cmManager.SetItemStatusInfo(itemStatus);
        }
        #endregion 

        [HttpPost]
        [POST("TMB/SetAdLog?{log}")]
        public bool SetAdLog(TMBAdLogInfo log)
        {
            AdLogInfo adLog = new AdLogInfo
            {
                CinemaCode = log.CinemaCode,
                ScreenCode = log.ScreenCode,
                BeginTime = DateTime.Parse(log.BeginTime),
                ContentsID = log.ContentsID,
                EndTime = DateTime.Parse(log.EndTime),
                AdID = log.AdID
            };

            return this._cmManager.SetAdLogInfo(adLog);
        }

        [NonAction]
        private void ResponseException()
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.NotModified);
            throw new HttpResponseException(response);
        }
    }
}