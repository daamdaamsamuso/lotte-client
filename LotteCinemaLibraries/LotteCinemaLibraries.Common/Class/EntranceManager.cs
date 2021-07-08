using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LotteCinemaLibraries.Common.Enum;
using LotteCinemaLibraries.Common.Model;
using LotteCinemaService.Model.Common;

namespace LotteCinemaLibraries.Common.Class
{
    public class EntranceManager
    {
        #region Variable

        private List<MovieTimeCellInfo> _cellList;

        #endregion

        #region Property

        #region CurrentCellList

        private List<MovieTimeCellInfo> _currentCellList;
        public List<MovieTimeCellInfo> CurrentCellList
        {
            get { return this._currentCellList; }
        }

        #endregion

        #region CurrentIndex

        private int _currentCellIndex = 0;
        public int CurrentCellIndex
        {
            get { return this._currentCellIndex; }
        }

        #endregion

        #endregion

        #region Constructor

        public EntranceManager()
        {
        }

        #endregion

        #region Public Method

        public void Init(List<MovieTimeCellInfo> cellList, bool engScreenName = false)
        {
            this._cellList = cellList;

            foreach (var cell in cellList)
            {
                ReplaceTitle(ref cell.MovieKoreaName);
                cell.ScreenName = ScreenInfoHelper.ConvertScreenName(cell.ScreenName, engScreenName);
            }
        }

        public void BoxOfficeInit(List<MovieTimeCellInfo> cellList, bool engScreenName = false)
        {
            this._cellList = cellList;

            foreach (var cell in cellList)
            {
                ReplaceTitle(ref cell.MovieKoreaName);
                var tempSpecialName = string.Empty;
                if (cell.ScreenDivCode == "100")
                {
                }
                else if (cell.ScreenDivCode == "200")
                {
                    tempSpecialName = "CINE COUPLE";
                }
                else if (cell.ScreenDivCode == "300")
                {
                    tempSpecialName = "CHARLOTTE";
                }
                else if (cell.ScreenDivCode == "301")
                {
                    tempSpecialName = "CHARLOTTE PRIVATE";
                }
                else if (cell.ScreenDivCode == "400")
                {
                    tempSpecialName = "ARTE CLASSIC";
                }
                else if (cell.ScreenDivCode == "401")
                {
                    tempSpecialName = "ARTE ANI";
                }
                else if (cell.ScreenDivCode == "700")
                {
                    tempSpecialName = "WINE CINEMA TRAIN";
                }
                else if (cell.ScreenDivCode == "800")
                {
                    tempSpecialName = "KTX CINEMA";
                }
                else if (cell.ScreenDivCode == "900")
                {
                    tempSpecialName = "SUPER SOUND";
                }
                else if (cell.ScreenDivCode == "910")
                {
                    tempSpecialName = "SUPER VIBE";
                }
                else if (cell.ScreenDivCode == "920")
                {
                    tempSpecialName = "CINE COUPLE SEAT";
                }
                else if (cell.ScreenDivCode == "930")
                {
                    tempSpecialName = "SUPER4D";
                }
                else if (cell.ScreenDivCode == "940")
                {
                    tempSpecialName = "SUPER PLEX";
                }
                else if (cell.ScreenDivCode == "941")
                {
                    tempSpecialName = "SUPER PLEX G";
                }
                else if (cell.ScreenDivCode == "950")
                {
                    tempSpecialName = "CINE BIZ";
                }
                else if (cell.ScreenDivCode == "960")
                {
                    tempSpecialName = "CINE FAMILY";
                }
                //2019.02.09
                else if (cell.ScreenDivCode == "980")
                {
                    tempSpecialName = "SUPERS";
                }
                //2020.12.09
                else if (cell.ScreenDivCode == "988")
                {
                    tempSpecialName = "COLORIUM";
                }

                if (cell.CinemaCode == "3024")
                {
                    if (cell.ScreenNumber == 5)
                    {
                        //tempSpecialName = "장안대학교";
                    }
                }
                if (cell.CinemaCode == "1016")
                {
                    if (cell.ScreenNumber == 10)
                    {
                        tempSpecialName = "";
                    }
                }

                cell.ScreenName = tempSpecialName +" ";
               // cell.ScreenName = ScreenInfoHelper.ConvertScreenName(cell.ScreenName, engScreenName);
            }
        }

