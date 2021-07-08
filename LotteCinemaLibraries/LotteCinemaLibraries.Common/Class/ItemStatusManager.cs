using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using LotteCinemaService.Model.Common;
using LotteCinemaService.WebAPI.Helper;

namespace LotteCinemaLibraries.Common.Class
{
    public class ItemStatusManager
    {
        #region Variable

        private string _ip;
        private string _queryStatus;
        private string _queryLog;
        private string _cinemaCode;
        private Uri _serverUri;
        private int _itemID;
        private DispatcherTimer _updateStatusTimer;

        #endregion

        #region Constructor

        public ItemStatusManager(Uri serverUri, string queryStatus, string queryLog, string cinemaCode, int itemID)
        {
            this._serverUri = serverUri;
            this._queryStatus = queryStatus;
            this._queryLog = queryLog;
            this._cinemaCode = cinemaCode;
            this._itemID = itemID;
        }

        #endregion

        #region Public Method

        public void UpdateStatus(int contentsID, DateTime beginTime)
        {
            ItemStatusInfo statusInfo = new ItemStatusInfo
            {
                ContentsID = contentsID,
                BeginTime = beginTime
            };

            Update(statusInfo);
        }

        public void WriteLog(DateTime beginTime, DateTime endTime, string adID, int contentsID)
        {
            AdLogInfo logInfo = new AdLogInfo
            {
                CinemaCode = this._cinemaCode,
                BeginTime = beginTime,
                EndTime = endTime,
                AdID = adID,
                ContentsID = contentsID.ToString(),
                ScreenCode = this._itemID.ToString()
            };

            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                WebApiHelper.PostJson<AdLogInfo>(this._queryLog, this._serverUri, logInfo);
            }));
        }

        public void StartUpdateStatusTimer(double interval = 60)
        {
            if (this._updateStatusTimer == null)
            {
                this._updateStatusTimer = new DispatcherTimer();
                this._updateStatusTimer.Interval = TimeSpan.FromSeconds(interval);
                this._updateStatusTimer.Tick += _updateTimer_Tick;
            }

            this._updateStatusTimer.Start();
        }

        public void StopUpdateTimer()
        {
            if (this._updateStatusTimer != null)
            {
                this._updateStatusTimer.Stop();
            }
        }

        #endregion

        #region Private Method

        private void SetLocalIP()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                var ipAddress = host.AddressList.LastOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
                this._ip = ipAddress.ToString();
            }
        }

        private void Update(ItemStatusInfo statusInfo)
        {
            if (this._cinemaCode == null)
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                if (string.IsNullOrWhiteSpace(this._ip))
                {
                    SetLocalIP();
                }

                statusInfo.CinemaCode = int.Parse(this._cinemaCode);
                statusInfo.IP = this._ip;
                statusInfo.ScreenCode = this._itemID;

                WebApiHelper.PostJson<ItemStatusInfo>(this._queryStatus, this._serverUri, statusInfo);
            }));
        }

        #endregion

        #region Event Handler

        private void _updateTimer_Tick(object sender, EventArgs e)
        {
            ItemStatusInfo statusInfo = new ItemStatusInfo
            {
                ContentsID = 0,
                BeginTime = DateTime.Now
            };

            Update(statusInfo);
        }

        #endregion
    }
}