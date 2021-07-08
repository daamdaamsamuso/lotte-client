using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace LotteCinemaLibraries.Common.Control
{
    public partial class DirectFlowTextPanel : UserControl
    {
        #region public bool Run
        public static readonly DependencyProperty RunProperty =
            DependencyProperty.Register("Run",
                typeof(bool),
                typeof(DirectFlowTextPanel),
                new PropertyMetadata(false));

        public bool Run
        {
            get { return (bool)GetValue(RunProperty); }
            set
            {
                SetValue(RunProperty, value);
            }
        }
        #endregion

        #region public double Frame
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame",
            typeof(double),
            typeof(DirectFlowTextPanel),
            new PropertyMetadata(100d));

        public double Frame
        {
            get { return (double)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }
        #endregion

        #region public double Speed
        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed",
            typeof(double),
            typeof(DirectFlowTextPanel),
            new PropertyMetadata(1d));

        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }
        #endregion

        #region public double SentenceSpacing
        public static readonly DependencyProperty SentenceSpacingProperty =
            DependencyProperty.Register("SentenceSpacing",
            typeof(double),
            typeof(DirectFlowTextPanel),
            new PropertyMetadata(50d));

        public double SentenceSpacing
        {
            get { return (double)GetValue(SentenceSpacingProperty); }
            set { SetValue(SentenceSpacingProperty, value); }
        }
        #endregion

        public delegate void ASentenceAppear();
        public event ASentenceAppear OnASentenceAppear;

        private readonly List<FlowTextPanelItem> _currentShowingFlowTextList;
        private DateTime _beforeTime;

        public DirectFlowTextPanel()
        {
            InitializeComponent();

            Loaded += FlowTextPanel_Loaded;

            _currentShowingFlowTextList = new List<FlowTextPanelItem>();
        }

        public void AddFlowTextPanelItem(string txt)
        {
            AddFlowTextPanelItem(ConvertTextOnPanel(txt));
        }

        public void AddFlowTextPanelItem(TextBlock textBlock)
        {
            AddFlowTextPanelItem(ConvertTextOnPanel(textBlock));
        }

        void FlowTextPanel_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (Run == false) return;

            var currentTime = DateTime.Now;

            double frame = 1d / Frame;

            if (currentTime >= _beforeTime.AddSeconds(frame))
            {
                _beforeTime = currentTime;

                Move();
            }
        }

        private void Move()
        {
            if (_currentShowingFlowTextList.Count == 0)
            {
                return;
            }

            for (int i = 0; i < _currentShowingFlowTextList.Count; i++)
            {
                var item = _currentShowingFlowTextList[i];

                if (item == null) return;

                var transform = item.Text.RenderTransform as TranslateTransform;

                if (transform == null) return;

                transform.X = transform.X - Speed; // 텍스트 이동
                //item.Text.Margin = new Thickness(item.Text.Margin.Left - Speed, 0, 0, 0);

                if (item.Text.ActualWidth <= 0) return;

                if (item.IsClipToBounds == false && item.Text.ActualWidth > this.ActualWidth)
                {
                    item.IsClipToBounds = true;

                    double remainWidth = item.Text.ActualWidth - this.ActualWidth;
                    item.Text.Margin = new Thickness(0, 0, -5, 0);
                }

                #region 텍스트 추가, 제거

                // 텍스트 화면에 전부 보임. 다음 텍스트 추가
                if (transform.X <= this.ActualWidth - item.Text.ActualWidth && item.IsNextItem == false)
                {
                    item.IsNextItem = true;

                    if (OnASentenceAppear != null) OnASentenceAppear();
                }

                // 텍스트 화면에서 사라짐. 제거
                if (transform.X <= -item.Text.ActualWidth && item.IsRemove == false)
                {
                    item.IsRemove = true;

                    RemoveFlowTextPanelItem(0);
                }

                #endregion
            }
        }

        private void AddFlowTextPanelItem(FlowTextPanelItem item)
        {
            
            Dispatcher.BeginInvoke(new Action(() =>
            {

                MainCanvas.Children.Add(item.Text);
                _currentShowingFlowTextList.Add(item);
            }));
        }


        private void RemoveFlowTextPanelItem(int index)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MainCanvas.Children.RemoveAt(index);
                _currentShowingFlowTextList.RemoveAt(index);
            }));
        }

        private FlowTextPanelItem ConvertTextOnPanel(string text)
        {
            var item = new FlowTextPanelItem
            {
                IsNextItem = false,
                IsRemove = false,
                IsClipToBounds = false,
                Text = new TextBlock
                {
                    Text = text,
                    Foreground = Brushes.White,
                    FontSize = FontSize,
                    FontFamily = FontFamily,
                    RenderTransform = new TranslateTransform {X = this.ActualWidth + SentenceSpacing},
                    HorizontalAlignment = HorizontalAlignment.Left,
                },
            };

            return item;
        }

        private FlowTextPanelItem ConvertTextOnPanel(TextBlock textBlock)
        {
            var item = new FlowTextPanelItem
            {
                IsNextItem = false,
                IsRemove = false,
                IsClipToBounds = false,
                Text = textBlock,
            };

            item.Text.FontSize = FontSize;
            item.Text.FontFamily = FontFamily;
            item.Text.RenderTransform = new TranslateTransform {X = this.ActualWidth + SentenceSpacing};
            item.Text.HorizontalAlignment = HorizontalAlignment.Left;

            return item;
        }

        class FlowTextPanelItem
        {
            public bool IsNextItem;
            public bool IsRemove;
            public bool IsClipToBounds;
            public TextBlock Text = new TextBlock();
        }
    }

    
}
