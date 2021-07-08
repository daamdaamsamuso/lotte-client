using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Enum;
using LotteCinemaService.Model.TBA;
using System;
using System.Collections.Generic;
using System.Data;
using LotteCinemaService.WebAPI.Helper;
using LotteCinemaService.WebAPI.Helper.LCSM;

namespace LotteCinemaService.Database.Manager
{
    public class TBAManager : DatabaseManager
    {
        #region Variable

        private DateTimeEx _dt;

        #endregion

        public TBAManager(string didConnectionString) :
            base(didConnectionString)
        {
            this._dt = new DateTimeEx();
        }

        #region Public Method

        public List<string> GetCheckList()
        {
            List<string> listChecked = new List<string>();

            if (ConnectionDID())
            {
                using (var reader = this.mssql.StoredProcedure("VINYL_TBA_GetCheckScreenInfoList", null))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            listChecked.Add(DatabaseUtil.TryConvertToString(reader["id"]));
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return listChecked;
        }

        public bool SetAdverLogInfo(string adverInfoCode, string cinemaCode, int adverType, DateTime startTime, DateTime endTime, bool isBoxOffice)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "AdverMainInfoCode", adverInfoCode, 10);
                    parameterValues.Add(SqlDbType.VarChar, "CinemaCode", cinemaCode, 4);
                    parameterValues.Add(SqlDbType.Int, "AdverType", adverType, 4);
                    parameterValues.Add(SqlDbType.DateTime, "StartDate", startTime, startTime.ToString().Length);
                    parameterValues.Add(SqlDbType.DateTime, "EndDate", endTime, endTime.ToString().Length);

                    string procedureName = !isBoxOffice ? "lces_user.VINYL_AD_TBA_InsertADLogInfo" : "lces_user.VINYL_AD_TBA_InsertADLogInfo_Right";

                    using (this.mssql.StoredProcedure(procedureName, parameterValues))
                    {
                        DisConnection();
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool SetAdverUpdate(string adverInfoCode, string update, bool isBoxOffice)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "AdverMainInfoCode", adverInfoCode, 10);
                    parameterValues.Add(SqlDbType.VarChar, "UpdateID", update, 20);

                    var procedureName = !isBoxOffice ? "VINYL_AD_TBA_SetAdverMainInfoUpdate" : "VINYL_AD_TBA_SetAdverMainInfoUpdate_Right";