        public void UpdateCurrentCellList(double beforeMinute, double afterMinute)
        {
            try
            {
                this._currentCellList = null;

                var before = DateTime.Now.AddMinutes(-beforeMinute);
                var after = DateTime.Now.AddMinutes(afterMinute);

                this._currentCellList = (from cell in this._cellList
                                         where ConvertStrToDateTime(cell.StartTime) != DateTime.MaxValue && 
                                               ConvertStrToDateTime(cell.StartTime).CompareTo(before) == 1 && 
                                               ConvertStrToDateTime(cell.StartTime).CompareTo(after) == -1
                                         select cell).ToList();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private DateTime ConvertStrToDateTime(string str)
        {
            DateTime dt = DateTime.MaxValue;

            if (!DateTime.TryParse(str, out dt))
            {
              
                if (str.CompareTo("24:00") >= 0)
                {
                    var split = str.Split(':');
                    var hours = double.Parse(split[0]);
                    var minutes = double.Parse(split[1]);

                    

                    var CheckTime = DateTime.Now.Hour;
                    if (CheckTime >= 0 && CheckTime < 4)
                    {
                        dt = DateTime.Today.AddDays(-1).AddHours(hours).AddMinutes(minutes);
                        //dt = DateTime.Today.AddHours(hours).AddMinutes(minutes);
                    }
                    else
                    {
                        dt = DateTime.Today.AddHours(hours).AddMinutes(minutes);
                    }
                }
            }

            return dt;
        }

        public string GetScreenFloor(MovieTimeCellInfo cell)
        {
            if (cell.ScreenFloor > 0)
            {
                return string.Format("{0}층", cell.ScreenFloor);
            }
            else
            {
                string temp = cell.ScreenFloor.ToString().Replace("-", "B");
                //return string.Empty;


                return string.Format("{0}층", temp);
            }
        }

        public string GetScreenNumber(MovieTimeCellInfo cell, string cinemaCode)
        {
            var type = ScreenInfoHelper.GetScreenType(cell.ScreenName);

            if (cinemaCode != "1016" && (type == ScreenType.CHARLOTTE || type == ScreenType.CHARLOTTE_PRIVATE))
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0}관", cell.ScreenNumber);
            }
        }

        public MovieTimeCellInfo GetNextCell()
        {
            if (this._currentCellIndex < this._currentCellList.Count)
            {
                return this._currentCellList[this._currentCellIndex++];
            }
            else
            {
                return null;
            }
        }

        public void CheckCurrentCellIndex()
        {
            if (this._currentCellIndex >= this._currentCellList.Count)
            {
                this._currentCellIndex = 0;
            }
        }

        public void UpdateEntranceInfo(EntranceInfo entranceInfo, MovieTimeCellInfo cell, string cinemaCode)
        {
            if (cell != null)
            {
                entranceInfo.Title = cell.MovieKoreaName;
                entranceInfo.StartTime = cell.StartTime;
                entranceInfo.ScreenNumber = GetScreenNumber(cell, cinemaCode);
                entranceInfo.ScreenName = cell.ScreenName;
                entranceInfo.NoticeVisible = Visibility.Visible;
                entranceInfo.ScreenFloor = GetScreenFloor(cell);

                if (cell.ScreenName == "WOW ")
                {
                    entranceInfo.ScreenNumber = "";
                    entranceInfo.ScreenName = "WOW관 (10관)";
                    //entranceInfo.ScreenFloor = "(7층)";
                }
            }
            else
            {
                entranceInfo.Title = string.Empty;
                entranceInfo.StartTime = string.Empty;
                entranceInfo.ScreenNumber = string.Empty;
                entranceInfo.ScreenName = string.Empty;
                entranceInfo.ScreenFloor = string.Empty;
                entranceInfo.NoticeVisible = Visibility.Collapsed;
            }
        }

        #endregion

        #region Private Method

        private void ReplaceTitle(ref string title)
        {
            string oldTitle = title;

            if (title.Contains("["))
            {
                title = title.Replace("[", "").Replace("]", "");
            }

            if (title.Contains("(") && title.Contains(")"))
            {
                int leftIndex = title.IndexOf("(");
                int rightIndex = title.IndexOf(")");

                if (leftIndex < rightIndex)
                {
                    title = title.Remove(leftIndex, rightIndex - leftIndex + 1);
                }
            }

            if (oldTitle.Contains("(일어자막)"))
            {
                title = "(일어자막)" + title;
            }

            if (oldTitle.Contains("(영어자막)"))
            {
                title = "(영어자막)" + title;
            }
        }

        #endregion
    }
}