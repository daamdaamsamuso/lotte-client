using FTP;
using lotte_Client.Config;
using lotte_Client.Databases;
using lotte_Client.Helpers;
using lotte_Client.Models;
using lotte_Client.Utils;
using LotteCinemaService.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace lotte_Client
{
    public partial class MainWindow : Window
    {
        private bool _isServer;
        private int _programNumber;

        private DispatcherTimer _fadeOutTimer;
        private DoubleAnimation _fadeInAnimation;
        private DoubleAnimation _fadeOutAnimation;

        public MainWindow()
        {
            DataContext = App.MainViewModel;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Log.WriteLine("MainWindow_Loaded");

            InitConfigValue();
            InitFadeAnimation();
            InitDatabaseController();
        }
        // 환경설정
        private void InitConfigValue()
        {
            ConfigValue.Instance.LoadConfigValue();
            Width = ConfigValue.Instance.SizeWidth;
            Height = ConfigValue.Instance.SizeHeight;
            //Cursor = Cursors.None;
        }

        // 페이드애니메이션
        private void InitFadeAnimation()
        {
            _fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(1.0d))
            };

            _fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1.0d))
            };

            _fadeOutTimer = new DispatcherTimer();
            _fadeOutTimer.Interval = TimeSpan.FromSeconds(ConfigValue.Instance.DurationTimeSec - 1);
            _fadeOutTimer.Tick += _fadeOutTimer_Tick;
        }

        void _fadeOutTimer_Tick(object sender, EventArgs e)
        {
            FadeOut();
            //_fadeOutTimer.Stop();

            Play();
        }


        // 페이드 인
        private void FadeIn()
        {
            Debug.WriteLine("FadeIn");
            Dispatcher.BeginInvoke(new Action(() =>
            {
                BeginAnimation(OpacityProperty, _fadeInAnimation);
            }));
        }

        // 페이드 아웃
        private void FadeOut()
        {
            Debug.WriteLine("FadeOut");
            Dispatcher.BeginInvoke(new Action(() =>
            {
                BeginAnimation(OpacityProperty, _fadeOutAnimation);
            }));
        }

        // 데이터베이스
        private void InitDatabaseController()
        {
            SystemMessage("File Downloading......");

            DatabaseController.Instance.DbCompleted += Instance_DbCompleted;
            DatabaseController.Instance.SetData(ConfigValue.Instance.IsLocalVersion);
        }

        // 데이터베이스 데이터 다운로드 완료 이벤트 (스케줄 시작)
        void Instance_DbCompleted()
        {
            if (DatabaseController.Instance.AdverList.Count == 0)
            {
                //LogController.Instance.NoAdver();
            }

            InitScheduleController();

            SystemMessageTextBlock.Visibility = Visibility.Collapsed;
        }

        // 스케줄
        private void InitScheduleController()
        {
            SystemMessage("System Loading......");
            Play();


        }


        // 시스템 메시지 (레이아웃 좌측 상단)
        private void SystemMessage(string message)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                SystemMessageTextBlock.Text = message
                ));
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    break;

                case Key.F1:
                    SystemMessageTextBlock.Visibility = SystemMessageTextBlock.Visibility == Visibility.Visible
                        ? Visibility.Collapsed
                        : Visibility.Visible;
                    break;
            }

            base.OnKeyDown(e);
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();

            if (e.ClickCount == 2)
            {
                Close();
            }
        }

        // 파일 재생
        private void PlayVideoImage(string source, ContentsType type)
        {
            if (type == ContentsType.Video)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        Me.Visibility = Visibility.Visible;
                        Me.Close();
                        Me.Source = new Uri(source);
                        Me.Play();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Log.WriteLine(ex.Message);
                    }
                }));
            }
            else if (type == ContentsType.Image)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        Img.Source = new BitmapImage(new Uri(source));
                        Me.Visibility = Visibility.Collapsed;

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Log.WriteLine(ex.Message);
                    }
                }));
            }
        }

        int idx = 0;

        private void Play()
        {
            _fadeOutTimer.Stop();
            _fadeOutTimer.Start();
            FadeIn();

            DatabaseController.Instance.AdverList.ToString();
            var item = DatabaseController.Instance.AdverList[idx];
            if (idx < (DatabaseController.Instance.AdverList.Count-1))
            {
                idx = idx + 1;
            }
            else
            {
                idx = 0;
            }

          
            if (item.LayoutType == LayoutType.VideoImage2)
            {
                PlayVideoImage(item.Contents[0].LocalFilePath, item.Contents[0].ContentsType);
            }
            else
            {
                PlayVideoImage(item.Contents[0].LocalFilePath,item.Contents[0].ContentsType);
            }
        }
    }
}
