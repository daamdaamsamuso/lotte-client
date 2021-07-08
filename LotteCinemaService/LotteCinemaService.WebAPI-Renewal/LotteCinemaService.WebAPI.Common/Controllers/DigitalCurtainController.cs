using System.Diagnostics;
using AttributeRouting.Web.Http;
using LotteCinemaLibraries.Config;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;
using LotteCinemaService.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class DigitalCurtainController : ApiController
    {
        private DigitalCurtainManager _dcManager;

        public DigitalCurtainController()
        {
            this._dcManager = new DigitalCurtainManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        [GET("DC/DigitalCurtainInfoList?{contentType}")]
        public List<DigitalCurtainInfo> GetDigitalCurtainInfo(int contentType)
        {
            try
            {
                List<DigitalCurtainInfo> list = new List<DigitalCurtainInfo>();

                var result = this._dcManager.GetDigitalCurtainInfo((ContentsType)contentType);

                if (contentType == (int)ContentsType.Weather || contentType == (int)ContentsType.WeatherDefault)
                {
                    var groups = (from s in result
                                  group s by new { s.WeatherType, s.ContentsName, s.MorningBeginDate, s.MorningEndDate }).ToList();

                    foreach (var group in groups)
                    {
                        DigitalCurtainInfo digitalCurtainInfo = new DigitalCurtainInfo
                        {
                            WeatherType = group.Key.WeatherType,
                            ContentsName = group.Key.ContentsName,
                            MorningBeginDate = group.Key.MorningBeginDate,
                            MorningEndDate = group.Key.MorningEndDate,
                            ContentsList = new List<ContentsInfo>(),
                        };

                        foreach (var item in group)
                        {
                            ContentsInfo contentsInfo = new ContentsInfo
                            {
                                ContentsID = 0,
                                GroupID = item.GroupID,
                                FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                                FileType = item.FileType,
                                FileSize = item.FileSize,
                                ItemPositionID = item.ItemPositionID,
                                ContentsType = item.ContentsType
                            };

                            contentsInfo.FtpFilePath = Path.Combine(Config.FTP_DC_MEDIA_PATH, contentsInfo.GroupID.ToString(), contentsInfo.FileName);
                            contentsInfo.LocalFilePath = Path.Combine(Config.LOCAL_DC_MEDIA_PATH, contentsInfo.GroupID.ToString(), contentsInfo.FileName);

                            digitalCurtainInfo.ContentsList.Add(contentsInfo);
                        }

                        list.Add(digitalCurtainInfo);
                    }

                }
                else if (contentType == (int)ContentsType.WeatherTime)
                {
                    var groups = (from s in result
                                  group s by new { s.MorningBeginDate, s.MorningEndDate }).ToList();

                    foreach (var group in groups)
                    {
                        DigitalCurtainInfo digitalCurtainInfo = new DigitalCurtainInfo
                        {
                            MorningBeginDate = group.Key.MorningBeginDate,
                            MorningEndDate = group.Key.MorningEndDate
                        };

                        list.Add(digitalCurtainInfo);
                    }
                }

                result.Clear();

                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        [HttpPut]
        [PUT("DC/DigitalCurtainDate_Update?{date}")]
        public bool UpdateDigitalCurtainDate(DigitalCurtainInfoRaw date)
        {
            return this._dcManager.UpdateDigitalCurtainDate(date);
        }

        [HttpPut]
        [PUT("DC/DigitalCurtainWeather_Update?{date}")]
        public bool UpdateDigitalCurtainWeather(DigitalCurtainInfoRaw weather)
        {
            return this._dcManager.UpdateDigitalCurtainWeather(weather);
        }
    }
}