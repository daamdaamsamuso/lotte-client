using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LotteCinemaLibraries.Common.Control
{
    public class FourDivisionMediaControl : ContentControl
    {
        #region Dependency Property

        #region UriSource

        public static DependencyProperty UriSourceProperty =
            DependencyProperty.Register("UriSource",
            typeof(List<Uri>),
            typeof(FourDivisionMediaControl),
            new PropertyMetadata(new List<Uri>()));

        public List<Uri> UriSource
        {
            get { return this.GetValue(UriSourceProperty) as List<Uri>; }
            set { this.SetValue(UriSourceProperty, value); }
        }

        #endregion

        #region IsDivision

        public static DependencyProperty IsDivisionProperty =
            DependencyProperty.Register("IsDivision",
            typeof(bool),
            typeof(FourDivisionMediaControl));

        public bool IsDivision
        {
            get { return (bool)this.GetValue(IsDivisionProperty); }
            set { this.SetValue(IsDivisionProperty, value); }
        }

        #endregion

        #region MediaState

        public static DependencyProperty MediaStateProperty =
            DependencyProperty.Register("MediaState",
            typeof(MediaState),
            typeof(FourDivisionMediaControl),
            new PropertyMetadata(MediaState.Manual, new PropertyChangedCallback(ChangedMediaStateProperty)));

        private static void ChangedMediaStateProperty(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as FourDivisionMediaControl).ChangedState();
        }

        public MediaState MediaState
        {
            get { return (MediaState)this.GetValue(MediaStateProperty); }
            set { this.SetValue(MediaStateProperty, value); }
        }

        #endregion

        #region SoundIndex

        public static DependencyProperty SoundIndexProperty =
            DependencyProperty.Register("SoundIndex",
            typeof(int),
            typeof(FourDivisionMediaControl),
            new PropertyMetadata(0));

        public int SoundIndex
        {
            get { return (int)this.GetValue(SoundIndexProperty); }
            set { this.SetValue(SoundIndexProperty, value); }
        }

        #endregion

        #endregion

        #region Vraible

        private Grid _rootGrid;
        private int _failedCount;
        private int _openedCount;

        #endregion

        #region Event

        #region MediaOpened

        public static RoutedEvent MediaOpenedEvent =
            EventManager.RegisterRoutedEvent("MediaOpened",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(FourDivisionMediaControl));

        public event RoutedEventHandler MediaOpened
        {
            add { this.AddHandler(MediaOpenedEvent, value); }
            remove { this.RemoveHandler(MediaOpenedEvent, value); }
        }

        #endregion

        #region MediaFailed

        public static RoutedEvent MediaFailedEvent =
            EventManager.RegisterRoutedEvent("MediaFailed",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(FourDivisionMediaControl));

        public event RoutedEventHandler MediaFailed
        {
            add { this.AddHandler(MediaFailedEvent, value); }
            remove { this.RemoveHandler(MediaFailedEvent, value); }
        }

        #endregion

        #region MediaClosed

        public static RoutedEvent MediaClosedEvent =
            EventManager.RegisterRoutedEvent("MediaClosed",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(FourDivisionMediaControl));

        public event RoutedEventHandler MediaClosed
        {
            add { this.AddHandler(MediaClosedEvent, value); }
            remove { this.RemoveHandler(MediaClosedEvent, value); }
        }

        #endregion

        #endregion

        #region Constructor

        public FourDivisionMediaControl()
        {
            this._rootGrid = new Grid();
            this.Content = this._rootGrid;

            this.Loaded += FourDivisionMediaControl_Loaded;
        }

        #endregion

        #region Private Method

        private void InitLayout()
        {
            if (this.IsDivision)
            {
                var width = this.ActualWidth / 2;
                var height = this.ActualHeight / 2;

                for (int i = 0; i < 4; i++)
                {
                    ColumnDefinition column = new ColumnDefinition();
                    column.Width = new GridLength(width, GridUnitType.Pixel);

                    RowDefinition row = new RowDefinition();
                    row.Height = new GridLength(height, GridUnitType.Pixel);

                    this._rootGrid.RowDefinitions.Add(row);
                    this._rootGrid.ColumnDefinitions.Add(column);
                }

                for (int row = 0; row < 2; row++)
                {
                    for (int column = 0; column < 2; column++)
                    {
                        var media = CreateMedia();

                        Grid.SetRow(media, row);
                        Grid.SetColumn(media, column);

                        this._rootGrid.Children.Add(media);
                    }
                }
            }
            else
            {
                this._rootGrid.Children.Add(CreateMedia());
            }
        }

        private void ChangedState()
        {
            switch (this.MediaState)
            {
                case System.Windows.Controls.MediaState.Play:
                    Play();
                    break;

                case System.Windows.Controls.MediaState.Pause:
                    Pause();
                    break;

                case System.Windows.Controls.MediaState.Stop:
                    Stop();
                    break;

                case System.Windows.Controls.MediaState.Close:
                    Close();
                    break;
            }
        }

        private void Play()
        {
            if (!this.IsLoaded) return;

            if (this._rootGrid.Children.Count == this.UriSource.Count)
            {
                int i = 0;

                foreach (MediaElement media in this._rootGrid.Children)
                {
                    media.Source = this.UriSource[i++];
                    media.IsMuted = this.SoundIndex != i;
                    media.Play();
                }

                this._rootGrid.Opacity = 0;

                var showStoryboard = CreateOpacityStoryboard(1.0, EasingMode.EaseIn);
                showStoryboard.Begin();
            }
        }

        private void Pause()
        {
            foreach (MediaElement media in this._rootGrid.Children)
            {
                media.Pause();
            }
        }

        private void Stop()
        {
            foreach (MediaElement media in this._rootGrid.Children)
            {
                media.Stop();
            }
        }

        private void Close()
        {
            var hideStoryboard = CreateOpacityStoryboard(0, EasingMode.EaseOut);
            hideStoryboard.Completed += hideStoryboard_Completed;

            hideStoryboard.Begin();
        }

        private MediaElement CreateMedia()
        {
            MediaElement media = new MediaElement();
            media.LoadedBehavior = MediaState.Manual;

            media.MediaFailed += media_MediaFailed;
            media.MediaOpened += media_MediaOpened;
            return media;

        }

        private Storyboard CreateOpacityStoryboard(double opacity, EasingMode easingMode)
        {
            Storyboard sb = new Storyboard();

            TimeSpan duration = TimeSpan.FromSeconds(0.5);

            DoubleAnimationUsingKeyFrames frames = new DoubleAnimationUsingKeyFrames();
            EasingDoubleKeyFrame opacityFrame = new EasingDoubleKeyFrame(opacity, KeyTime.FromTimeSpan(duration));

            opacityFrame.EasingFunction = new QuadraticEase { EasingMode = easingMode };

            frames.KeyFrames.Add(opacityFrame);
            sb.Children.Add(frames);

            Storyboard.SetTarget(sb, this._rootGrid);
            Storyboard.SetTargetProperty(sb, new PropertyPath(UIElement.OpacityProperty));

            return sb;
        }

        #endregion

        #region Event Handler

        private void FourDivisionMediaControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitLayout();
            ChangedState();
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (this.IsDivision)
            {
                ++this._openedCount;

                if (this._openedCount == 4)
                {
                    this.RaiseEvent(new RoutedEventArgs(FourDivisionMediaControl.MediaOpenedEvent));
                }
            }
            else
            {
                this.RaiseEvent(new RoutedEventArgs(FourDivisionMediaControl.MediaOpenedEvent));
            }     
        }

        private void media_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (this.IsDivision)
            {
                ++this._failedCount;

                if (this._failedCount == 4)
                {
                    this.RaiseEvent(new RoutedEventArgs(FourDivisionMediaControl.MediaFailedEvent));
                }
            }
            else
            {
                this.RaiseEvent(new RoutedEventArgs(FourDivisionMediaControl.MediaFailedEvent));
            }            
        }

        private void hideStoryboard_Completed(object sender, EventArgs e)
        {
            foreach (MediaElement media in this._rootGrid.Children)
            {
                media.Close();
            }

            this.RaiseEvent(new RoutedEventArgs(FourDivisionMediaControl.MediaClosedEvent));
        }

        #endregion
    }
}