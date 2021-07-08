using System.Windows;
using System.Windows.Controls;

namespace LotteCinemaLibraries.Common.Control
{
    /// <summary>
    /// Interaction logic for DownloadProgressControl.xaml
    /// </summary>
    public partial class DownloadProgressControl : UserControl
    {
        #region Dependency Property

        #region DownloadMessage

        public static readonly DependencyProperty DownloadMessageProperty =
            DependencyProperty.Register("DownloadMessage",
            typeof(string),
            typeof(DownloadProgressControl),
            new PropertyMetadata("파일을 다운로드 중 입니다."));

        public string DownloadMessage
        {
            get { return (string)this.GetValue(DownloadMessageProperty); }
            set { this.SetValue(DownloadMessageProperty, value); }
        }

        #endregion

        #region DownloadCount

        public static readonly DependencyProperty DownloadCountProperty =
            DependencyProperty.Register("DownloadCount",
            typeof(int),
            typeof(DownloadProgressControl),
            new PropertyMetadata(0));

        public int DownloadCount
        {
            get { return (int)this.GetValue(DownloadCountProperty); }
            set { this.SetValue(DownloadCountProperty, value); }
        }

        #endregion

        #region DownloadTotalCount

        public static readonly DependencyProperty DownloadTotalCountProperty =
            DependencyProperty.Register("DownloadTotalCount",
            typeof(int),
            typeof(DownloadProgressControl),
            new PropertyMetadata(0));

        public int DownloadTotalCount
        {
            get { return (int)this.GetValue(DownloadTotalCountProperty); }
            set { this.SetValue(DownloadTotalCountProperty, value); }
        }

        #endregion

        #region DownloadPercentage

        public static readonly DependencyProperty DownloadPercentageProperty =
            DependencyProperty.Register("DownloadPercentage",
            typeof(double),
            typeof(DownloadProgressControl),
            new PropertyMetadata(0d));

        public double DownloadPercentage
        {
            get { return (double)this.GetValue(DownloadPercentageProperty); }
            set { this.SetValue(DownloadPercentageProperty, value); }
        }

        #endregion

        #region DownloadFileName

        public static readonly DependencyProperty DownloadFileNameProperty =
            DependencyProperty.Register("DownloadFileName",
            typeof(string),
            typeof(DownloadProgressControl),
            new PropertyMetadata(""));

        public string DownloadFileName
        {
            get { return (string)this.GetValue(DownloadFileNameProperty); }
            set { this.SetValue(DownloadFileNameProperty, value); }
        }

        #endregion

        #region DownloadByte

        public static readonly DependencyProperty DownloadByteProperty =
            DependencyProperty.Register("DownloadByte",
            typeof(long),
            typeof(DownloadProgressControl),
            new PropertyMetadata(0L));

        public long DownloadByte
        {
            get { return (long)this.GetValue(DownloadByteProperty); }
            set { this.SetValue(DownloadByteProperty, value); }
        }

        #endregion

        #region DownloadTotalByte

        public static readonly DependencyProperty DownloadTotalByteProperty =
            DependencyProperty.Register("DownloadTotalByte",
            typeof(long),
            typeof(DownloadProgressControl),
            new PropertyMetadata(0L));

        public long DownloadTotalByte
        {
            get { return (long)this.GetValue(DownloadTotalByteProperty); }
            set { this.SetValue(DownloadTotalByteProperty, value); }
        }

        #endregion

        #region VisibleDownloadProgress

        public static readonly DependencyProperty VisibleDownloadProgressProperty =
            DependencyProperty.Register("VisibleDownloadProgress",
            typeof(Visibility),
            typeof(DownloadProgressControl),
            new PropertyMetadata(Visibility.Collapsed));

        public Visibility VisibleDownloadProgress
        {
            get { return (Visibility)this.GetValue(VisibleDownloadProgressProperty); }
            set { this.SetValue(VisibleDownloadProgressProperty, value); }
        }

        #endregion

        #endregion

        public DownloadProgressControl()
        {
            InitializeComponent();
        }
    }
}