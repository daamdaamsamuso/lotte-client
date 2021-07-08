using System;
using System.Collections.Generic;
using System.Data;
using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.TBA;

namespace LotteCinemaService.Database.TBA
{
    public class TBAManager : DatabaseManager
    {
        #region Variable

        private DateTimeEx _dt;

        #endregion

        public TBAManager(string ticketConnectionString, string webConnectionString, string didConnectionString) :
            base(ticketConnectionString, webConnectionString, didConnectionString)
        {
            this._dt = new DateTimeEx();
        }

        #region Public Method

        public List<string> GetCheckList()
        {
            List<string> listChecked = new List<string>();

            if (ConnectionWeb())
            {
                using (var sdr = this.mssql.StoredProcedure("VINYL_TBA_GetCheckScreenInfoList", null))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            listChecked.Add(sdr["id"] as string);
                        }

                        sdr.Close();
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
                if (ConnectionWeb())
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
                if (ConnectionWeb())
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

            if (ConnectionWeb())
            {
                string spName = !isBoxOffice ? "VINYL_AD_TBA_GetAdverMainInfo" : "VINYL_AD_TBA_GetAdverMainInfo_Right";
                string codeName = !isBoxOffice ? "AdverMainInfoCode" : "AdverInfoCode";

                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "CinemaCode", cinemaCode, 4);

                using (var sdr = this.mssql.StoredProcedure(spName, parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            ADInfo adInfo = new ADInfo()
                            {
                                AdverMainInfoCode = sdr[codeName] as string,
                                AdverType = int.Parse(sdr["AdverType"].ToString()),
                                AccountCode = int.Parse(sdr["AccountCode"].ToString()),
                                UpdateID = sdr["UpdateID"] as string,
                                IsBoxOffice = isBoxOffice
                            };

                            adverMainInfo.Add(adInfo);
                        }

                        sdr.Close();
                    }
                }

                DisConnection();
            }

            return adverMainInfo;
        }

        public AdTime GetAdverTimeReference(bool isBoxOffice)
        {
            AdTime adverTimeReference = new AdTime();

            if (ConnectionWeb())
            {
                string procedureName = !isBoxOffice ? "VINYL_AdverTimeReference_List" : "VINYL_AdverTimeReference_List_Right";

                using (var reader = this.mssql.StoredProcedure(procedureName, null))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                adverTimeReference.StartTime = reader["StartTime"].ToString();
                                adverTimeReference.EndTime = reader["EndTime"].ToString();
                                adverTimeReference.SecondInterval = Convert.ToInt32(reader["SecondInterval"].ToString());
                            }
                            catch
                            {
                                adverTimeReference.StartTime = "09:00";
                                adverTimeReference.EndTime = "26:00";
                                adverTimeReference.SecondInterval = 90;
                            }
                        }

                        reader.Close();
                    }
                    else
                    {
                        adverTimeReference.StartTime = "09:00";
                        adverTimeReference.EndTime = "26:00";
                        adverTimeReference.SecondInterval = 90;
                    }
                }

                DisConnection();
            }

            return adverTimeReference;
        }

        public DateTime GetTodayDateTime()
        {
            var dateTime = DateTime.Now;

            try
            {
                if (ConnectionTicket())
                {
                    using (var reader = this.mssql.StoredProcedure("GetNowDateTime", null))
                    {
                        while (reader.Read())
                        {
                            dateTime = Convert.ToDateTime(reader["datetime"]);
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

            if (ConnectionWeb())
            {
                using (var sdr = mssql.StoredProcedure("VINYL_TBAInfo_GetBoxOfficeList", null))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            BoxOfficeMovieInfo item = new BoxOfficeMovieInfo
                            {
                                MovieInfoCode = sdr["MovieInfoCode"] as string,
                                MovieTitle = sdr["TitleKor"] as string
                            };

                            infoList.Add(item);
                        }

                        sdr.Close();
                    }
                }

                DisConnection();
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

            if (ConnectionWeb())
            {
                using (var sdr = mssql.StoredProcedure("VINYL_TBAInfo_GetForecastList", null))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            ForecastInfo forecastInfo = new ForecastInfo
                            {
                                MovieCode = sdr["MovieCode"] as string,
                                MovieKoreaName = sdr["movieKoreaName"] as string
                            };

                            infoList.Add(forecastInfo);
                        }

                        sdr.Close();
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

            if (ConnectionWeb())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "CinemaCode", cinemaCode1, 4);

                using (var sdr = this.mssql.StoredProcedure("VINYL_OneLineNoticeList", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            NoticeInfo noticeInfo = new NoticeInfo()
                            {
                                Seq = int.Parse(sdr["Seq"].ToString()),
                                CinemaCode = sdr["cinemaCode"] as string,
                                CinemaName = sdr["cinemaName"] as string,
                                NoticeContent = sdr["NoticeContent"] as string,
                                CheckValue = int.Parse(sdr["CheckValue"].ToString())
                            };

                            list_notice.Add(noticeInfo);
                        }

                        sdr.Close();
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

            if (ConnectionWeb())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "CinemaCode", cinemaCode1, 4);

                using (var sdr = this.mssql.StoredProcedure("VINYL_PopupNoticeList", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            NoticeInfo noticeInfo = new NoticeInfo
                            {
                                Seq = int.Parse(sdr["Seq"].ToString()),
                                CinemaCode = sdr["cinemaCode"] as string,
                                CinemaName = sdr["cinemaName"] as string,
                                NoticeTitle = sdr["NoticeTitle"] as string,
                                NoticeContent = sdr["NoticeContent"] as string,
                                CheckValue = int.Parse(sdr["CheckValue"].ToString())
                            };

                            if (noticeInfo.CheckValue == 1)
                            {
                                list_notice.Add(noticeInfo);
                            }
                        }

                        sdr.Close();
                    }
                }

                DisConnection();
            }

            return list_notice;
        }

        public List<FloorInfo> GetFloorInfoList(string cinemaCode, string cinemaCode2 = null)
        {
            List<FloorInfo> floorInoList = new List<FloorInfo>();

            if (ConnectionWeb())
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
                            Visible = Convert.ToString(reader["Visible"]).ToUpper() == "Y",
                            CinemaCode = Convert.ToString(reader["CinemaCode"]),
                            TheaterFloor = Convert.ToInt32(reader["TheaterFloor"]),
                            TheaterNumber = Convert.ToInt32(reader["TheaterNumber"])
                        };

                        floorInoList.Add(floorInfo);
                    }

                    reader.Close();
                }

                DisConnection();
            }

            return floorInoList;
        }

        #endregion
    }
}