                    using (this.mssql.StoredProcedure(procedureName, parameterValues))
                    {
                        DisConnection();
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public List<ADInfo> GetAdverMainInfoCode(string cinemaCode, bool isBoxOffice)
        {
            List<ADInfo> adverMainInfo = new List<ADInfo>();

            if (ConnectionDID())
            {
                string spName = !isBoxOffice ? "VINYL_AD_TBA_GetAdverMainInfo" : "VINYL_AD_TBA_GetAdverMainInfo_Right";
                string codeName = !isBoxOffice ? "AdverMainInfoCode" : "AdverInfoCode";

                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "CinemaCode", cinemaCode, 4);

                using (var reader = this.mssql.StoredProcedure(spName, parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ADInfo adInfo = new ADInfo()
                            {
                                AdverMainInfoCode = DatabaseUtil.TryConvertToString(reader[codeName]),
                                AdverType = DatabaseUtil.TryConvertToInteger(reader["AdverType"]),
                                AccountCode = DatabaseUtil.TryConvertToInteger(reader["AccountCode"]),
                                UpdateID = DatabaseUtil.TryConvertToString(reader["UpdateID"]),
                                IsBoxOffice = isBoxOffice
                            };

                            adverMainInfo.Add(adInfo);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return adverMainInfo;
        }

        public DateTime GetTodayDateTime()
        {
            var dateTime = DateTime.Now;

            try
            {
                if (ConnectionDID())
                {
                    using (var reader = this.mssql.StoredProcedure("GetNowDateTime", null))
                    {
                        while (reader.Read())
                        {
                            dateTime = DatabaseUtil.TryConvertToDateTime(reader["datetime"]);
                        }

                        reader.Close();
                    }

                    DisConnection();
                }
                else
                {
                    dateTime = DateTime.Now;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                dateTime = DateTime.Now;
            }

            return dateTime;
        }

        /// <summary>
        /// 박스오피스 가져오기
        /// </summary>
        /// <returns>영화코드 리스트</returns>
        public List<BoxOfficeMovieInfo> GetBoxOfficeList()
        {
            List<BoxOfficeMovieInfo> infoList = new List<BoxOfficeMovieInfo>();

            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetBoxOfficeList(), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            var items = datasets.Tables[0].DataTableToList<MappedBoxOfficeMovieInfo>();

            if (items != null)
            {
                foreach (var item in items)
                {
                    var data = new BoxOfficeMovieInfo
                    {
                        MovieInfoCode = item.MovieInfoCode,
                        MovieTitle = item.TitleKor
                    };

                    infoList.Add(data);
                }
            }
            
            return infoList;
        }
        
        /// <summary>
        /// 상영예정작 가져오기
        /// </summary>
        /// <returns>영화코드 리스트</returns>
        public List<ForecastInfo> GetForecastList()
        {
            List<ForecastInfo> infoList = new List<ForecastInfo>();

            if (ConnectionDID())
            {
                using (var reader = mssql.StoredProcedure("VINYL_TBAInfo_GetForecastList", null))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ForecastInfo forecastInfo = new ForecastInfo
                            {
                                MovieCode = DatabaseUtil.TryConvertToString(reader["MovieCode"]),
                                MovieKoreaName = DatabaseUtil.TryConvertToString(reader["movieKoreaName"])
                            };

                            infoList.Add(forecastInfo);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return infoList;
        }

        /// <summary>
        /// 한줄공지 가져오기
        /// </summary>
        /// <returns>한줄공지 리스트</returns>
        public List<NoticeInfo> GetOneLineNotices(string cinemaCode1)
        {
            List<NoticeInfo> list_notice = new List<NoticeInfo>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "CinemaCode", cinemaCode1, 4);

                using (var reader = this.mssql.StoredProcedure("VINYL_OneLineNoticeList", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            NoticeInfo noticeInfo = new NoticeInfo()
                            {
                                Seq = DatabaseUtil.TryConvertToInteger(reader["Seq"]),
                                CinemaCode = DatabaseUtil.TryConvertToString(reader["cinemaCode"]),
                                CinemaName = DatabaseUtil.TryConvertToString(reader["cinemaName"]),
                                NoticeContent = DatabaseUtil.TryConvertToString(reader["NoticeContent"]),
                                CheckValue = DatabaseUtil.TryConvertToInteger(reader["CheckValue"])
                            };

                            list_notice.Add(noticeInfo);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list_notice;
        }

        /// <summary>
        /// 팝업공지 가져오기
        /// </summary>
        /// <returns>팝업 공지들</returns>
        public List<NoticeInfo> GetGeneralNotice(string cinemaCode1)
        {
            List<NoticeInfo> list_notice = new List<NoticeInfo>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "CinemaCode", cinemaCode1, 4);

                using (var reader = this.mssql.StoredProcedure("VINYL_PopupNoticeList", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            NoticeInfo noticeInfo = new NoticeInfo
                            {
                                Seq = DatabaseUtil.TryConvertToInteger(reader["Seq"]),
                                CinemaCode = DatabaseUtil.TryConvertToString(reader["cinemaCode"]),
                                CinemaName = DatabaseUtil.TryConvertToString(reader["cinemaName"]),
                                NoticeTitle = DatabaseUtil.TryConvertToString(reader["NoticeTitle"]),
                                NoticeContent = DatabaseUtil.TryConvertToString(reader["NoticeContent"]),
                                CheckValue = DatabaseUtil.TryConvertToInteger(reader["CheckValue"])
                            };

                            if (noticeInfo.CheckValue == 1)
                            {
                                list_notice.Add(noticeInfo);
                            }
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list_notice;
        }

        /// <summary>
        /// 층 정보 가져오기
        /// </summary>
        /// <param name="cinemaCode"></param>
        /// <param name="cinemaCode2"></param>
        /// <returns></returns>
        public List<FloorInfo> GetFloorInfoList(string cinemaCode, string cinemaCode2 = null)
        {
            List<FloorInfo> floorInoList = new List<FloorInfo>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "CinemaCode", cinemaCode);
                parameterValues.Add(SqlDbType.VarChar, "CinemaCode2", cinemaCode2);

                using (var reader = this.mssql.StoredProcedure("VINYL_CinemaFloor_Select", parameterValues))
                {
                    while (reader.Read())
                    {
                        FloorInfo floorInfo = new FloorInfo
                        {
                            Visible = DatabaseUtil.TryConvertToString(reader["Visible"]).ToUpper() == "Y",
                            CinemaCode = DatabaseUtil.TryConvertToString(reader["CinemaCode"]),
                            TheaterFloor = DatabaseUtil.TryConvertToInteger(reader["TheaterFloor"]),
                            TheaterNumber = DatabaseUtil.TryConvertToInteger(reader["TheaterNumber"])
                        };

                        floorInoList.Add(floorInfo);
                    }

                    reader.Close();
                }

                DisConnection();
            }

            return floorInoList;
        }

        /// <summary>
        /// 투명 광고 리스트 가져오기
        /// </summary>
        /// <param name="cinemaCode"></param>
        /// <param name="cinemaCode2"></param>
        /// <returns></returns>
        public List<TransparentAdInfoProcedure> GetTransparentAdInfoList(string cinemaCode)
        {
            List<TransparentAdInfoProcedure> transparentAdInoList = new List<TransparentAdInfoProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "CienamCode", cinemaCode, 10);

                using (var reader = this.mssql.StoredProcedure("DID_TransparentAdInfoList_SELECT", parameterValues))
                {
                    while (reader.Read())
                    {
                        TransparentAdInfoProcedure adInfo = new TransparentAdInfoProcedure
                        {
                            ID = DatabaseUtil.TryConvertToString(reader["ID"]),
                            Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                            BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]),
                            EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]),
                            Account = DatabaseUtil.TryConvertToInteger(reader["Account"]),
                            ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]),
                            Category = (CategoryType)DatabaseUtil.TryConvertToInteger(reader["Category"]),
                            FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                            FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                            FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"]),
                            ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                            ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"]),
                        };

                        transparentAdInoList.Add(adInfo);
                    }

                    reader.Close();
                }

                DisConnection();
            }

            return transparentAdInoList;
        }

        #endregion
    }
}