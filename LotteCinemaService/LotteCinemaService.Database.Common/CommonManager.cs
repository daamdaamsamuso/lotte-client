using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;
using LotteCinemaService.WebAPI.Helper;
using LotteCinemaService.WebAPI.Helper.LCSM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace LotteCinemaService.Database.Manager
{
    public class CommonManager : DatabaseManager
    {
        #region Variable

        public DateTimeEx _dt = new DateTimeEx();

        #endregion

        #region Public Method

        #region Constructor
        public CommonManager(string didConnectionString) :
            base(didConnectionString)
        {
        }
        #endregion

        #region Select Method

        #region 000GroupTest

        public List<TheaterGroupInfoRaw> GetTheaterGroupList()
        {
            List<TheaterGroupInfoRaw> groupList = new List<TheaterGroupInfoRaw>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                using (var sdr = this.mssql.StoredProcedure("TEST_DID_TheaterGroup_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            TheaterGroupInfoRaw item = new TheaterGroupInfoRaw
                            {
                                ID = sdr["ID"] as string,
                                Name = sdr["Name"] as string
                            };
                            groupList.Add(item);
                        }
                        sdr.Close();
                    }
                    DisConnection();
                }
            }
            return groupList;
        }

        #endregion

        #region ExistsMember

        public bool ExistsMember(string id)
        {
            bool exists = false;

            if (ConnectionDID())
            {
                // EXEC ExistsMember 'dudfufl'
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "ID", id, 20);

                using (var reader = this.mssql.StoredProcedure("ExistsMember_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            exists = DatabaseUtil.TryConvertToChar(reader["Exists"]) == '1';
                        }

                        reader.Close();
                    }

                    DisConnection();
                }
            }

            return exists;
        }

        #endregion

        #region GetMovieInfo

        public List<PlannedMovieInfoRaw> GetMovieInfo()
        {
            var list = new List<PlannedMovieInfoRaw>();

            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetMovieInfo(), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            if (datasets.Tables.Count > 0)
            {
                var items = datasets.Tables[0].DataTableToList<MappedPlannedMovieInfoRaw_MovieInfo>();

                if (items != null)
                {
                    list.AddRange(items.Select(item => new PlannedMovieInfoRaw
                    {
                        Title = item.Title, 
                        MovieCode = item.MovieCode,

                    }));
                }
            }

            ////////////////////////////////////////////////////////////////////
            // !!리스트 개수 만큼 컨텐츠 코드 조회함
            // !!속도가 너무 느리면 MovieContentsInfoList 한번에 받아와서 비교문으로 변경 해야함
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];

                if (string.IsNullOrEmpty(item.MovieCode))
                {
                    list.Remove(item);
                }
                else
                {
                    var movieContentsInfoItems = GetLCSMMovieContentsInfoList(item.MovieCode);
                    if (movieContentsInfoItems.Count > 0)
                    {
                        if (string.IsNullOrEmpty(movieContentsInfoItems[0].LargePosterFileName)
                            && string.IsNullOrEmpty(movieContentsInfoItems[0].SmallPosterFileName)
                            && string.IsNullOrEmpty(movieContentsInfoItems[0].MovieFileName))
                        {
                            list.Remove(item);
                        }
                        else
                        {
                            list[i].LargePoster = !string.IsNullOrEmpty(movieContentsInfoItems[0].LargePosterFileName);
                            list[i].SmallPoster = !string.IsNullOrEmpty(movieContentsInfoItems[0].SmallPosterFileName);
                            list[i].Video = !string.IsNullOrEmpty(movieContentsInfoItems[0].MovieFileName);
                        }

                    }
                    else
                    {
                        list.Remove(item);
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////

            return list;
        }

        #endregion

        #region GetMovieShowingContent

        public List<PlannedMovieInfoRaw> GetMovieShowingContent(string theater, string playPlan, string itemID)
        {
            var list = new List<PlannedMovieInfoRaw>();

            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetMovieShowingContent(theater, playPlan, itemID), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            if (datasets.Tables.Count > 0)
            {
                var items = datasets.Tables[0].DataTableToList<MappedPlannedMovieInfoRaw_MovieShowingContent>();

                if (items != null)
                {
                    list.AddRange(items.Select(item => new PlannedMovieInfoRaw
                    {
                        MovieCode = item.MovieCode,
                        ContentsCode = item.ContentsCode,
                        Title = item.MovieShortName,
                    }));
                }

                ////////////////////////////////////////////////////////////////////
                // !!리스트 개수 만큼 컨텐츠 코드 조회함
                // !!속도가 너무 느리면 MovieContentsInfoList 한번에 받아와서 비교문으로 변경 해야함
                foreach (var item in list)
                {
                    var movieContentsInfoItems = GetLCSMMovieContentsInfoList(item.ContentsCode);
                    if (movieContentsInfoItems.Count > 0)
                    {
                        item.LargePoster = DatabaseUtil.TryConvertIsNullToBool(movieContentsInfoItems[0].LargePosterFileName);
                        item.SmallPoster = DatabaseUtil.TryConvertIsNullToBool(movieContentsInfoItems[0].SmallPosterFileName);
                        item.Video = DatabaseUtil.TryConvertIsNullToBool(movieContentsInfoItems[0].MovieFileName);
                    }
                }
                ////////////////////////////////////////////////////////////////////
            }

            return list;
        }

        #endregion

        #region GetCheckMovieTimeList

        public List<CheckMovieTimeInfoRaw> GetCheckMovieTimeList(string theater)
        {
            List<CheckMovieTimeInfoRaw> list = new List<CheckMovieTimeInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                using (var sdr = this.mssql.StoredProcedure("DID_CheckMovieTimeInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            CheckMovieTimeInfoRaw checkmovietime = new CheckMovieTimeInfoRaw
                            {
                                Seq = DatabaseUtil.TryConvertToInteger(sdr["Seq"]),
                                CinemaCode = DatabaseUtil.TryConvertToString(sdr["CinemaCode"]),
                                CinemaName = DatabaseUtil.TryConvertToString(sdr["CinemaName"]),
                                MovieCode = DatabaseUtil.TryConvertToString(sdr["MovieCode"]),
                                MovieKoreaName = DatabaseUtil.TryConvertToString(sdr["MovieKoreaName"]),
                                ScreenCode = DatabaseUtil.TryConvertToString(sdr["ScreenCode"]),
                                ScreenName = DatabaseUtil.TryConvertToString(sdr["ScreenName"]),
                                StartTime = DatabaseUtil.TryConvertToString(sdr["StartTime"]),
                                UseYN = DatabaseUtil.TryConvertCharToBool(sdr["UseYN"]),
                            };
                            list.Add(checkmovietime);
                        }

                        sdr.Close();
                    }

                    DisConnection();
                }
            }
            return list;
        }

        #endregion

        #region GetMovieShowing

        public List<PlannedMovieInfoRaw> GetMovieShowing(string theater, string itemID)
        {
            List<PlannedMovieInfoRaw> list = new List<PlannedMovieInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.VarChar, "ItemID", itemID, 3);
                using (var sdr = this.mssql.StoredProcedure("DID_MovieShowingInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            PlannedMovieInfoRaw movie = new PlannedMovieInfoRaw
                            {
                                OrderNo = DatabaseUtil.TryConvertToInteger(sdr["OrderNo"]),
                                Theater = DatabaseUtil.TryConvertToString(sdr["Theater"]),
                                ContentsCode = DatabaseUtil.TryConvertToString(sdr["ContentsCode"]),
                                Title = DatabaseUtil.TryConvertToString(sdr["Title"])
                            };

                            list.Add(movie);
                        }

                        sdr.Close();
                    }

                    DisConnection();
                }
            }
            return list;
        }

        #endregion

        #region GetCurrentTime
        public DateTime GetCurrentTime()
        {
            DateTime currentTime = DateTime.Now;

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();

                using (var sdr = this.mssql.StoredProcedure("DID_CurrentTime_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            currentTime = DateTime.Parse(sdr[0].ToString());
                        }

                        sdr.Close();
                    }
                }

                DisConnection();
            }

            return currentTime;
        }
        #endregion

        #region GetScheduleInfo
        public List<ScheduleInfoRaw> GetScheduleInfo(string itemID, string contentsType, string theater)
        {
            List<ScheduleInfoRaw> list = new List<ScheduleInfoRaw>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                parameterValues.Add(SqlDbType.Char, "ContentsType", contentsType, 2);
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);

                using (var reader = this.mssql.StoredProcedure("DID_ScheduleInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ScheduleInfoRaw schedule = new ScheduleInfoRaw();
                            schedule.ContentsType = DatabaseUtil.TryConvertToString(reader["ContentsType"]);
                            schedule.ID = DatabaseUtil.TryConvertToString(reader["ID"]);
                            schedule.ItemID = DatabaseUtil.TryConvertToString(reader["ItemID"]);
                            schedule.OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]);
                            schedule.Theater = DatabaseUtil.TryConvertToString(reader["Theater"]);
                            schedule.Title = DatabaseUtil.TryConvertToString(reader["Title"]);
                            schedule.IsFull = DatabaseUtil.TryConvertToString(reader["IsFull"]) == "1" ? true : false;
                            list.Add(schedule);
                        }

                        reader.Close();
                    }
                }
                DisConnection();
            }
            return list;
        }

        #endregion

        #region GetMovieContentsSetting

        public bool GetMovieContentsSetting(string theater, string itemid, string id)
        {
            bool isAuto = true;
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemid, 3);
                parameterValues.Add(SqlDbType.VarChar, "ID", id, 100);

                using (var reader = this.mssql.StoredProcedure("DID_MovieContentsSettingInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            isAuto = DatabaseUtil.TryConvertCharToBool(reader["IsAuto"]);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return isAuto;
        }

        #endregion

        #region GetMovieInfoList
        public List<MovieInfo> GetMovieInfoList(string cinemaCode1, string cinemaCode2)
        {
            SaveDateTime();
            List<MovieInfo> returnValue = new List<MovieInfo>();
            //_dt.Datetime = "2016-08-02";
            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetMovieShowingList(_dt.Datetime, cinemaCode1, cinemaCode2), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            var items = datasets.Tables[0].DataTableToList<MappedMovieShowingInfo>();
            foreach (var item in items)
            {
                var data = new MovieInfo
                {
                    MovieCode = item.movieCode,
                    ContentsCode = item.contentsCode,
                    ID = item.id,
                    MovieName = item.movieKoreaName,
                    PermissionLevel = item.permissionLevel,
                    ScreenName = item.screenName,
                    ScreenDivCode = item.ScreenDivCd,
                    ScreenCode = item.screenCode,
                    ScreenFloor = item.screenFloor,
                    FilmCode = item.filmCode.ToString(),
                    CaptionCode = item.captionCode.ToString(),
                    FourDTypeCode = item.FourDTypeCode,
                    WatchCount = item.WatchCount == "" ? 0 : int.Parse(item.WatchCount),
                    BookingCount = item.BookingCount == "" ? 0 : int.Parse(item.BookingCount)
                };
                returnValue.Add(data);
            }
            return returnValue;
        }
        #endregion


        public List<MovieTimeCellInfo> GetMovieTimeCellInfoList(string cinemaCode1, string cinemaCode2)
        {
            SaveDateTime();
            var cellList = new List<MovieTimeCellInfo>();
            //_dt.Datetime = "2016-08-02";
            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetMovieTimeCellInfoList(_dt.Datetime, cinemaCode1, cinemaCode2), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            var items = datasets.Tables[0].DataTableToList<MappedMovieTimeCellInfo>();

            foreach (var item in items)
            {
                var data = new MovieTimeCellInfo
                {
                    CinemaCode = item.cinemaCode.ToString("0000"),
                    PlayDate = item.playDate.ToString("yyyy-MM-dd"),
                    MovieCode = item.movieCode,
                    ShowSeq = item.showSeq.ToString(),
                    MovieKoreaName = item.movieKoreaName,
                    ScreenDivCode = item.ScreenDivCd,
                    ScreenName = item.screenName,
                    ScreenCode = item.screenCode.ToString(),
                    ScreenFloor = item.screenFloor,
                    ID = item.id,
                    ContentsCode = item.contentsCode,
                    StartTime = item.startTime.Count() == 4?string.Format("{0}{1}:{2}{3}",item.startTime[0],item.startTime[1],item.startTime[2],item.startTime[3]):"00:00",
                    EndTime = item.endTime.Count() == 4 ? string.Format("{0}{1}:{2}{3}", item.endTime[0], item.endTime[1], item.endTime[2], item.endTime[3]) : "00:00",
                    EventCode = item.EventCode.ToString(),
                    TicketCode = item.ticketCode,
                    FilmCode = item.filmCode.ToString(),
                    FourDTypeCode = item.FourDTypeCode,
                    SeatCount = item.seatCnt,
                    LeftSeat = item.leftseat,
                    IsCheck = false
                };

                cellList.Add(data);
            }

            return cellList;
        }

        #region GetScheduleList
        public List<ScheduleProcedure> GetScheduleList(string theater, string itemID, ContentsType type)
        {
            List<ScheduleProcedure> list = new List<ScheduleProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)type).ToString("00"), 2);

                using (var reader = this.mssql.StoredProcedure("DID_ScheduleInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ScheduleProcedure item = new ScheduleProcedure();

                            if (type == ContentsType.Adver || type == ContentsType.SubAdver)
                            {
                                item.OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]);
                                item.ID = DatabaseUtil.TryConvertToString(reader["ID"]);
                                item.ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]);
                                item.SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]);
                                item.Title = DatabaseUtil.TryConvertToString(reader["Title"]);
                                item.LayoutType = DatabaseUtil.TryConvertToInteger(reader["LayoutType"]);
                                item.ScheduleType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ScheduleType"]);
                                item.AdType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["AdType"]);
                                item.FileName = DatabaseUtil.TryConvertToString(reader["FileName"]);
                                item.ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]);
                                item.Category = (CategoryType)DatabaseUtil.TryConvertToInteger((reader["Category"]));
                                item.FileType = DatabaseUtil.TryConvertToString(reader["FileType"]);
                                item.ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"]);
                                item.BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]);
                                item.EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]);
                            }
                            else if (type == ContentsType.Media)
                            {
                                if (itemID == ((int)ItemID.DigitalCurtain).ToString())
                                {
                                    item.OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]);
                                    item.ID = DatabaseUtil.TryConvertToString(reader["ID"]);
                                }
                                else
                                {
                                    item.OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]);
                                    item.ID = DatabaseUtil.TryConvertToString(reader["ID"]);
                                    item.ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]);
                                    item.SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]);
                                    item.Title = DatabaseUtil.TryConvertToString(reader["Title"]);
                                    item.LayoutType = DatabaseUtil.TryConvertToInteger(reader["LayoutType"]);
                                    item.ScheduleType =
                                        (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ScheduleType"]);
                                    item.FileName = DatabaseUtil.TryConvertToString(reader["FileName"]);
                                    item.Category = (CategoryType)DatabaseUtil.TryConvertToInteger((reader["Category"]));
                                    item.ContentsType =
                                        (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]);
                                    item.FileType = DatabaseUtil.TryConvertToString(reader["FileType"]);
                                    item.ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"]);
                                }
                            }
                            else if (type == ContentsType.Pattern)
                            {
                                item.OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]);
                                item.ID = DatabaseUtil.TryConvertToString(reader["ID"]);
                                item.IsFull = DatabaseUtil.TryConvertToString(reader["IsFull"]);
                            }

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetTESTScheduleList
        public List<ScheduleProcedure> GetTESTScheduleList(string theater, string itemID, ContentsType type)
        {
            List<ScheduleProcedure> list = new List<ScheduleProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)type).ToString("00"), 2);

                using (var reader = this.mssql.StoredProcedure("TEST_DID_ScheduleInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ScheduleProcedure item = new ScheduleProcedure();

                            if (type == ContentsType.Adver || type == ContentsType.SubAdver)
                            {
                                item.OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]);
                                item.ID = DatabaseUtil.TryConvertToString(reader["ID"]);
                                item.ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]);
                                item.SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]);
                                item.Title = DatabaseUtil.TryConvertToString(reader["Title"]);
                                item.LayoutType = DatabaseUtil.TryConvertToInteger(reader["LayoutType"]);
                                item.ScheduleType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ScheduleType"]);
                                item.AdType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["AdType"]);
                                item.FileName = DatabaseUtil.TryConvertToString(reader["FileName"]);
                                item.ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]);
                                item.Category = (CategoryType)DatabaseUtil.TryConvertToInteger((reader["Category"]));
                                item.FileType = DatabaseUtil.TryConvertToString(reader["FileType"]);
                                item.ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"]);
                                item.BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]);
                                item.EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]);
                            }
                            else if (type == ContentsType.Media)
                            {
                                if (itemID == ((int)ItemID.DigitalCurtain).ToString())
                                {
                                    item.OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]);
                                    item.ID = DatabaseUtil.TryConvertToString(reader["ID"]);
                                }
                                else
                                {
                                    item.OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]);
                                    item.ID = DatabaseUtil.TryConvertToString(reader["ID"]);
                                    item.ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]);
                                    item.SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]);
                                    item.Title = DatabaseUtil.TryConvertToString(reader["Title"]);
                                    item.LayoutType = DatabaseUtil.TryConvertToInteger(reader["LayoutType"]);
                                    item.ScheduleType =
                                        (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ScheduleType"]);
                                    item.FileName = DatabaseUtil.TryConvertToString(reader["FileName"]);
                                    item.Category = (CategoryType)DatabaseUtil.TryConvertToInteger((reader["Category"]));
                                    item.ContentsType =
                                        (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]);
                                    item.FileType = DatabaseUtil.TryConvertToString(reader["FileType"]);
                                    item.ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"]);
                                }
                            }
                            else if (type == ContentsType.Pattern)
                            {
                                item.OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]);
                                item.ID = DatabaseUtil.TryConvertToString(reader["ID"]);
                            }

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetAdInfoList
        public List<AdInfoProcedure> GetAdInfoList(string theater, string itemID, ContentsType type)
        {
            List<AdInfoProcedure> list = new List<AdInfoProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)type).ToString("00"), 2);

                using (var reader = this.mssql.StoredProcedure("DID_AdInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            AdInfoProcedure item = new AdInfoProcedure
                            {
                                Account = DatabaseUtil.TryConvertToInteger(reader["Account"]),
                                ID = DatabaseUtil.TryConvertToString(reader["ID"]).Trim(),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                LayoutType = (LayoutType)DatabaseUtil.TryConvertToInteger(reader["LayoutType"]),
                                SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]),
                                BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]),
                                EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]),
                                ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]),
                                Category = (CategoryType)DatabaseUtil.TryConvertToInteger(reader["Category"]),
                                FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                                FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"]),
                                FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                                ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetTESTAdInfoList
        public List<AdInfoProcedure> GetTESTAdInfoList(string theater, string itemID, ContentsType type)
        {
            List<AdInfoProcedure> list = new List<AdInfoProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)type).ToString("00"), 2);

                using (var reader = this.mssql.StoredProcedure("TEST_DID_AdInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            AdInfoProcedure item = new AdInfoProcedure
                            {
                                Account = DatabaseUtil.TryConvertToInteger(reader["Account"]),
                                ID = DatabaseUtil.TryConvertToString(reader["ID"]).Trim(),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                LayoutType = (LayoutType)DatabaseUtil.TryConvertToInteger(reader["LayoutType"]),
                                SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]),
                                BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]),
                                EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]),
                                ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]),
                                Category = (CategoryType)DatabaseUtil.TryConvertToInteger(reader["Category"]),
                                FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                                FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"]),
                                FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                                ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetAdInfoItem

        public AdInfoRaw GetAdInfoItem(string id)
        {
            AdInfoRaw item = new AdInfoRaw();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "ID", id, 20);

                using (var reader = this.mssql.StoredProcedure("DID_AdInfoID_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            item = new AdInfoRaw
                            {
                                ID = DatabaseUtil.TryConvertToString(reader["ID"]),
                                ItemID = DatabaseUtil.TryConvertToString(reader["ItemID"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                Account = DatabaseUtil.TryConvertToString(reader["Account"]),
                                BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]),
                                EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]),
                                ContractID = DatabaseUtil.TryConvertToString(reader["ContractID"]),
                                SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                                LayoutType = (LayoutType)DatabaseUtil.TryConvertToInteger(reader["LayoutType"]),
                                TheaterName = DatabaseUtil.TryConvertToString(reader["CinemaName"])
                            };
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return item;
        }

        #endregion

        #region GetTransparentAdInfoItem

        public AdInfoRaw GetTransparentAdInfoItem(string id)
        {
            AdInfoRaw item = new AdInfoRaw();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "ID", id, 20);

                using (var reader = this.mssql.StoredProcedure("DID_TransparentAdInfoID_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            item = new AdInfoRaw
                            {
                                ID = DatabaseUtil.TryConvertToString(reader["ID"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                Account = DatabaseUtil.TryConvertToString(reader["Account"]),
                                BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]),
                                EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]),
                                ContractID = DatabaseUtil.TryConvertToString(reader["ContractID"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                TheaterName = DatabaseUtil.TryConvertToString(reader["CinemaName"])
                            };
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return item;
        }

        #endregion

        #region GetSkinInfoItem

        public string GetSkinMapInfoItem(string id)
        {
            string skinID = string.Empty;
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "GroupID", id, 20);

                using (var reader = this.mssql.StoredProcedure("DID_SkinMapInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            skinID = DatabaseUtil.TryConvertToString(reader["SkinID"]);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return skinID;
        }

        #endregion

        #region GetAdInfo

        public List<AdInfoRaw> GetAdItemInfo(string itemID, string contentsType)
        {
            List<AdInfoRaw> list = new List<AdInfoRaw>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "ContentsType", contentsType, 2);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                using (var reader = this.mssql.StoredProcedure("DID_AdInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            AdInfoRaw item = new AdInfoRaw
                            {
                                ID = DatabaseUtil.TryConvertToString(reader["ID"]),
                                ItemID = DatabaseUtil.TryConvertToString(reader["ItemID"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                Account = DatabaseUtil.TryConvertToString(reader["Account"]),
                                BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]),
                                EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]),
                                ContractID = DatabaseUtil.TryConvertToString(reader["ContractID"]),
                                SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                TheaterName = DatabaseUtil.TryConvertToString(reader["TheaterName"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }

        public bool GetSpecialTypeInfo()
        {
            var result = false;
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "ContentsType", "04", 2);
                parameterValues.Add(SqlDbType.Char, "ItemID", "202", 3);
                parameterValues.Add(SqlDbType.VarChar, "Theater", "1016", 10);
                using (var reader = this.mssql.StoredProcedure("DID_AdInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result = DatabaseUtil.TryConvertToString(reader["IsFull"]) == "True" ? true : false;
                            break;
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return result;
        }


        #endregion

        #region GetAdInfo

        public List<AdInfoRaw> GetAdInfo(string itemID, string contentsType, string theater)
        {
            List<AdInfoRaw> list = new List<AdInfoRaw>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
         

                if (itemID == ((int)ItemID.TBATransparent).ToString("000"))
                {
                    parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);

                    using (var reader = this.mssql.StoredProcedure("DID_TransparentAdInfo_SELECT", parameterValues))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AdInfoRaw item = new AdInfoRaw
                                {
                                    ID = DatabaseUtil.TryConvertToString(reader["ID"]),
                                    Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                    Account = DatabaseUtil.TryConvertToString(reader["Account"]),
                                    BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]),
                                    EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]),
                                    ContractID = DatabaseUtil.TryConvertToString(reader["ContractID"]),
                                    Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                    TheaterName = DatabaseUtil.TryConvertToString(reader["TheaterName"])
                                };

                                list.Add(item);
                            }

                            reader.Close();
                        }
                    }

                    DisConnection();
                }
                else
                {
                    parameterValues.Add(SqlDbType.VarChar, "ContentsType", contentsType, 2);
                    parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                    using (var reader = this.mssql.StoredProcedure("DID_AdInfo_SELECT", parameterValues))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AdInfoRaw item = new AdInfoRaw
                                {
                                    ID = DatabaseUtil.TryConvertToString(reader["ID"]),
                                    ItemID = DatabaseUtil.TryConvertToString(reader["ItemID"]),
                                    Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                    Account = DatabaseUtil.TryConvertToString(reader["Account"]),
                                    BeginDate = DatabaseUtil.TryConvertToDateTime(reader["BeginDate"]),
                                    EndDate = DatabaseUtil.TryConvertToDateTime(reader["EndDate"]),
                                    ContractID = DatabaseUtil.TryConvertToString(reader["ContractID"]),
                                    SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]),
                                    Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                    TheaterName = DatabaseUtil.TryConvertToString(reader["TheaterName"])
                                };

                                list.Add(item);
                            }

                            reader.Close();
                        }
                    }

                    DisConnection();
                }
            }


            return list;
        }

        #endregion

        #region GetMediaInfo

        public List<MediaInfoRaw> GetMediaInfo(string itemID, string theater)
        {
            List<MediaInfoRaw> list = new List<MediaInfoRaw>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                using (var reader = this.mssql.StoredProcedure("DID_MediaInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            MediaInfoRaw item = new MediaInfoRaw
                            {
                                ID = DatabaseUtil.TryConvertToString(reader["ID"]),
                                ItemID = DatabaseUtil.TryConvertToString(reader["ItemID"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                LayoutType = DatabaseUtil.TryConvertToString(reader["LayoutType"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }

        #endregion

        #region GetNoticeInfoList
        public List<NoticeInfoRaw> GetNoticeInfoList(string theater, string itemID)
        {
            List<NoticeInfoRaw> list = new List<NoticeInfoRaw>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                using (var reader = this.mssql.StoredProcedure("DID_NoticeInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            NoticeInfoRaw item = new NoticeInfoRaw
                            {
                                NoticeID = DatabaseUtil.TryConvertToString(reader["NoticeID"]),
                                ItemID = DatabaseUtil.TryConvertToString(reader["ItemID"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                Text = DatabaseUtil.TryConvertToString(reader["Text"]),
                                IsVisible = DatabaseUtil.TryConvertToChar(reader["IsVisible"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetDigitalSignNoticeInfoList
        public List<DigitalSignInfoRaw> GetDigitalSignNoticeInfoList(string theater, string contentsType, string isVisible)
        {
            List<DigitalSignInfoRaw> list = new List<DigitalSignInfoRaw>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ContentsType", contentsType, 2);
                parameterValues.Add(SqlDbType.Char, "IsVisible", isVisible, 1);

                using (var reader = this.mssql.StoredProcedure("DID_DigitalSignInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            DigitalSignInfoRaw item = new DigitalSignInfoRaw
                            {
                                NoticeID = DatabaseUtil.TryConvertToString(reader["NoticeID"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                                Text = DatabaseUtil.TryConvertToString(reader["Text"]),
                                FontFamily = DatabaseUtil.TryConvertToString(reader["FontFamily"]),
                                CharacterColor = DatabaseUtil.TryConvertToString(reader["CharacterColor"]),
                                CharacterBold = DatabaseUtil.TryConvertToString(reader["CharacterBold"]),
                                SkinID = DatabaseUtil.TryConvertToString(reader["SkinID"]),
                                IconFileName = DatabaseUtil.TryConvertToString(reader["IconFileName"]),
                                Visible = DatabaseUtil.TryConvertToChar(reader["IsVisible"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetPlannedMovieInfoList
        public List<PlannedMovieInfoRaw> GetPlannedMovieInfoList(string theater, string itemID)
        {
            List<PlannedMovieInfoRaw> list = new List<PlannedMovieInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                using (var reader = this.mssql.StoredProcedure("DID_PlannedMovieInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            PlannedMovieInfoRaw item = new PlannedMovieInfoRaw
                            {
                                Seq = DatabaseUtil.TryConvertToInteger(reader["Seq"]),
                                OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                MovieCode = DatabaseUtil.TryConvertToString(reader["MovieCode"]),
                                ContentsCode = DatabaseUtil.TryConvertToString(reader["MovieCode"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                ItemID = DatabaseUtil.TryConvertToString(reader["ItemID"]),
                                RegDate = DatabaseUtil.TryConvertToDateTime(reader["RegDate"]),
                                RegID = DatabaseUtil.TryConvertToString(reader["RegID"]),
                                UpdateDate = DatabaseUtil.TryConvertToDateTime(reader["UpdateDate"]),
                                UpdateRegID = DatabaseUtil.TryConvertToString(reader["RegID"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetPopupNoticeInfoList
        public List<PopupNoticeInfoProcedure> GetPopupNoticeInfoList(string theater, string itemID)
        {
            List<PopupNoticeInfoProcedure> list = new List<PopupNoticeInfoProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                using (var reader = this.mssql.StoredProcedure("DID_PopupNoticeInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            PopupNoticeInfoProcedure item = new PopupNoticeInfoProcedure
                            {
                                FontFamily = DatabaseUtil.TryConvertToString(reader["FontFamily"]),
                                Header = DatabaseUtil.TryConvertToString(reader["Header"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                Body = DatabaseUtil.TryConvertToString(reader["Body"]),
                                HeaderCharacterColor = DatabaseUtil.TryConvertToString(reader["HeaderCharacterColor"]),
                                HeaderCharacterBold = DatabaseUtil.TryConvertToString(reader["HeaderCharacterBold"]),
                                TitleCharacterColor = DatabaseUtil.TryConvertToString(reader["TitleCharacterColor"]),
                                TitleCharacterBold = DatabaseUtil.TryConvertToString(reader["TitleCharacterBold"]),
                                BodyCharacterColor = DatabaseUtil.TryConvertToString(reader["BodyCharacterColor"]),
                                BodyCharacterBold = DatabaseUtil.TryConvertToString(reader["BodyCharacterBold"]),
                                SkinID = DatabaseUtil.TryConvertToString(reader["SkinID"]),
                                ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]),
                                FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                                FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"]),
                                FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                                ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetPopupNoticeInfoList
        public List<PopupNoticeInfoRaw> GetPopupNoticeInfo(string theater, string itemID)
        {
            List<PopupNoticeInfoRaw> list = new List<PopupNoticeInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                using (var reader = this.mssql.StoredProcedure("DID_PopupNoticeInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            PopupNoticeInfoRaw item = new PopupNoticeInfoRaw
                            {
                                NoticeID = DatabaseUtil.TryConvertToString(reader["NoticeID"]),
                                SkinID = DatabaseUtil.TryConvertToString(reader["SkinID"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                ItemID = DatabaseUtil.TryConvertToString(reader["ItemID"]),
                                FontFamily = DatabaseUtil.TryConvertToString(reader["FontFamily"]),
                                Header = DatabaseUtil.TryConvertToString(reader["Header"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                Body = DatabaseUtil.TryConvertToString(reader["Body"]),
                                HeaderCharacterColor = DatabaseUtil.TryConvertToString(reader["HeaderCharacterColor"]),
                                HeaderCharacterBold = DatabaseUtil.TryConvertToString(reader["HeaderCharacterBold"]),
                                TitleCharacterColor = DatabaseUtil.TryConvertToString(reader["TitleCharacterColor"]),
                                TitleCharacterBold = DatabaseUtil.TryConvertToString(reader["TitleCharacterBold"]),
                                BodyCharacterColor = DatabaseUtil.TryConvertToString(reader["BodyCharacterColor"]),
                                BodyCharacterBold = DatabaseUtil.TryConvertToString(reader["BodyCharacterBold"]),
                                IsVisible = DatabaseUtil.TryConvertToChar(reader["IsVisible"]),
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetContentsInfo
        public List<ContentsInfoRaw> GetContentsInfo(int category, string itemID)
        {
            List<ContentsInfoRaw> list = new List<ContentsInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Int, "Category", category);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 10);

                using (var reader = this.mssql.StoredProcedure("ContentsInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ContentsInfoRaw item = new ContentsInfoRaw
                            {
                                ContentsName = DatabaseUtil.TryConvertToString(reader["ContentsName"]),
                                FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"]),
                                GroupID = DatabaseUtil.TryConvertToString(reader["GroupID"])

                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetSkinMapInfoList
        public List<SkinMapInfoProcedure> GetSkinMapInfoList(string theater, string itemID, ContentsType contentType)
        {
            List<SkinMapInfoProcedure> list = new List<SkinMapInfoProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)contentType).ToString("00"), 2);
                parameterValues.Add(SqlDbType.VarChar, "TodayDate", "2014-07-24", 20);

                using (var reader = this.mssql.StoredProcedure("DID_SkinMapInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            SkinMapInfoProcedure item = new SkinMapInfoProcedure
                            {
                                ContentsName = DatabaseUtil.TryConvertToString(reader["ContentsName"]),
                                FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                                FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"]),
                                FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                                GroupID = DatabaseUtil.TryConvertToString(reader["GroupID"]),
                                ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"]),
                                SkinID = DatabaseUtil.TryConvertToString(reader["SkinID"]),
                                ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]),
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetTESTSkinMapInfoList
        public List<SkinMapInfoProcedure> GetTESTSkinMapInfoList(string theater, string itemID, ContentsType contentType)
        {
            List<SkinMapInfoProcedure> list = new List<SkinMapInfoProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);
                parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)contentType).ToString("00"), 2);
                parameterValues.Add(SqlDbType.VarChar, "TodayDate", "2014-07-24", 20);

                using (var reader = this.mssql.StoredProcedure("TEST_DID_SkinMapInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            SkinMapInfoProcedure item = new SkinMapInfoProcedure
                            {
                                ContentsName = DatabaseUtil.TryConvertToString(reader["ContentsName"]),
                                FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                                FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"]),
                                FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                                GroupID = DatabaseUtil.TryConvertToString(reader["GroupID"]),
                                ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"]),
                                SkinID = DatabaseUtil.TryConvertToString(reader["SkinID"]),
                                ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]),
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetTheaterFloorInfoList
        public List<TheaterFloorInfoRaw> GetTheaterFloorInfoList(string theater1, string theater2 = "")
        {
            List<TheaterFloorInfoRaw> list = new List<TheaterFloorInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater1", theater1, 10);
                parameterValues.Add(SqlDbType.VarChar, "Theater2", theater2, 10);

                using (var reader = this.mssql.StoredProcedure("DID_TheaterFloorInfo_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            TheaterFloorInfoRaw item = new TheaterFloorInfoRaw
                            {
                                Seq = DatabaseUtil.TryConvertToInteger(reader["Seq"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                FloorNo = DatabaseUtil.TryConvertToInteger(reader["FloorNo"]),
                                ScreenCode = DatabaseUtil.TryConvertToString(reader["ScreenCode"]),
                                ScreenName = DatabaseUtil.TryConvertToString(reader["ScreenName"]),
                                UseYN = DatabaseUtil.TryConvertToChar(reader["UseYN"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetRecommandMovieInfoList
        public List<RecommandedMovieInfoRaw> GetRecommandMovieInfoList(string itemID)
        {
            List<RecommandedMovieInfoRaw> list = new List<RecommandedMovieInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                using (var reader = this.mssql.StoredProcedure("LCSM_RecommandedMovieInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            RecommandedMovieInfoRaw item = new RecommandedMovieInfoRaw
                            {
                                Seq = DatabaseUtil.TryConvertToInteger(reader["Seq"]),
                                OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]),
                                Theater = DatabaseUtil.TryConvertToString(reader["Theater"]),
                                MovieCode = DatabaseUtil.TryConvertToString(reader["MovieCode"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                ItemID = DatabaseUtil.TryConvertToString(reader["ItemID"]),
                                RegDate = DatabaseUtil.TryConvertToDateTime(reader["RegDate"]),
                                RegID = DatabaseUtil.TryConvertToString(reader["RegID"]),
                                UpdateDate = DatabaseUtil.TryConvertToDateTime(reader["UpdateDate"]),
                                UpdateRegID = DatabaseUtil.TryConvertToString(reader["RegID"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }
            return list;
        }
        #endregion

        #region GetRecommandedMovieInfoList
        public List<RecommandedMovieInfoProcedure> GetRecommandedMovieInfoList(string itemID)
        {
            List<RecommandedMovieInfoProcedure> list = new List<RecommandedMovieInfoProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                using (var reader = this.mssql.StoredProcedure("LCSM_RecommandedMovieInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            RecommandedMovieInfoProcedure item = new RecommandedMovieInfoProcedure
                            {
                                OrderNo = DatabaseUtil.TryConvertToInteger(reader["OrderNo"]),
                                MovieCode = DatabaseUtil.TryConvertToString(reader["MovieCode"]),
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            foreach(var item in list)
            {
                var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetRecommandMovieInfoList(item.MovieCode), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
                var val = datasets.Tables[0].DataTableToList<MappedRecommandedMovieInfoProcedure>();
                if(val.Count !=0)
                {
                    item.Title = val[0].TitleKor;
                    item.RunningTime = val[0].PlayTime;
                    item.Country = val[0].CommCdNm;
                    item.Genre = val[0].CommCdNm1;
                    item.Direction = val[0].Direction;
                    item.Casts = val[0].CastsProducer;
                    item.WatchClass = val[0].ViwGrdCd;
                    item.OpenDate = DateTime.Parse(val[0].RelsDt);
                }
            }
            return list;
        }
        #endregion

        #region GetEventInfoList
        public List<EventInfoProcedure> GetEventInfoList(string theater, string itemID)
        {
            List<EventInfoProcedure> list = new List<EventInfoProcedure>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 10);

                using (var reader = this.mssql.StoredProcedure("DID_EventInfoList_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            EventInfoProcedure item = new EventInfoProcedure
                            {
                                EventID = DatabaseUtil.TryConvertToString(reader["EventID"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                SoundPosition = DatabaseUtil.TryConvertToInteger(reader["SoundPosition"]),
                                LayoutType = (LayoutType)DatabaseUtil.TryConvertToInteger(reader["LayoutType"]),
                                ContentsID = DatabaseUtil.TryConvertToInteger(reader["ContentsID"]),
                                FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                                FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                                FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"]),
                                ContentsType = (ContentsType)DatabaseUtil.TryConvertToInteger(reader["ContentsType"]),
                                ItemPositionID = DatabaseUtil.TryConvertToInteger(reader["ItemPositionID"])
                            };

                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;

        }
        #endregion

        #region GetAccountList
        public List<AccountInfoProcedure> GetAccountList(string theater, string itemID, bool isSpecial, string BeginDate, string EndDate)
        {
            List<AccountInfoProcedure> AccountList = new List<AccountInfoProcedure>();
            List<AccountInfoProcedure> list = new List<AccountInfoProcedure>();

            for (int i = 1; i <= 15; i++)
            {
                AccountList.Add(new AccountInfoProcedure
                {
                    Account = i.ToString("00")
                });
            }

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 10);
                parameterValues.Add(SqlDbType.VarChar, "BeginDate", BeginDate+" 00:00", 20);
                parameterValues.Add(SqlDbType.VarChar, "EndDate", EndDate+" 23:00", 20);
                if (isSpecial)
                {
                    parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)ContentsType.SpecialAdver).ToString("00"), 2);

                }
                else
                {
                    parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)ContentsType.Adver).ToString("00"), 2);

                }

                using (var reader = this.mssql.StoredProcedure("DID_Account_SELECT", parameterValues))
                {

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            if ((reader["Account"] as string) != null)
                            {

                                foreach (AccountInfoProcedure item in AccountList)
                                {
                                    if (item.Account.Equals(reader["Account"] as string))
                                    {
                                        list.Add(item);
                                    }
                                }
                            }

                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return AccountList.Except(list).ToList();
        }
        #endregion

        #region GetTransparencyADAccountList
        public List<AccountInfoProcedure> GetTransparencyADAccountList(string theater, string BeginDate, string EndDate)
        {
            List<AccountInfoProcedure> AccountList = new List<AccountInfoProcedure>();
            List<AccountInfoProcedure> list = new List<AccountInfoProcedure>();

            for (int i = 1; i <= 15; i++)
            {
                AccountList.Add(new AccountInfoProcedure
                {
                    Account = i.ToString("00")
                });
            }

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.VarChar, "BeginDate", BeginDate+" 00:00", 20);
                parameterValues.Add(SqlDbType.VarChar, "EndDate", EndDate+" 23:00", 20);
                using (var reader = this.mssql.StoredProcedure("DID_TransparencyADAccount_SELECT", parameterValues))
                {

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            if ((reader["Account"] as string) != null)
                            {

                                foreach (AccountInfoProcedure item in AccountList)
                                {
                                    if (item.Account.Equals(reader["Account"] as string))
                                    {
                                        list.Add(item);
                                    }
                                }
                            }

                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return AccountList.Except(list).ToList();
        }
        #endregion

        #region GetSkinInfoList
        public List<SkinInfoRaw> GetSkinInfoList(string itemID, string ContentsType)
        {
            List<SkinInfoRaw> skinList = new List<SkinInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "ItemID", itemID, 3);
                parameterValues.Add(SqlDbType.Char, "ContentsType", ContentsType, 2);

                using (var sdr = this.mssql.StoredProcedure("DID_SkinInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            SkinInfoRaw skin = new SkinInfoRaw
                            {
                                ID = sdr["ID"] as string,
                                Title = sdr["Title"] as string,
                                ItemID = sdr["ItemID"] as string

                            };
                            skinList.Add(skin);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
            }
            return skinList;
        }
        #endregion

        #region GetEventList
        public List<EventInfoRaw> GetEventList(string theater, string itemID)
        {
            List<EventInfoRaw> eventList = new List<EventInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                using (var sdr = this.mssql.StoredProcedure("DID_EventInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            EventInfoRaw eventItem = new EventInfoRaw
                            {
                                EventID = sdr["EventID"] as string,
                                Title = sdr["Title"] as string,
                                Theater = sdr["Theater"] as string,
                                ItemID = sdr["ItemID"] as string,
                                LayoutType = int.Parse(sdr["LayoutType"].ToString())
                            };
                            eventList.Add(eventItem);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
            }
            return eventList;
        }
        #endregion

        #region GetContractList
        public List<ContractInfoProcedure> GetContractList(string itemCode, string AdvertiserName, string ContractID, string ContractName)
        {
            List<ContractInfoProcedure> contractList = new List<ContractInfoProcedure>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();

                if (ContractName == null || ContractName.Equals(string.Empty))
                {
                    parameterValues.Add(SqlDbType.VarChar, "ContractName", DBNull.Value);
                }
                else
                {
                    parameterValues.Add(SqlDbType.VarChar, "ContractName", ContractName, 100);
                }

                if (AdvertiserName == null || AdvertiserName.Equals(string.Empty))
                {
                    parameterValues.Add(SqlDbType.VarChar, "AdvertiserName", DBNull.Value);
                }
                else
                {
                    parameterValues.Add(SqlDbType.VarChar, "AdvertiserName", AdvertiserName, 50);
                }

                if (ContractID == null || ContractID.Equals(string.Empty))
                {
                    parameterValues.Add(SqlDbType.VarChar, "ContractID", DBNull.Value);
                }
                else
                {
                    parameterValues.Add(SqlDbType.VarChar, "ContractID", ContractID, 100);
                }

                using (var sdr = this.mssql.StoredProcedure("DID_ContractInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            ContractInfoProcedure contractInfo = new ContractInfoProcedure
                            {
                                ContractID = int.Parse(sdr["ContractID"].ToString()).ToString(),
                                ContractName = sdr["ContractName"] as string,
                                AdvertiserID = int.Parse(sdr["AdvertiserID"].ToString()).ToString(),
                                ContractDate = sdr["ContractDate"] as string,
                                BeginDate = sdr["BeginDate"] as string,
                                EndDate = sdr["EndDate"] as string,
                                AdvertiserName = sdr["AdvertiserName"] as string
                            };
                            contractList.Add(contractInfo);
                        }
                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
            }
            return contractList;
        }
        #endregion

        #region GetUserPaidAmount

        public int GetUserPaidAmount(string userID)
        {
            int amount = 0;

            if (ConnectionDID())
            {
                // EXEC spGetUserPaidAmount '1', '회원아이디'
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Int, "paramType", 1);
                parameterValues.Add(SqlDbType.VarChar, "inputParam", userID, 30);

                using (var reader = this.mssql.StoredProcedure("[lcnew-dsi].lcnew.lces_user.spGetUserPaidAmount", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            amount = DatabaseUtil.TryConvertToInteger(reader["shareAmtSum"]);
                        }

                        reader.Close();
                    }

                    DisConnection();
                }
            }

            return amount;
        }

        #endregion

        #region GetTheaterName

        public string GetTheaterName(int code)
        {
            string CinemaName = string.Empty;
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Int, "CinemaCode", code);

                using (var sdr = this.mssql.StoredProcedure("CinemaName_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            CinemaName = DatabaseUtil.TryConvertToString(sdr["CinemaName"]);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
            }
            return CinemaName;
        }


        #endregion

        #region ESEventLog_List

        public List<ESEventLogInfo> GetESEventLog_List(int seq)
        {
            List<ESEventLogInfo> list = new List<ESEventLogInfo>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Int, "Seq", seq);

                using (var sdr = this.mssql.StoredProcedure("VINYL_ES_EventLog_List", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            ESEventLogInfo item = new ESEventLogInfo();
                            item.UserID = DatabaseUtil.TryConvertToString(sdr["UserID"]);
                            item.WorkName = DatabaseUtil.TryConvertToString(sdr["WorkName"]);
                            item.Location = (EventLogLocationType)DatabaseUtil.TryConvertToInteger(sdr["Location"]);
                            item.LocationName = item.Location.ToString();
                            item.RegDateTime = DatabaseUtil.TryConvertToDateTime(sdr["RegDateTime"]);
                            item.RegDateTimeToText = item.RegDateTime.ToShortTimeString();
                            list.Add(item);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
            }
            return list;
        }


        #endregion

        #region GetSiteInfo

        public List<SiteInfo> GetSiteInfo(string startChar)
        {
            List<SiteInfo> siteList = new List<SiteInfo>();
            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "startChar", startChar, 2);

                using (var sdr = this.mssql.StoredProcedure("DID_SiteInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            SiteInfo siteItem = new SiteInfo
                            {
                                CinemaCode = sdr["Kobis"] as string,
                                CinemaName = sdr["CinemaName"] as string
                            };
                            siteList.Add(siteItem);
                        }
                        sdr.Close();
                    }
                    DisConnection();
                }
            }
            return siteList;
        }

        #endregion

        #region GetContentsInfo
        public List<ContentsInfoRaw> GetContentsInfo(string groupid)
        {
            List<ContentsInfoRaw> list = new List<ContentsInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "GroupID", groupid, 20);
                using (var reader = this.mssql.StoredProcedure("ContentsInfoUsingID_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ContentsInfoRaw item = new ContentsInfoRaw
                            {
                                ContentsName = DatabaseUtil.TryConvertToString(reader["ContentsName"]),
                                ContentsType = DatabaseUtil.TryConvertToString(reader["ContentsType"]),
                                FileName = DatabaseUtil.TryConvertToString(reader["FileName"]),
                                FileSize = DatabaseUtil.TryConvertToLong(reader["FileSize"]),
                                FileType = DatabaseUtil.TryConvertToString(reader["FileType"]),
                                GroupID = DatabaseUtil.TryConvertToString(reader["GroupID"]),
                                ContentsID = DatabaseUtil.TryConvertToString(reader["ContentsID"]),
                                ItemPositionID = DatabaseUtil.TryConvertToString(reader["ItemPositionID"]),
                                UseYN = 'Y'
                            };
                            list.Add(item);
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }

            return list;
        }
        #endregion

        #region GetPopupNoticeInfoItem
        public PopupNoticeInfoRaw GetPopupNoticeInfoItem(string noticeID)
        {
            PopupNoticeInfoRaw item = new PopupNoticeInfoRaw();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "NoticeID", noticeID, 17);
                using (var reader = this.mssql.StoredProcedure("DID_PopupNoticeInfoItem_SELECT", parameterValues))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            item = new PopupNoticeInfoRaw
                            {
                                NoticeID = DatabaseUtil.TryConvertToString(reader["NoticeID"]),
                                SkinID = DatabaseUtil.TryConvertToString(reader["SkinID"]),
                                FontFamily = DatabaseUtil.TryConvertToString(reader["FontFamily"]),
                                Header = DatabaseUtil.TryConvertToString(reader["Header"]),
                                Title = DatabaseUtil.TryConvertToString(reader["Title"]),
                                Body = DatabaseUtil.TryConvertToString(reader["Body"]),
                                HeaderCharacterColor = DatabaseUtil.TryConvertToString(reader["HeaderCharacterColor"]),
                                HeaderCharacterBold = DatabaseUtil.TryConvertToString(reader["HeaderCharacterBold"]),
                                TitleCharacterColor = DatabaseUtil.TryConvertToString(reader["TitleCharacterColor"]),
                                TitleCharacterBold = DatabaseUtil.TryConvertToString(reader["TitleCharacterBold"]),
                                BodyCharacterColor = DatabaseUtil.TryConvertToString(reader["BodyCharacterColor"]),
                                BodyCharacterBold = DatabaseUtil.TryConvertToString(reader["BodyCharacterBold"])
                            };
                        }

                        reader.Close();
                        reader.Dispose();
                    }
                }
                DisConnection();
            }
            return item;
        }
        #endregion

        #region GetCurtainContentsTime
        public List<string> GetCurtainContentsTime()
        {
            List<string> time = new List<string>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();

                using (var sdr = this.mssql.StoredProcedure("DID_DigitalCurtainDATE_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            time.Add(DatabaseUtil.TryConvertToString(sdr[0]));
                            time.Add(DatabaseUtil.TryConvertToString(sdr[1]));
                        }

                        sdr.Close();
                    }
                }

                DisConnection();
            }

            return time;
        }
        #endregion

        #region GetUserInfo
        public UserInfoRaw GetUserInfo(string id, string pw)
        {
            UserInfoRaw userinfo = new UserInfoRaw();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "UserID", id, 20);
                parameterValues.Add(SqlDbType.Char, "Password", pw, 20);

                using (var sdr = this.mssql.StoredProcedure("UserInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            userinfo.UserID = DatabaseUtil.TryConvertToString(sdr["UserID"]);
                            userinfo.CinemaCode = DatabaseUtil.TryConvertToInteger(sdr["CinemaCode"]);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
            }
            return userinfo;
        }
        #endregion

        #region GetMovieContentsInfoList

        public List<MovieContentsInfoRaw> GetMovieContentsInfoList()
        {
            List<MovieContentsInfoRaw> list = new List<MovieContentsInfoRaw>();

            if (ConnectionDID())
            {
                using (var sdr = this.mssql.StoredProcedure("DID_MovieContentsInfo_SELECT", null))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            MovieContentsInfoRaw item = new MovieContentsInfoRaw
                            {
                                Seq = DatabaseUtil.TryConvertToInteger(sdr["Seq"]),
                                ContentsCode = DatabaseUtil.TryConvertToString(sdr["ContentsCode"]),
                                MovieShortName = DatabaseUtil.TryConvertToString(sdr["MovieShortName"]),
                                LargePosterFileName = DatabaseUtil.TryConvertToString(sdr["LargePosterFileName"]),
                                SmallPosterFileName = DatabaseUtil.TryConvertToString(sdr["SmallPosterFileName"]),
                                MovieFileName = DatabaseUtil.TryConvertToString(sdr["MovieFileName"]),
                                RegDate = DatabaseUtil.TryConvertToDateTime(sdr["RegDate"]),
                                RegID = DatabaseUtil.TryConvertToString(sdr["RegID"])
                            };

                            list.Add(item);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
            }

            return list;
        }

        #endregion

        #region GetMovieContentsUploadAvailableInfoList

        public List<MovieContentsUploadAvailableInfo> GetMovieContentsUploadAvailableInfoList(DateTime startDate, DateTime endDate)
        {
            var list = new List<MovieContentsUploadAvailableInfo>();

            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetMovieContentsUploadAvailableInfoList(startDate, endDate), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            if (datasets.Tables.Count > 0)
            {
                var items = datasets.Tables[0].DataTableToList<MappedMovieContentsUploadAvailableInfo>();

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var data = new MovieContentsUploadAvailableInfo
                        {
                            TitleKor = item.TitleKor,
                            MovieInfoCode = item.MovieInfoCode,
                            OpenDate = item.OpenDate
                        };
                        list.Add(data);
                    }
                }
            }

            ////////////////////////////////////////////////////////////////////
            // !!리스트 개수 만큼 컨텐츠 코드 조회함
            // !!속도가 너무 느리면 MovieContentsInfoList 한번에 받아와서 비교문으로 변경 해야함
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];

                if (string.IsNullOrEmpty(item.MovieInfoCode))
                {
                    list.Remove(item);
                }
                else
                {
                    var movieContentsInfoItems = GetLCSMMovieContentsInfoList(item.MovieInfoCode);
                    if (movieContentsInfoItems.Count > 0)
                    {
                        item.IsPoster1 = !(string.IsNullOrEmpty(movieContentsInfoItems.FirstOrDefault().LargePosterFileName));
                        item.IsPoster2 = !(string.IsNullOrEmpty(movieContentsInfoItems.FirstOrDefault().SmallPosterFileName));
                        item.IsVideo = !(string.IsNullOrEmpty(movieContentsInfoItems.FirstOrDefault().MovieFileName));
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////
            
            
            return list;
        }

        #endregion

        #region GetMovieContentsUploadAvailableInfoList

        /// <summary>
        /// 인브레인측에 새로 서비스 요청해야함(이름검색)
        /// </summary>
        /// <param name="searchTitle"></param>
        /// <returns></returns>
        public List<MovieContentsUploadAvailableInfo> GetMovieContentsUploadAvailableInfoList(DateTime startDate, DateTime endDate,string searchTitle)
        {
            var list = new List<MovieContentsUploadAvailableInfo>();

            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetMovieContentsUploadAvailableInfoList(startDate,endDate,searchTitle), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            if (datasets.Tables.Count > 0)
            {
                var items = datasets.Tables[0].DataTableToList<MappedMovieContentsUploadAvailableInfo>();

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var data = new MovieContentsUploadAvailableInfo
                        {
                            TitleKor = item.TitleKor,
                            MovieInfoCode = item.MovieInfoCode,
                            OpenDate = item.OpenDate
                        };

                        list.Add(data);
                    }
                }
            }

            ////////////////////////////////////////////////////////////////////
            // !!리스트 개수 만큼 컨텐츠 코드 조회함
            // !!속도가 너무 느리면 MovieContentsInfoList 한번에 받아와서 비교문으로 변경 해야함
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];

                if (string.IsNullOrEmpty(item.MovieInfoCode))
                {
                    list.Remove(item);
                }
                else
                {
                    var movieContentsInfoItems = GetLCSMMovieContentsInfoList(item.MovieInfoCode);
                    if (movieContentsInfoItems.Count > 0)
                    {
                        item.IsPoster1 = !(string.IsNullOrEmpty(movieContentsInfoItems.FirstOrDefault().LargePosterFileName));
                        item.IsPoster2 = !(string.IsNullOrEmpty(movieContentsInfoItems.FirstOrDefault().SmallPosterFileName));
                        item.IsVideo = !(string.IsNullOrEmpty(movieContentsInfoItems.FirstOrDefault().MovieFileName));
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////


            return list;
        }

        #endregion


        #region GetADStatusInfo

        public List<AdStatusInfo> GetADStatusInfo(string adcode, string advertiser, string startdate, string enddate, int statusCode, string theater, string itemcode)
        {
            try
            {
                List<AdStatusInfo> list = new List<AdStatusInfo>();

                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ADCode", adcode, 20);
                    parameterValues.Add(SqlDbType.VarChar, "Advertiser", advertiser, 10);
                    parameterValues.Add(SqlDbType.DateTime, "StartDate", startdate);
                    parameterValues.Add(SqlDbType.DateTime, "EndDate", enddate);
                    parameterValues.Add(SqlDbType.Int, "StatusCode", statusCode);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "ItemID", itemcode, 10);

                    using (var sdr = this.mssql.StoredProcedure("DID_AdStatusInfo_SELECT", parameterValues))
                    {

                        if (sdr != null)
                        {
                            while (sdr.Read())
                            {
                                AdStatusInfo item = new AdStatusInfo
                                {
                                    ADCode = DatabaseUtil.TryConvertToString(sdr["ADCode"]),
                                    Advertiser = DatabaseUtil.TryConvertToString(sdr["Advertiser"]),
                                    AdvertiserID = DatabaseUtil.TryConvertToString(sdr["AdvertiserID"]),
                                    StartDate = DatabaseUtil.TryConvertToDateTime(sdr["StartDate"]),
                                    EndDate = DatabaseUtil.TryConvertToDateTime(sdr["EndDate"]),
                                    Title = DatabaseUtil.TryConvertToString(sdr["Title"]),
                                    Theater = DatabaseUtil.TryConvertToString(sdr["Theater"]),
                                    TheaterName = DatabaseUtil.TryConvertToString(sdr["TheaterName"]),
                                    ADType = (ContentsType)DatabaseUtil.TryConvertToInteger(sdr["ADType"])
                                };

                                list.Add(item);
                            }

                            sdr.Close();
                        }

                        DisConnection();
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        #region GetADStatusInfo

        public List<AdStatusInfo> GetADStatusInfo(string adcode, string advertiser, int statusCode)
        {
            List<AdStatusInfo> list = new List<AdStatusInfo>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "ADCode", adcode, 20);
                parameterValues.Add(SqlDbType.VarChar, "Advertiser", advertiser, 10);
                parameterValues.Add(SqlDbType.Int, "StatusCode", statusCode);


                using (var sdr = this.mssql.StoredProcedure("DID_AdStatusInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            AdStatusInfo item = new AdStatusInfo
                            {
                                ADCode = DatabaseUtil.TryConvertToString(sdr["ADCode"]),
                                Advertiser = DatabaseUtil.TryConvertToString(sdr["Advertiser"]),
                                AdvertiserID = DatabaseUtil.TryConvertToString(sdr["AdvertiserID"]),
                                StartDate = DatabaseUtil.TryConvertToDateTime(sdr["StartDate"]),
                                EndDate = DatabaseUtil.TryConvertToDateTime(sdr["EndDate"]),
                                Title = DatabaseUtil.TryConvertToString(sdr["Title"]),
                            };

                            list.Add(item);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
            }

            return list;
        }

        #endregion

        #region GetESEventInfomationList
        public List<ESEventInfomationRawInfo> GetESEventInfomationList(string cinemaCode01, string cinemaCode02, string beginDate, string endDate)
        {
            List<ESEventInfomationRawInfo> eventlist = new List<ESEventInfomationRawInfo>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "CinemaCode1", cinemaCode01, 4);

                parameterValues.Add(SqlDbType.VarChar, "CinemaCode2", cinemaCode02, 4);

                parameterValues.Add(SqlDbType.DateTime, "BeginDate", DateTime.ParseExact(beginDate, "yyyy-MM-dd", CultureInfo.CurrentCulture));
                parameterValues.Add(SqlDbType.DateTime, "EndDate", DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.CurrentCulture).AddHours(28));
                try
                {
                    using (var sdr = this.mssql.StoredProcedure("VINYL_ES_EventInformation_TEST", parameterValues))
                    {
                        if (sdr != null)
                        {
                            while (sdr.Read())
                            {
                                eventlist.Add(new ESEventInfomationRawInfo()
                                 {
                                     BgmIndex = DatabaseUtil.TryConvertToInteger(sdr["BgmIndex"], 5),
                                     CinemaCode = DatabaseUtil.TryConvertToString(sdr["CinemaCode"]),
                                     EventDeleteTimer = DatabaseUtil.TryConvertToDateTime(sdr["EventDeleteTimer"]),
                                     EventEndTime = DatabaseUtil.TryConvertToDateTime(sdr["EventEndTime"]),
                                     EventEnterTime = DatabaseUtil.TryConvertToDateTime(sdr["EventEnterTime"]),
                                     EventStartTime = DatabaseUtil.TryConvertToDateTime(sdr["EventStartTime"]),
                                     FTPFilePath = DatabaseUtil.TryConvertToString(sdr["FTPFilePath"]),
                                     isComplete = DatabaseUtil.TryConvertCharToBool(sdr["isComplete"]),
                                     isDisplayed = DatabaseUtil.TryConvertCharToBool(sdr["isDisplayed"]),
                                     isOpen = DatabaseUtil.TryConvertCharToBool(sdr["isOpen"]),
                                     Seq = DatabaseUtil.TryConvertToInteger(sdr["Seq"]),
                                     TransNo = DatabaseUtil.TryConvertToString(sdr["TransNo"]),
                                     UserEmail = DatabaseUtil.TryConvertToString(sdr["UserEmail"]),
                                     UserID = DatabaseUtil.TryConvertToString(sdr["UserID"]),
                                     UserPhone = DatabaseUtil.TryConvertToString(sdr["UserPhone"])
                                 });
                            }

                            sdr.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                DisConnection();
            }
            return eventlist;
        }
        #endregion

        #region GetAdTimeInfo

        public AdTime GetAdTimeInfo(string theater)
        {
            AdTime adTime = new AdTime();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);

                using (var sdr = this.mssql.StoredProcedure("DID_AdTimeInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            adTime.StartTime = DatabaseUtil.TryConvertToString(sdr["StartTime"]);
                            adTime.EndTime = DatabaseUtil.TryConvertToString(sdr["EndTime"]);
                            adTime.SecondInterval = DatabaseUtil.TryConvertToInteger(sdr["Interval"]);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
            }

            return adTime;
        }

        #endregion

        #endregion

        #region Insert Method

        #region InsertCheckMovieTimeInfo
        public bool InsertCheckMovieTimeInfo(MovieTimeCellInfo movietimecell)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "CinemaCode", movietimecell.CinemaCode, 10);
                    parameterValues.Add(SqlDbType.VarChar, "MovieCode", movietimecell.ContentsCode, 6);
                    parameterValues.Add(SqlDbType.VarChar, "MovieKoreaName", movietimecell.MovieKoreaName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "ScreenCode", movietimecell.ScreenCode, 2);
                    parameterValues.Add(SqlDbType.Char, "StartTime", movietimecell.StartTime, 5);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", movietimecell.RegID);

                    using (var sdr = this.mssql.StoredProcedure("DID_CheckMovieTimeInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertLimitedADInfo
        public bool InsertLimitedADInfo(AdInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", InfoRaw.ID, 20);
                    if (InfoRaw.ContentsType == ContentsType.Adver)
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", InfoRaw.ContractID);
                    }
                    else
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", DBNull.Value);
                    }
                    parameterValues.Add(SqlDbType.Char, "ItemID", InfoRaw.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Title", InfoRaw.Title, 100);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", InfoRaw.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "ContentsType", ((int)InfoRaw.ContentsType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "LayoutType", ((int)InfoRaw.LayoutType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "SoundPosition", InfoRaw.SoundPosition.ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", InfoRaw.BeginDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", InfoRaw.EndDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "Account", InfoRaw.Account, 2);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", InfoRaw.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_LimitedAdInfo_INSERT", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertTransparentAdverInfo
        public bool InsertTransparentAdverInfo(AdInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", InfoRaw.ID, 20);
                    parameterValues.Add(SqlDbType.Int, "ContractID", InfoRaw.ContractID);
                    parameterValues.Add(SqlDbType.VarChar, "Title", InfoRaw.Title, 100);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", InfoRaw.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", InfoRaw.BeginDate.ToString("yyyy-MM-dd"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", InfoRaw.EndDate.ToString("yyyy-MM-dd"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "Account", InfoRaw.Account, 2);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", InfoRaw.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_TransparentAdInfo_INSERT", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertOriginFileInfo
        public bool InsertOriginFileInfo(ContentsInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ItemID", InfoRaw.ItemID, 3);
                    parameterValues.Add(SqlDbType.Char, "ItemPositionID", InfoRaw.ItemPositionID,2);
                    parameterValues.Add(SqlDbType.VarChar, "ContentsName", InfoRaw.ContentsName, 100);
                    parameterValues.Add(SqlDbType.Char, "ADID", InfoRaw.GroupID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "OriginFileName", InfoRaw.OriginFileName, 500);

                    using (var sdr = this.mssql.StoredProcedure("DID_OriginFileInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertContentsInfo
        public bool InsertContentsInfo(ContentsInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ContentsName", InfoRaw.ContentsName, 100);
                    parameterValues.Add(SqlDbType.Int, "Category", InfoRaw.Category);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", InfoRaw.BeginDate, 10);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", InfoRaw.EndDate, 10);
                    parameterValues.Add(SqlDbType.VarChar, "FileName", InfoRaw.FileName, 100);
                    parameterValues.Add(SqlDbType.Int, "UserGroupID", InfoRaw.UserGroupID);
                    if (!(InfoRaw.ContractID == 0))
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", InfoRaw.ContractID);
                    }
                    else
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", DBNull.Value);
                    }
                    parameterValues.Add(SqlDbType.VarChar, "FileType", InfoRaw.FileType, 5);
                    parameterValues.Add(SqlDbType.BigInt, "FileSize", InfoRaw.FileSize);
                    parameterValues.Add(SqlDbType.VarChar, "GroupID", InfoRaw.GroupID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "ItemID", InfoRaw.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "ItemPositionID", int.Parse(InfoRaw.ItemPositionID).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "ContentsType", int.Parse(InfoRaw.ContentsType).ToString("00"), 2);

                    using (var sdr = this.mssql.StoredProcedure("DID_ContentsInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertMediaInfo
        public bool InsertMediaInfo(MediaInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", InfoRaw.ID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", InfoRaw.Theater, 10);
                    parameterValues.Add(SqlDbType.Char, "ItemID", InfoRaw.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Title", InfoRaw.Title, 100);
                    parameterValues.Add(SqlDbType.Char, "LayoutType", (int.Parse(InfoRaw.LayoutType)).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.Char, "SoundPosition", InfoRaw.SoundPosition.ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", InfoRaw.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_Media_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertADInfo
        public bool InsertADInfo(AdInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", InfoRaw.ID, 20);
                    if (!string.IsNullOrEmpty(InfoRaw.ContractID))
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", InfoRaw.ContractID);
                    }
                    else
                    {
                        parameterValues.Add(SqlDbType.Int, "ContractID", DBNull.Value);
                    }
                    parameterValues.Add(SqlDbType.Char, "ItemID", InfoRaw.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Title", InfoRaw.Title, 100);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", InfoRaw.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "ContentsType", ((int)InfoRaw.ContentsType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "LayoutType", ((int)InfoRaw.LayoutType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "SoundPosition", InfoRaw.SoundPosition.ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", InfoRaw.BeginDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", InfoRaw.EndDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", InfoRaw.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_AdInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public bool InsertMovieContentsAvailable(MovieContentsInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ContentsCode", InfoRaw.ContentsCode, 6);
                    parameterValues.Add(SqlDbType.VarChar, "MovieShortName", InfoRaw.MovieShortName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "LargePosterFileName", InfoRaw.LargePosterFileName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "SmallPosterFileName", InfoRaw.SmallPosterFileName, 20);
                    parameterValues.Add(SqlDbType.VarChar, "MovieFileName", InfoRaw.MovieFileName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", InfoRaw.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_MovieContentsAvailable_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region InsertSkinMapInfo
        public bool InsertSkinMapInfo(SkinMapInfoProcedure InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "SkinID", InfoRaw.SkinID, 20);
                    parameterValues.Add(SqlDbType.Char, "GroupID", InfoRaw.GroupID, 20);
                    parameterValues.Add(SqlDbType.Char, "ItemID", InfoRaw.SkinID, 3);
                    parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)InfoRaw.ContentsType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", InfoRaw.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_SkinMapInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertSkinInfo
        public bool InsertSkinInfo(SkinInfoRaw skin)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", skin.ID, 20);
                    parameterValues.Add(SqlDbType.Char, "ItemID", skin.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Title", skin.Title, 100);
                    parameterValues.Add(SqlDbType.Char, "ContentsType", skin.ContentsType, 2);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", skin.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_SkinInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion 

        #region InsertEventInfo
        public bool InsertEventInfo(EventInfoRaw eventItem)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "EventID", eventItem.EventID, 20);
                    parameterValues.Add(SqlDbType.Char, "ItemID", eventItem.ItemID, 3);
                    parameterValues.Add(SqlDbType.Char, "Theater", eventItem.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "Title", eventItem.Title, 100);
                    parameterValues.Add(SqlDbType.Char, "LayoutType", eventItem.LayoutType.ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", eventItem.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_EventInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertNoticeInfo
        public bool InsertNoticeInfo(NoticeInfoRaw notice)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", notice.NoticeID, 17);
                    parameterValues.Add(SqlDbType.Char, "ItemID", notice.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", notice.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "Text", notice.Text, 300);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", notice.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_NoticeInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region InsertDigitalSignInfo
        public bool InsertDigitalSignInfo(DigitalSignInfoRaw DSItem)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "NoticeID", DSItem.NoticeID, 17);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", DSItem.Theater, 10);
                    parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)DSItem.ContentsType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "Text", DSItem.Text, 300);
                    parameterValues.Add(SqlDbType.VarChar, "FontFamily", DSItem.FontFamily, 100);
                    parameterValues.Add(SqlDbType.VarChar, "CharacterColor", DSItem.CharacterColor, 500);
                    parameterValues.Add(SqlDbType.VarChar, "CharacterBold", DSItem.CharacterBold, 300);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", DSItem.RegID, 100);
                    if (DSItem.IconFileName == null)
                        parameterValues.Add(SqlDbType.VarChar, "IconFileName", DBNull.Value);
                    else
                        parameterValues.Add(SqlDbType.VarChar, "IconFileName", DSItem.IconFileName, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_DigitalSignInfo_INSERT", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertDigitalSignInfo
        public bool InsertPopupNoticeInfo(PopupNoticeInfoRaw item)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", item.NoticeID, 17);
                    parameterValues.Add(SqlDbType.Char, "SkinID", item.SkinID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", item.Theater, 10);
                    parameterValues.Add(SqlDbType.Char, "ItemID", item.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "FontFamily", item.FontFamily, 100);
                    parameterValues.Add(SqlDbType.VarChar, "Header", item.Header, 200);
                    parameterValues.Add(SqlDbType.VarChar, "Title", item.Title, 200);
                    parameterValues.Add(SqlDbType.VarChar, "Body", item.Body, 500);
                    parameterValues.Add(SqlDbType.VarChar, "HeaderCharacterColor", item.HeaderCharacterColor, 500);
                    parameterValues.Add(SqlDbType.VarChar, "HeaderCharacterBold", item.HeaderCharacterBold, 300);
                    parameterValues.Add(SqlDbType.VarChar, "TitleCharacterColor", item.TitleCharacterColor, 500);
                    parameterValues.Add(SqlDbType.VarChar, "TitleCharacterBold", item.TitleCharacterBold, 300);
                    parameterValues.Add(SqlDbType.VarChar, "BodyCharacterColor", item.BodyCharacterColor, 500);
                    parameterValues.Add(SqlDbType.VarChar, "BodyCharacterBold", item.BodyCharacterBold, 300);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", item.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_PopupNoticeInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region SetScheduleInfo
        public bool SetScheduleInfo(ScheduleInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ID", InfoRaw.ID, 20);
                    parameterValues.Add(SqlDbType.Char, "ContentsType", (int.Parse(InfoRaw.ContentsType)).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "ItemID", InfoRaw.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", InfoRaw.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", InfoRaw.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_ScheduleInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region SetMovieShowing
        public bool SetMovieShowing(PlannedMovieInfoRaw movie)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Int, "OrderNo", movie.OrderNo);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", movie.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "ContentsCode", movie.ContentsCode, 6);
                    parameterValues.Add(SqlDbType.VarChar, "Title", movie.Title, 100);
                    parameterValues.Add(SqlDbType.Char, "ItemID", movie.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", movie.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_MovieShowingInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region SetPlannedMovieInfoList
        public bool SetPlannedMovieInfoList(PlannedMovieInfoRaw movie)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Int, "OrderNo", movie.OrderNo);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", movie.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "MovieCode", movie.MovieCode, 6);
                    parameterValues.Add(SqlDbType.VarChar, "Title", movie.Title, 100);
                    parameterValues.Add(SqlDbType.Char, "ItemID", movie.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", movie.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_PlannedMovieInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region SetRecommandMovieInfoList
        public bool SetRecommandMovieInfoList(RecommandedMovieInfoRaw movie)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Int, "OrderNo", movie.OrderNo);
                    parameterValues.Add(SqlDbType.VarChar, "Theater", movie.Theater, 10);
                    parameterValues.Add(SqlDbType.VarChar, "MovieCode", movie.MovieCode, 6);
                    parameterValues.Add(SqlDbType.VarChar, "Title", movie.Title, 100);
                    parameterValues.Add(SqlDbType.Char, "ItemID", movie.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", movie.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_RecommandedMovieInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region SetItemStatusInfo
        public void SetItemStatusInfo(ItemStatusInfo statusInfo)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Int, "CinemaCode", statusInfo.CinemaCode);
                    parameterValues.Add(SqlDbType.Int, "ScreenCode", (int)statusInfo.ScreenCode);
                    parameterValues.Add(SqlDbType.VarChar, "IP", statusInfo.IP, 20);
                    parameterValues.Add(SqlDbType.BigInt, "DiskUsed", 0);
                    parameterValues.Add(SqlDbType.BigInt, "DiskTotal", 0);
                    parameterValues.Add(SqlDbType.DateTime, "BeginTime", statusInfo.BeginTime);
                    parameterValues.Add(SqlDbType.Int, "ContentsID", statusInfo.ContentsID);
                    parameterValues.Add(SqlDbType.Int, "PlayNO", 0);
                    parameterValues.Add(SqlDbType.Int, "Duration", 0);

                    using (var sdr = this.mssql.StoredProcedure("LC_DSI.dbo.ScreenStatus_Update2", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region SetAdLogInfo
        public bool SetAdLogInfo(AdLogInfo logInfo)
        {
            bool result = false;

            try
            {
                if (ConnectionDID())
                {
                    var duration = (int)Math.Round((logInfo.EndTime - logInfo.BeginTime).TotalSeconds);

                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.NVarChar, "CinemaCode", logInfo.CinemaCode, 4);
                    parameterValues.Add(SqlDbType.NVarChar, "ScreenCode", logInfo.ScreenCode, 5);
                    parameterValues.Add(SqlDbType.NVarChar, "ScreenNo", "1", 2);
                    parameterValues.Add(SqlDbType.NVarChar, "BeginTime", logInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"), 19);
                    parameterValues.Add(SqlDbType.NVarChar, "ContentsID", logInfo.ContentsID, 10);
                    parameterValues.Add(SqlDbType.NVarChar, "SectionID", "DID", 10);
                    parameterValues.Add(SqlDbType.NVarChar, "PlayNo", "1", 10);
                    parameterValues.Add(SqlDbType.NVarChar, "EndTime", logInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"), 19);
                    parameterValues.Add(SqlDbType.NVarChar, "Duration", duration.ToString(), 10);
                    parameterValues.Add(SqlDbType.NVarChar, "AdID", logInfo.AdID, 20);

                    using (var sdr = this.mssql.StoredProcedure("LC_DSI.dbo.GeneralResult_Insert", parameterValues))
                    {
                        if (sdr != null)
                        {
                            var errorCode = DatabaseUtil.TryConvertToInteger(sdr["ERROR_CODE"]);

                            result = errorCode == 0;

                            sdr.Close();
                        }
                    }

                    DisConnection();
                }

                return result;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region InsertMovieContentsInfo
        public bool InsertMovieContentsInfo(MovieContentsInfoRaw info)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ContentsCode", info.ContentsCode, 6);
                    parameterValues.Add(SqlDbType.VarChar, "MovieShortName", info.MovieShortName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "LargePosterFileName", info.LargePosterFileName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "SmallPosterFileName", info.SmallPosterFileName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "MovieFileName", info.MovieFileName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", info.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_MovieContentsInfo_INSERT", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion 

        #endregion     

        #region Update Method

        #region UpdateESEventInformation

        public bool UpdateESEventInformation(int seq, DateTime startTime, bool isDisplayedNull, char isDisplayed, bool isOpenNull, char isOpen)
        {
            try
            {
                if (ConnectionDID())
                {

                    var parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Int, "Seq", seq);

                    if (startTime == DateTime.MinValue)
                    {
                        parameterValues.Add(SqlDbType.DateTime, "EventStartTime", DBNull.Value);
                    }
                    else
                    {
                        parameterValues.Add(SqlDbType.DateTime, "EventStartTime", startTime);
                    }

                    if (isDisplayedNull)
                    {
                        parameterValues.Add(SqlDbType.Char, "IsDisplayed", DBNull.Value);
                    }
                    else
                    {
                        parameterValues.Add(SqlDbType.Char, "IsDisplayed", isDisplayed);
                    }

                    if (isOpenNull)
                    {
                        parameterValues.Add(SqlDbType.Char, "IsOpen", DBNull.Value);
                    }
                    else
                    {
                        parameterValues.Add(SqlDbType.Char, "IsOpen", isOpen);
                    }

                    using (var sdr = this.mssql.StoredProcedure("VINYL_ES_EventInformation_Test_Update", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region UpdateContentsInfo
        public bool UpdateContentsInfo(ContentsInfoRaw InfoRaw)
        {
            var result = false;
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ContentsName", InfoRaw.ContentsName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "FileType", InfoRaw.FileType, 5);
                    parameterValues.Add(SqlDbType.BigInt, "FileSize", InfoRaw.FileSize);
                    parameterValues.Add(SqlDbType.VarChar, "GroupID", InfoRaw.GroupID, 20);
                    parameterValues.Add(SqlDbType.Char, "ContentsType", InfoRaw.ContentsType, 2);
                    parameterValues.Add(SqlDbType.VarChar, "ItemPositionID", int.Parse(InfoRaw.ItemPositionID).ToString("00"), 2);
                    using (var sdr = this.mssql.StoredProcedure("DID_ContentsInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                        else
                        {
                            result = true;
                        }
                    }

                    DisConnection();
                }
                return result;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateCheckMovieTime
        public bool UpdateCheckMovieTime(bool checkStatus, int id)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "UseYN", checkStatus ? '1' : '0');
                    parameterValues.Add(SqlDbType.Int, "Seq", id);
                    using (var sdr = this.mssql.StoredProcedure("DID_CheckMovieTimeUseYN_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateContentsInfo
        public bool UpdateEventContentsInfo(ContentsInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "ContentsName", InfoRaw.ContentsName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "FileName", InfoRaw.FileName, 100);
                    parameterValues.Add(SqlDbType.VarChar, "FileType", InfoRaw.FileType, 5);
                    parameterValues.Add(SqlDbType.BigInt, "FileSize", InfoRaw.FileSize);
                    parameterValues.Add(SqlDbType.VarChar, "GroupID", InfoRaw.GroupID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "ItemPositionID", int.Parse(InfoRaw.ItemPositionID).ToString("00"), 2);
                    using (var sdr = this.mssql.StoredProcedure("DID_ContentsInfoEvent_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdatgeSkinMapInfo
        public bool UpdatgeSkinMapInfo(SkinMapInfoProcedure InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "SkinID", InfoRaw.SkinID, 20);
                    parameterValues.Add(SqlDbType.Char, "GroupID", InfoRaw.GroupID, 20);
                    parameterValues.Add(SqlDbType.Char, "ItemID", InfoRaw.SkinID, 3);
                    parameterValues.Add(SqlDbType.Char, "ContentsType", ((int)InfoRaw.ContentsType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", InfoRaw.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_SkinMapInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateNoticeInfo
        public bool UpdateNoticeInfo(NoticeInfoRaw notice)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", notice.NoticeID, 17);
                    parameterValues.Add(SqlDbType.VarChar, "Text", notice.Text, 300);
                    parameterValues.Add(SqlDbType.VarChar, "UpdateRegID", notice.UpdateRegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_NoticeInfo_Update", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateSkinInfo
        public bool UpdateSkinInfo(SkinInfoRaw skin)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", skin.ID, 20);
                    parameterValues.Add(SqlDbType.Char, "ItemID", skin.ItemID, 3);
                    parameterValues.Add(SqlDbType.VarChar, "Title", skin.Title, 100);
                    parameterValues.Add(SqlDbType.Char, "ContentsType", skin.ContentsType, 2);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", skin.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_SkinInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateDigitalSignInfo
        public bool UpdateDigitalSignInfo(DigitalSignInfoRaw DSItem)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "NoticeID", DSItem.NoticeID, 17);
                    parameterValues.Add(SqlDbType.VarChar, "Text", DSItem.Text, 300);
                    parameterValues.Add(SqlDbType.VarChar, "FontFamily", DSItem.FontFamily, 100);
                    parameterValues.Add(SqlDbType.VarChar, "CharacterColor", DSItem.CharacterColor, 500);
                    parameterValues.Add(SqlDbType.VarChar, "CharacterBold", DSItem.CharacterBold, 300);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", DSItem.RegID, 100);
                    if (DSItem.IconFileName == null)
                        parameterValues.Add(SqlDbType.VarChar, "IconFileName", DBNull.Value);
                    else
                        parameterValues.Add(SqlDbType.VarChar, "IconFileName", DSItem.IconFileName, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_DigitalSignInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdatePopupNoticeInfo
        public bool UpdatePopupNoticeInfo(PopupNoticeInfoRaw item)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", item.NoticeID, 17);
                    parameterValues.Add(SqlDbType.Char, "SkinID", item.SkinID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "FontFamily", item.FontFamily, 100);
                    parameterValues.Add(SqlDbType.VarChar, "Header", item.Header, 200);
                    parameterValues.Add(SqlDbType.VarChar, "Title", item.Title, 200);
                    parameterValues.Add(SqlDbType.VarChar, "Body", item.Body, 500);
                    parameterValues.Add(SqlDbType.VarChar, "HeaderCharacterColor", item.HeaderCharacterColor, 500);
                    parameterValues.Add(SqlDbType.VarChar, "HeaderCharacterBold", item.HeaderCharacterBold, 300);
                    parameterValues.Add(SqlDbType.VarChar, "TitleCharacterColor", item.TitleCharacterColor, 500);
                    parameterValues.Add(SqlDbType.VarChar, "TitleCharacterBold", item.TitleCharacterBold, 300);
                    parameterValues.Add(SqlDbType.VarChar, "BodyCharacterColor", item.BodyCharacterColor, 500);
                    parameterValues.Add(SqlDbType.VarChar, "BodyCharacterBold", item.BodyCharacterBold, 300);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", item.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_PopupNoticeInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateNoticeUSEYN
        public bool UpdateNoticeUSEYN(string noticeID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", noticeID, 17);
                    using (var sdr = this.mssql.StoredProcedure("DID_NoticeUseYN_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdatePopupNoticeUSEYN
        public bool UpdatePopupNoticeUSEYN(string noticeID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", noticeID, 17);
                    using (var sdr = this.mssql.StoredProcedure("DID_PopupNoticeUseYN_UPDATE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateDSNoticeUSEYN
        public bool UpdateDSNoticeUSEYN(string noticeID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", noticeID, 17);
                    using (var sdr = this.mssql.StoredProcedure("DID_DSNoticeUseYN_UPDATE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateDigitalSignSkin
        public bool UpdateDigitalSignSkin(string SkinID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "SkinID", SkinID, 20);
                    using (var sdr = this.mssql.StoredProcedure("DID_DigitalSignSkin_UPDATE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateEventInfo
        public bool UpdateEventInfo(EventInfoRaw eventItem)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "EventID", eventItem.EventID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "Title", eventItem.Title, 100);
                    parameterValues.Add(SqlDbType.Char, "LayoutType", eventItem.LayoutType.ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", eventItem.RegID, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_EventInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateMediaInfo
        public bool UpdateMediaInfo(MediaInfoRaw InfoRaw)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", InfoRaw.ID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "Title", InfoRaw.Title, 100);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", InfoRaw.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_Media_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateDB_PopupNoticeUSEYN
        public bool UpdateDB_PopupNoticeUSEYN(string noticeID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", noticeID, 17);
                    using (var sdr = this.mssql.StoredProcedure("DID_PopupNoticeUseYN_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateADInfo
        public bool UpdateAdverInfo(AdInfoRaw ad)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", ad.ID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "Title", ad.Title, 100);
                    parameterValues.Add(SqlDbType.VarChar, "LayoutType", ((int)ad.LayoutType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "SoundPosition", ad.SoundPosition.ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", ad.BeginDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", ad.EndDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", ad.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_AdInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateLimitedAdverInfo
        public bool UpdateLimitedAdverInfo(AdInfoRaw ad)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", ad.ID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "Title", ad.Title, 100);
                    parameterValues.Add(SqlDbType.VarChar, "LayoutType", ((int)ad.LayoutType).ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "SoundPosition", ad.SoundPosition.ToString("00"), 2);
                    parameterValues.Add(SqlDbType.VarChar, "Account", ad.Account, 2);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", ad.BeginDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", ad.EndDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", ad.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_LimitedAdInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region UpdateTransparentAdverInfo
        public bool UpdateTransparentAdverInfo(AdInfoRaw ad)
        {
             try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", ad.ID, 20);
                    parameterValues.Add(SqlDbType.VarChar, "Title", ad.Title, 100);
                    parameterValues.Add(SqlDbType.VarChar, "Account", ad.Account, 2);
                    parameterValues.Add(SqlDbType.VarChar, "BeginDate", ad.BeginDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "EndDate", ad.EndDate.ToString("yyyy-MM-dd HH:mm"), 20);
                    parameterValues.Add(SqlDbType.VarChar, "RegID", ad.RegID, 100);

                    using (var sdr = this.mssql.StoredProcedure("DID_TransparentAdInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        public bool UpdateMovieContentsSetting(string code, string isAuto, string id)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "Code", code.Trim(' '), 13);
                    parameterValues.Add(SqlDbType.Char, "IsAuto", isAuto.Equals("True") ? '1' : '0', 1);
                    parameterValues.Add(SqlDbType.VarChar, "ID", id, 100);
                    using (var sdr = this.mssql.StoredProcedure("DID_MovieContentsSettingInfo_UPDATE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }
                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Delete Method

        #region DeleteMovieContentsAvailable
        public bool DeleteMovieContentsAvailable(int id)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", id, 6);


                    using (var sdr = this.mssql.StoredProcedure("DID_MovieContentsUploadAvailableList_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeletePopupNoticeInfo
        public bool DeletePopupNoticeInfo(string noticeID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", noticeID, 17);


                    using (var sdr = this.mssql.StoredProcedure("DID_PopupNoticeInfo_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteScheduleInfo
        public bool DeleteScheduleInfo(string theater, string itemID, string contentsType)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                    parameterValues.Add(SqlDbType.Char, "ItemID", (int.Parse(itemID)).ToString("000"), 3);
                    parameterValues.Add(SqlDbType.Char, "ContentsType", contentsType, 2);

                    using (var sdr = this.mssql.StoredProcedure("DID_ScheduleInfo_DELETEALL", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteADInfo
        public bool DeleteADInfo(string id)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", id, 20);

                    using (var sdr = this.mssql.StoredProcedure("DID_AdInfo_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteTransparentAdInfo
        public bool DeleteTransparentAdInfo(string id)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", id, 20);

                    using (var sdr = this.mssql.StoredProcedure("DID_TransparentAdInfo_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteMediaInfo
        public bool DeleteMediaInfo(string id)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", id, 20);


                    using (var sdr = this.mssql.StoredProcedure("DID_MediaInfo_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteNoticeInfo
        public bool DeleteNoticeInfo(string noticeID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "NoticeID", noticeID, 17);


                    using (var sdr = this.mssql.StoredProcedure("DID_NoticeInfo_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteEventInfo
        public bool DeleteEventInfo(string eventid)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "EventID", eventid, 20);


                    using (var sdr = this.mssql.StoredProcedure("DID_EventInfo_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteSkinInfo
        public bool DeleteSkinInfo(string skinID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "ID", skinID, 20);


                    using (var sdr = this.mssql.StoredProcedure("DID_SkinInfo_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteSkinMapInfo
        public bool DeleteSkinMapInfo(string Code)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Char, "GroupID", Code, 20);


                    using (var sdr = this.mssql.StoredProcedure("DID_SkinMapInfo_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteMovieShowingInfo
        public bool DeleteMovieShowingInfo(string theater, string itemID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                    parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                    using (var sdr = this.mssql.StoredProcedure("DID_MovieShowingInfo_DELETE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteCheckMovieTime
        public bool DeleteCheckMovieTime(string id)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Int, "Seq", id);

                    using (var sdr = this.mssql.StoredProcedure("DID_CheckMovieTime_DELETE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeletePlannedMovieInfoList
        public bool DeletePlannedMovieInfoList(string theater, string itemID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                    parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                    using (var sdr = this.mssql.StoredProcedure("DID_PlannedMovieInfoList_DELETE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteRecommandMovieInfoList
        public bool DeleteRecommandMovieInfoList(string theater, string itemID)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                    parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 3);

                    using (var sdr = this.mssql.StoredProcedure("DID_RecommandedMovieInfoList_DELETE", parameterValues))
                    {
                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeleteMovieContentsInfo
        public bool DeleteMovieContentsInfo(int seq)
        {
            try
            {
                if (ConnectionDID())
                {
                    StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                    parameterValues.Add(SqlDbType.Int, "Seq", seq);

                    using (var sdr = this.mssql.StoredProcedure("DID_MovieContentsInfo_DELETE", parameterValues))
                    {
                        if (sdr != null)
                        {
                            sdr.Close();
                        }
                    }

                    DisConnection();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Private Method

        /// <summary>
        /// Current Server DATETIME Save
        /// </summary>
        private void SaveDateTime()
        {
            DateTime dt;

            if (ConnectionDID())
            {
                string query = "SELECT GETDATE()";

                using (var reader = this.mssql.ExecuteQuery(query))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            dt = (DateTime)reader[0];

                            this._dt.Hour = dt.Hour;
                            this._dt.Minute = dt.Minute;
                            this._dt.Datetime = dt.ToString("yyyy-MM-dd");

                            if (0 <= dt.Hour && dt.Hour <= 4)
                            {
                                this._dt.Hour = 24 + dt.Hour;
                                this._dt.Datetime = dt.AddDays(-1).ToString("yyyy-MM-dd");
                            }
                        }

                        reader.Close();
                    }
                }

                DisConnection();
            }
            else // DB연결이 안될경우 시스템의 시각을 가지고 온다. 
            {
                dt = DateTime.Now;

                this._dt.Hour = dt.Hour;
                this._dt.Minute = dt.Minute;

                if (0 <= dt.Hour && dt.Hour <= 4)
                {
                    this._dt.Hour = 24 + dt.Hour;
                    this._dt.Datetime = dt.AddDays(-1).ToString("yyyy-MM-dd");
                }
            }
        }

        #endregion

        #region LCSM

        #region GetMovieContentsInfoList

        public List<MovieContentsInfoRaw> GetLCSMMovieContentsInfoList(string contentsCode)
        {
            List<MovieContentsInfoRaw> list = new List<MovieContentsInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.Char, "ContentsCode", contentsCode, 6);

                using (var sdr = this.mssql.StoredProcedure("LCSM_MovieContentsInfo_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            MovieContentsInfoRaw item = new MovieContentsInfoRaw
                            {
                                Seq = DatabaseUtil.TryConvertToInteger(sdr["Seq"]),
                                ContentsCode = DatabaseUtil.TryConvertToString(sdr["ContentsCode"]),
                                MovieShortName = DatabaseUtil.TryConvertToString(sdr["MovieShortName"]),
                                LargePosterFileName = DatabaseUtil.TryConvertToString(sdr["LargePosterFileName"]),
                                SmallPosterFileName = DatabaseUtil.TryConvertToString(sdr["SmallPosterFileName"]),
                                MovieFileName = DatabaseUtil.TryConvertToString(sdr["MovieFileName"]),
                                RegDate = DatabaseUtil.TryConvertToDateTime(sdr["RegDate"]),
                                RegID = DatabaseUtil.TryConvertToString(sdr["RegID"])
                            };

                            list.Add(item);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }

                    DisConnection();
                }
            }

            return list;
        }

        #endregion

        #endregion

    }
}