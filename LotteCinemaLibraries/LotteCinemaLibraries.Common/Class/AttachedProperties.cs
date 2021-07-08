using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LotteCinemaLibraries.Common.Model;

namespace LotteCinemaLibraries.Common.Class
{
    public class AttachedProperties : DependencyObject
    {
        #region MediaState

        public static readonly DependencyProperty MediaStateProperty =
            DependencyProperty.RegisterAttached("MediaState",
            typeof(MediaState),
            typeof(AttachedProperties),
            new PropertyMetadata(ChangedMediaStateProperty));

        public static MediaState GetMediaState(DependencyObject obj)
        {
            return (MediaState)obj.GetValue(MediaStateProperty);
        }

        public static void SetMediaState(DependencyObject obj, MediaState value)
        {
            obj.SetValue(MediaStateProperty, value);
        }

        private static void ChangedMediaStateProperty(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var media = obj as MediaElement;
            var state = (MediaState)e.NewValue;

            switch (state)
            {
                case MediaState.Pause:
                    media.Pause();
                    break;

                case MediaState.Play:
                    media.Play();
                    break;

                case MediaState.Stop:
                    media.Stop();
                    break;

                case MediaState.Close:
                    media.Close();
                    break;
            }
        }

        #endregion

        #region StoryboardCompleted

        public static readonly DependencyProperty StoryboardCompletedProperty =
            DependencyProperty.RegisterAttached("StoryboardCompleted",
            typeof(ICommand),
            typeof(AttachedProperties),
            new PropertyMetadata(ChangedStoryboardCompletedProperty));

        public static ICommand GetStoryboardCompleted(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(StoryboardCompletedProperty);
        }

        public static void SetStoryboardCompleted(DependencyObject obj, ICommand value)
        {
            obj.SetValue(StoryboardCompletedProperty, value);
        }

        private static void ChangedStoryboardCompletedProperty(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var storyboard = obj as Storyboard;
            var command = e.NewValue as ICommand;

            if (storyboard != null && command != null)
            {
                storyboard.Completed += (s, args) => command.Execute(null);
            }
        }

        #endregion

        #region TextEdit

        public static readonly DependencyProperty TextEditProperty =
            DependencyProperty.RegisterAttached("TextEdit",
            typeof(TextEditItem),
            typeof(AttachedProperties),
            new PropertyMetadata(ChangedTextEditProperty));

        private static void ChangedTextEditProperty(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var tb = obj as TextBlock;
            var editItem = e.NewValue as TextEditItem;

            if (string.IsNullOrWhiteSpace(editItem.Text)) return;

            tb.Text = editItem.Text;
            var tr = new TextRange(tb.ContentStart, tb.ContentEnd);
            tr.ClearAllProperties();

            if (!string.IsNullOrWhiteSpace(editItem.FontFamily))
            {
                tb.FontFamily = new FontFamily(editItem.FontFamily);
            }

            List<string> strList = new List<string>();
            strList.Add(editItem.Foreground);
            strList.Add(editItem.Bold);

            TextBlockHelper.SetEditText(tb, strList);
        }

        public static TextEditItem GetTextEdit(DependencyObject obj)
        {
            return obj.GetValue(TextEditProperty) as TextEditItem;
        }

        public static void SetTextEdit(DependencyObject obj, TextEditItem value)
        {
            obj.SetValue(TextEditProperty, value);
        }

        #endregion
    }
}