using System;
using System.Windows.Threading;

namespace LotteCinemaLibraries.Common.Class
{
    public class AdSchedulerEventArgs
    {
        public int Account;
        public DateTime Time;

        public AdSchedulerEventArgs(int account, DateTime time)
        {
            this.Account = account;
            this.Time = time;
        }
    }

    public class AdScheduler
    {
        #region Variable

        private readonly int TOTAL_ADVER_ACCOUNT_NUM = 13;
        private readonly int BASE_HOUR_TIME_OF_DAY = 4;

        private int _account;
        private DispatcherTimer _schedulingTimer;
        private DateTime _startDateTime;
        private DateTime _endDateTime;
        private int _interval;

        #endregion

        #region Event

        public delegate void AdStartEventHandler(object obj, AdSchedulerEventArgs e);
        public event AdStartEventHandler AdverStart;

        #endregion

        #region Constructor

        public AdScheduler(int adverCount, string startTime, string endTime, int interval)
        {
            TOTAL_ADVER_ACCOUNT_NUM = adverCount;
            SetAdverScheduleList(startTime, endTime, interval);
            InitAdverSchedulingTimer();
        }

        #endregion

        #region Public Method

        public void Start()
        {
            CalculateInterval();

            if (this._schedulingTimer.Interval != TimeSpan.Zero)
            {
                this._schedulingTimer.Start();
            }
        }

        public void Stop()
        {
            if (this._schedulingTimer != null)
            {
                this._schedulingTimer.Stop();
            }
        }

        #endregion

        #region Private Method

        private void SetAdverScheduleList(string startTime, string endTime, int interval)
        {
            this._interval = interval;

            var nowTime = DateTime.Now;

            if (nowTime.Hour >= 0 && nowTime.Hour <= BASE_HOUR_TIME_OF_DAY)
            {
                nowTime = nowTime.AddDays(-1);
            }

            this._startDateTime = ConvertTo24Hour(nowTime, startTime);
            this._endDateTime = ConvertTo24Hour(nowTime, endTime);
        }

        private DateTime ConvertTo24Hour(DateTime today, string time)
        {
            var split = time.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            int hour = int.Parse(split[0]);
            int minute = int.Parse(split[1]);

            DateTime dt;

            if (hour < 24)
            {
                dt = new DateTime(today.Year, today.Month, today.Day, hour, minute, 0);
            }
            else
            {
                dt = new DateTime(today.Year, today.Month, today.Day, hour - 24, minute, 0).AddDays(1);
            }

            return dt;
        }

        private void InitAdverSchedulingTimer()
        {
            this._schedulingTimer = new DispatcherTimer();
            this._schedulingTimer.Tick += new EventHandler(_schedulingTimer_Tick);
        }

        private void CalculateInterval()
        {
            try
            {
                var nowTime = DateTime.Now;
                DateTime adTime = this._startDateTime;
                this._account = 1;

                // 광고 시작 시간 이후만
                if (nowTime > this._startDateTime)
                {
                    // 현재까지 광고 카운트 계산
                    var timeGap = nowTime - this._startDateTime;
                    int count = (int)Math.Round(timeGap.TotalSeconds / this._interval);

                    int totalSecond = count * this._interval;

                    adTime = this._startDateTime.AddSeconds(totalSecond);

                    // 다음 광고 시간 설정
                    while (adTime < nowTime)
                    {
                        adTime = adTime.AddSeconds(this._interval);
                    }

                    // 상영 구좌 계산
                    count = (int)(adTime - this._startDateTime).TotalSeconds / this._interval;
                    this._account = count % TOTAL_ADVER_ACCOUNT_NUM + 1;
                }

                if (this._account == 0)
                {
                    this._account = TOTAL_ADVER_ACCOUNT_NUM;
                }

                if (adTime <= this._endDateTime)
                {
                    var interval = adTime - DateTime.Now;
                    this._schedulingTimer.Interval = interval;
                }
                else
                {
                    this._schedulingTimer.Interval = TimeSpan.Zero;
                    this._schedulingTimer.Stop();
                }
            }
            catch
            {
                this._schedulingTimer.Interval = TimeSpan.Zero;
                this._schedulingTimer.Stop();
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine("account : " + this._account + " interval : " + this._schedulingTimer.Interval);
#endif
        }

        #endregion

        #region Event Handler

        private void _schedulingTimer_Tick(object sender, EventArgs e)
        {
            var OnAdverStart = AdverStart;

            if (OnAdverStart != null)
            {
                OnAdverStart(this, new AdSchedulerEventArgs(this._account, DateTime.Now));
                CalculateInterval();
            }
        }

        #endregion
    }
}