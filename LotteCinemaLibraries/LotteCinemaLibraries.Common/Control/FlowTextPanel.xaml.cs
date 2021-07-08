using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LotteCinemaLibraries.Common.Control
{
    /// <summary>
    /// Interaction logic for FlowTextPanel.xaml
    /// </summary>
    public partial class FlowTextPanel : UserControl
    {
        #region public List<string> TextList
        public static readonly DependencyProperty TextListProperty =
            DependencyProperty.Register("TextList",
                typeof(List<string>),
                typeof(FlowTextPanel),
                new PropertyMetadata(null));

        public List<string> TextList
        {
            get { return (List<string>)GetValue(TextListProperty); }
            set { SetValue(TextListProperty, value); }
        }
        #endregion

        #region public bool Run
        public static readonly DependencyProperty RunProperty =
            DependencyProperty.Register("Run",
                typeof(bool),
                typeof(FlowTextPanel),
                new PropertyMetadata(false));

        public bool Run
        {
            get { return (bool)GetValue(RunProperty); }
            set
            {
                SetValue(RunProperty, value);
                if (value)
                {

                }
                else
                {
                    _index = -1;
                    _inCount = 0;
                    _outCount = 0;
                }
            }
        }
        #endregion

        #region public bool Loop
        public static readonly DependencyProperty LoopProperty =
            DependencyProperty.Register("Loop",
                typeof(bool),
                typeof(FlowTextPanel),
                new PropertyMetadata(false));

        public bool Loop
        {
            get { return (bool)GetValue(LoopProperty); }
            set
            {
                SetValue(LoopProperty, value);
            }
        }
        #endregion

        #region public bool Stop
        public static readonly DependencyProperty StopProperty =
            DependencyProperty.Register("Stop",
                typeof(bool),
                typeof(FlowTextPanel),
                new PropertyMetadata(false));

        public bool Stop
        {
            get { return (bool)GetValue(StopProperty); }
            set
            {
                SetValue(StopProperty, value);
            }
        }
        #endregion

        #region public double Frame
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame",
            typeof(double),
            typeof(FlowTextPanel),
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
            typeof(FlowTextPanel),
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
            typeof(FlowTextPanel),
            new PropertyMetadata(50d));

        public double SentenceSpacing
        {
            get { return (double)GetValue(SentenceSpacingProperty); }
            set { SetValue(SentenceSpacingProperty, value); }
        }
        #endregion

        public delegate void ASentenceAppear();
        /// <summary>
        /// You have to Set Loop = false
        /// </summary>
        public event ASentenceAppear OnASentenceAppear;

        public delegate void LastSentenceAppear();
        /// <summary>
        /// You have to Set Loop = false
        /// </summary>
        public event LastSentenceAppear OnLastSentenceAppear;

        public delegate void LastSentenceDisappear();
        /// <summary>
        /// You have to Set Loop = false
        /// </summary>
        public event LastSentenceDisappear OnLastSentenceDisappear;

        private List<FlowTextPanelItem> _currentShowingFlowTextList;
        private DateTime _beforeTime;
        private int _index = -1;

        private int _inCount = 0;
        private int _outCount = 0;

        public FlowTextPanel()
        {
            InitializeComponent();

            Loaded += FlowTextPanel_Loaded;

            _currentShowingFlowTextList = new List<FlowTextPanelItem>();
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

        public void UpdateTextList(List<string> textList)
        {
            TextList = null;
            TextList = textList;
        }

        public void AddAText(string text)
        {
            TextList.Add(text);
        }

        public void RemoveAText(int index)
        {
            TextList.RemoveAt(index);
        }

        private void Move()
        {
            if (Stop == false && _currentShowingFlowTextList.Count == 0)
            {
                string txt = GetNextText();
                if (txt != null)
                {
                    var item = ConvertTextOnPanel(txt);
                    item.Text.RenderTransform = new TranslateTransform { X = this.ActualWidth };
                    //item.Text.Margin = new Thickness(this.ActualWidth, 0, 0, 0);
                    AddFlowTextPanelItem(item);
                }
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
                    item.Text.Margin = new Thickness(0, 0, -remainWidth, 0);
                }

                #region 텍스트 추가, 제거

                // 텍스트 화면에 전부 보임. 다음 텍스트 추가
                if (transform.X <= this.ActualWidth - item.Text.ActualWidth && item.IsNextItem == false)
                {
                    if (OnASentenceAppear != null) OnASentenceAppear();

                    if (Stop == false)
                    {
                        item.IsNextItem = true;

                        string txt = GetNextText();

                        if (txt != null)
                        {
                            AddFlowTextPanelItem(ConvertTextOnPanel(txt));
                        }

                        if (_index == TextList.Count)
                        {
                            if (OnLastSentenceAppear != null) OnLastSentenceAppear();
                        }   
                    }
                }

                // 텍스트 화면에서 사라짐. 제거
                if (transform.X <= -item.Text.ActualWidth && item.IsRemove == false)
                {
                    item.IsRemove = true;

                    RemoveFlowTextPanelItem(0);

                    if (_inCount == _outCount)
                    {
                        _inCount = 0;
                        if (OnLastSentenceDisappear != null) OnLastSentenceDisappear();
                    }
                }

                #endregion
            }
        }

        public void AddFlowTextPanelItem(string txt)
        {
            AddFlowTextPanelItem(ConvertTextOnPanel(txt));
        }

        #region void AddFlowTextPanelItem(FlowTextPanelItem item)
        private void AddFlowTextPanelItem(FlowTextPanelItem item)
        {
            //Dispatcher.BeginInvoke(new Action(() =>
            //{
                _currentShowingFlowTextList.Add(item);
                MainCanvas.Children.Add(item.Text);
            //}));

            _inCount++;
        }
        #endregion

        #region void RemoveFlowTextPanelItem(int index)
        private void RemoveFlowTextPanelItem(int index)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _currentShowingFlowTextList.RemoveAt(index);
                MainCanvas.Children.RemoveAt(index);
            }));

            _outCount++;
        }
        #endregion

        #region FlowTextPanelItem ConvertTextOnPanel(string text)
        private FlowTextPanelItem ConvertTextOnPanel(string text)
        {
            var item = new FlowTextPanelItem
            {
                IsNextItem = false,
                IsRemove = false,
                IsClipToBounds = false,
                Text = new VinylTextBlock
                {
                    MyText = text,
                    MyFontColor = Brushes.White,
                    MyFontSize = 18,
                    RenderTransform = new TranslateTransform { X = this.ActualWidth + SentenceSpacing },
                    //Margin = new Thickness(this.ActualWidth + SentenceSpacing, 0, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left
                }
            };

            return item;
        }
        #endregion

        #region string GetNextText()
        private string GetNextText()
        {
            if (TextList.Count == 0)
            {
                return null;
            }

            _index++;
            if (_index >= TextList.Count)
            {
                _index = 0;

                if (Loop == false)
                {
                    _index = TextList.Count;
                    return null;
                }
            }

            return TextList[_index];
        }
        #endregion
    }

    class FlowTextPanelItem
    {
        public bool IsNextItem;
        public bool IsRemove;
        public bool IsClipToBounds;
        public VinylTextBlock Text;
    }
}
