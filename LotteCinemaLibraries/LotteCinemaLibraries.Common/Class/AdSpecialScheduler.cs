using System;
using System.Collections.Generic;
using System.Linq;
using LotteCinemaService.Model.Common;

namespace LotteCinemaLibraries.Common.Class
{
    public class AdSpecialScheduler
    {
        #region Variable

        private List<AdInfo> _scheduleInfos;
        private List<AdInfo> _currentScheduleInfos;
        private AdInfo _currentSchedule;

        #endregion

        #region Constructor

        public AdSpecialScheduler(List<AdInfo> scheduleInfos)
        {
            this._scheduleInfos = scheduleInfos;
            this._currentScheduleInfos = new List<AdInfo>();
        }

        #endregion

        #region Public Method

        public AdInfo GetCurrentSchedule()
        {
            if (this._scheduleInfos == null || this._scheduleInfos.Count == 0) return null;

            var dt = DateTime.Now;

            var currentScheduleInfos = (from sd in this._scheduleInfos
                                        where sd.BeginDate <= dt && sd.EndDate > dt
                                        select sd).ToList();

            if (currentScheduleInfos.Count == 0)
            {
                return null;
            }

            var equal = this._currentScheduleInfos.SequenceEqual(currentScheduleInfos);

            if (!equal)
            {
                this._currentScheduleInfos = currentScheduleInfos;
            }   

            this._currentSchedule = FindSchedule();

#if DEBUG
            System.Diagnostics.Debug.WriteLine("special : " + this._currentSchedule.ID + " | " + this._currentSchedule.BeginDate + " | " + this._currentSchedule.EndDate);
#endif

            return this._currentSchedule;
        }

        public AdInfo GetCurrentSchedule(string adID)
        {
            if (this._scheduleInfos == null || this._scheduleInfos.Count == 0) return null;

            var scheduleInfo = (from sd in this._scheduleInfos
                                        where sd.ID == adID
                                        select sd).FirstOrDefault();

            return scheduleInfo;
        }

        #endregion

        #region Private Method

        private AdInfo FindSchedule()
        {
            if (this._currentSchedule == null || !this._currentScheduleInfos.Contains(this._currentSchedule))
            {
                return this._currentScheduleInfos[0];
            }
            else
            {
                var index = this._currentScheduleInfos.IndexOf(this._currentSchedule);

                if (index < this._currentScheduleInfos.Count - 1)
                {
                    return this._currentScheduleInfos[index + 1];
                }
                else
                {
                    return this._currentScheduleInfos[0];
                }
            }
        }

        #endregion
    }
}