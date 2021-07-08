using System.ComponentModel;
using System.Windows;

namespace LotteCinemaLibraries.Common.Model
{
    public class EntranceInfo : INotifyPropertyChanged
    {
        #region Property

        #region StartTime

        private string startTime;
        public string StartTime
        {
            get { return this.startTime; }
            set
            {
                if (this.startTime != value)
                {
                    this.startTime = value;
                    this.RaisePropertyChanged("StartTime");
                }
            }
        }

        #endregion

        #region ScreenFloor

        private string screenFloor;
        public string ScreenFloor
        {
            get { return this.screenFloor; }
            set
            {
                if (this.screenFloor != value)
                {
                    this.screenFloor = value;
                    this.RaisePropertyChanged("ScreenFloor");
                }
            }
        }

        #endregion

        #region ScreenNumber

        private string screenNumber;
        public string ScreenNumber
        {
            get { return this.screenNumber; }
            set
            {
                if (this.screenNumber != value)
                {
                    this.screenNumber = value;
                    this.RaisePropertyChanged("ScreenNumber");
                }
            }
        }

        #endregion

        #region ScreenName

        private string screenName;
        public string ScreenName
        {
            get { return this.screenName; }
            set
            {
                if (this.screenName != value)
                {
                    this.screenName = value;
                    this.RaisePropertyChanged("ScreenName");
                }
            }
        }

        #endregion

        #region Title

        private string title;
        public string Title
        {
            get { return this.title; }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }

        #endregion

        #region FontFamily

        private string fontFamily = "나눔고딕";
        public string FontFamily
        {
            get { return this.fontFamily; }
            set
            {
                if (this.fontFamily != value)
                {
                    this.fontFamily = value;
                    this.RaisePropertyChanged("FontFamily");
                }
            }
        }

        #endregion

        #region NoticeVisible

        private Visibility noticeVisible = Visibility.Collapsed;
        public Visibility NoticeVisible
        {
            get { return this.noticeVisible; }
            set
            {
                if (this.noticeVisible != value)
                {
                    this.noticeVisible = value;
                    this.RaisePropertyChanged("NoticeVisible");
                }
            }
        }

        #endregion

        #endregion

        #region Contructor

        public EntranceInfo()
        {
            
        }

        #endregion

        #region RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            var onPropertyChanged = PropertyChanged;
            if (onPropertyChanged != null)
            {
                onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}