using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.WebAPI.Helper;
using LotteCinemaService.WebAPI.Helper.LCSM;

namespace LotteCinemaService.Database.Manager
{
    public class NowShowingManager : DatabaseManager
    {
        public NowShowingManager(string server)
            : base(server)
        {
        }

        public List<MovieShowingInfoRaw> GetMovieShowingInfoList(string theater, string itemID)
        {
            List<MovieShowingInfoRaw> list = new List<MovieShowingInfoRaw>();

            if (ConnectionDID())
            {
                StoredProcedureParameterValueList parameterValues = new StoredProcedureParameterValueList();
                parameterValues.Add(SqlDbType.VarChar, "Theater", theater, 10);
                parameterValues.Add(SqlDbType.Char, "ItemID", itemID, 10);

                using (var sdr = this.mssql.StoredProcedure("LCSM_MovieShowingInfoList_SELECT", parameterValues))
                {
                    if (sdr != null)
                    {
                        while (sdr.Read())
                        {
                            MovieShowingInfoRaw item = new MovieShowingInfoRaw
                            {
                                Seq = Convert.ToInt32(sdr["Seq"]),
                                OrderNo = Convert.ToInt32(sdr["OrderNo"]),
                                Theater = Convert.ToString(sdr["Theater"]),
                                ContentsCode = Convert.ToString(sdr["ContentsCode"]),
                                Title = Convert.ToString(sdr["Title"]),
                                ItemID = Convert.ToString(sdr["ItemID"]),
                                //RunningTime = Convert.ToString(sdr["RunningTime"]),
                                //WatchClass = Convert.ToString(sdr["WatchClass"]),
                                //OpenDate = Convert.ToString(sdr["OpenDate"]),
                                //Synopsis = Convert.ToString(sdr["Synopsis"]),
                                //Country = Convert.ToString(sdr["Country"]),
                                //Genre = Convert.ToString(sdr["Genre"]),
                                //Casts = Convert.ToString(sdr["Casts"]),
                                //Direction = Convert.ToString(sdr["Direction"]),
                            };
                            list.Add(item);
                        }

                        sdr.Close();
                        sdr.Dispose();
                    }
                    DisConnection();
                }

                foreach (var item in list)
                {
                    var movieInfo = GetMovieShowingInfoList_MovieInfo(item.ContentsCode);
                    item.RunningTime = movieInfo.RunningTime;
                    item.WatchClass = ConvertAgeToGrade(movieInfo.WatchClass);
                    item.OpenDate = movieInfo.OpenDate.ToString("yyyy-MM-dd");
                    item.Synopsis = movieInfo.Synopsis;
                    item.Genre = movieInfo.Genre;
                    item.Casts = movieInfo.Casts;
                    item.Direction = movieInfo.Direction;
                }
            }

            return list;
        }

        private string ConvertAgeToGrade(string watchClass)
        {
            return watchClass;
        }

        public MappedMovieShowingInfoRaw_MovieInfo GetMovieShowingInfoList_MovieInfo(string contentsCode)
        {
            var item = new MappedMovieShowingInfoRaw_MovieInfo();

            var datasets = WebApiHelper.GetResultDataSet(
                WebApiLCSMQuery.GetMovieShowingInfoList_GetMovieInfo(contentsCode),
                new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            if (datasets.Tables.Count > 0)
            {
                var items = datasets.Tables[0].DataTableToList<MappedMovieShowingInfoRaw_MovieInfo>();

                if (items != null)
                {
                    if (items.Count > 0)
                    {
                        item = items[0];
                    }
                }
            }

            return item;
        }

        public List<MovieShowingInfoRaw> GetMovieShowingInfoAutoList(string theater, string itemID, string playDate)
        {
            var list = new List<MovieShowingInfoRaw>();

            var intItemId = 0;
            try
            {
                intItemId = Convert.ToInt32(itemID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetMovieShowingInfoAutoList(theater, intItemId, playDate), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            if (datasets.Tables.Count > 0)
            {
                var items = datasets.Tables[0].DataTableToList<MappedMovieShowingInfoRaw>();

                if (items != null)
                {
                    list.AddRange(items.Select(item => new MovieShowingInfoRaw
                    {
                        ContentsCode = item.ContentsCode,
                        Title = item.Title,
                        ItemID = item.ItemID,
                        RunningTime = item.RunningTime,
                        WatchClass = ConvertAgeToGrade(item.WatchClass),
                        OpenDate = item.OpenDate.ToString("yyyy-MM-dd"),
                        Synopsis = item.Synopsis,
                        Country = item.Country,
                        Genre = item.Genre,
                        Casts = item.Casts,
                        Direction = item.Direction
                    }));
                }
            }

            ////////////////////////////////////////////////////////////////////
            // !!리스트 개수 만큼 컨텐츠 코드 조회함
            // !!속도가 너무 느리면 MovieContentsInfoList 한번에 받아와서 비교문으로 변경 해야함
            for (int i = list.Count - 1; i >= 0; i--)
                        {
                var item = list[i];

                if (string.IsNullOrEmpty(item.ContentsCode))
                            {
                    list.Remove(item);
                }
                else
                {
                    var movieContentsInfoItems = GetLCSMMovieContentsInfoList(item.ContentsCode);
                    if (movieContentsInfoItems.Count > 0)
                    {
                        if (string.IsNullOrEmpty(movieContentsInfoItems[0].LargePosterFileName)
                            || string.IsNullOrEmpty(movieContentsInfoItems[0].SmallPosterFileName)
                            || string.IsNullOrEmpty(movieContentsInfoItems[0].MovieFileName))
                        {
                            list.Remove(item);
                        }
                        }
                    else
                    {
                        list.Remove(item);
                    }
                }
            }

            return list;
        }

        public List<MovistInfoRaw> GetMovistInfoList(string contentsCode)
        {
            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetMovistInfoRaw(contentsCode), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            var items = datasets.Tables[0].DataTableToList<MappedMovistInfoRaw>();
            List<MovistInfoRaw> returnValue = new List<MovistInfoRaw>();
            foreach (var item in items)
            {
                returnValue.Add(new MovistInfoRaw()
                {
                   MovistType = item.MovistType,
                   Name = item.NameKor,
                   Priority = item.Priority
                });
            }
            return returnValue;
        }

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
