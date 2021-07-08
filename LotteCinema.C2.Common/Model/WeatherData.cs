using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using LotteCinema.C2.Common.Enum;

namespace LotteCinema.C2.Common.Model
{
    public class WeatherData : INotifyPropertyChanged
    {  
        #region Property

        #region Temp

        private string temp = "25";
        public string Temp
        {
            get { return this.temp; }
            set
            {
                if (this.temp != value)
                {
                    this.temp = value;
                    this.RaisePropertyChanged("Temp");
                }
            }
        }

        #endregion

        #region WeatherKind

        private WeatherKind weatherKind;
        public WeatherKind WeatherKind
        {
            get { return this.weatherKind; }
            set
            {
                if (this.weatherKind != value)
                {
                    this.weatherKind = value;
                    this.RaisePropertyChanged("WeatherKind");
                }
            }
        }

        #endregion

        #region Icon

        private BitmapImage icon = new BitmapImage(new Uri("../../Assets/Image/Icon/Weather_Clear.png", UriKind.Relative));
        public BitmapImage Icon
        {
            get { return this.icon; }
            set
            {
                if (this.icon != value)
                {
                    this.icon = value;
                    this.RaisePropertyChanged("Icon");
                }
            }
        }

        #endregion

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