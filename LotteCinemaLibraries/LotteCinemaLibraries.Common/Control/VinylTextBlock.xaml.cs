using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace LotteCinemaLibraries.Common.Control
{
    /// <summary>
    /// Interaction logic for VinylTextBlock.xaml
    /// </summary>
    public partial class VinylTextBlock : FrameworkElement
    {
        string myText = "TEST";
        double myFontSize = 22;
        Brush myFontColor = Brushes.Black;
        double myFontHorizontalSpace = 0;
        double myFontVerticalSpace = 0;
        FontWeight myFontWeight = FontWeights.Normal;
        FontFamily myFontFamily = new FontFamily("맑은 고딕");
        bool myIsWrappingEnabled = false;
        bool myIsTrimmingEnabled = false;
        double myMaxWidth = 0;
        double myMaxHeight = 0;

        public static readonly DependencyProperty myIsWrappingEnabledProperty;
        public static readonly DependencyProperty myIsTrimmingEnabledProperty;
        public static readonly DependencyProperty myTextProperty;
        public static readonly DependencyProperty myFontSizeProperty;
        public static readonly DependencyProperty myFontColorProperty;
        public static readonly DependencyProperty myFontHorizontalSpaceProperty;
        public static readonly DependencyProperty myFontVerticalSpaceProperty;
        public static readonly DependencyProperty myFontWeightProperty;
        public static readonly DependencyProperty myFontFamilyProperty;
        public static readonly DependencyProperty myMaxWidthProperty;
        public static readonly DependencyProperty myMaxHeightProperty;

        public bool MyIsWrappingEnabled
        {
            get { return (bool)GetValue(myIsWrappingEnabledProperty); }
            set { SetValue(myIsWrappingEnabledProperty, value); }
        }

        public bool MyIsTrimmingEnabled
        {
            get { return (bool)GetValue(myIsTrimmingEnabledProperty); }
            set { SetValue(myIsTrimmingEnabledProperty, value); }
        }

        public string MyText
        {
            get { return (string)GetValue(myTextProperty); }
            set { SetValue(myTextProperty, value); }
        }

        public double MyFontSize
        {
            get { return (double)GetValue(myFontSizeProperty); }
            set { SetValue(myFontSizeProperty, value); }
        }

        public Brush MyFontColor
        {
            get { return (Brush)GetValue(myFontColorProperty); }
            set { SetValue(myFontColorProperty, value); }
        }

        public double MyFontHorizontalSpace
        {
            get { return (double)GetValue(myFontHorizontalSpaceProperty); }
            set { SetValue(myFontHorizontalSpaceProperty, value); }
        }

        public double MyFontVerticalSpace
        {
            get { return (double)GetValue(myFontVerticalSpaceProperty); }
            set { SetValue(myFontVerticalSpaceProperty, value); }
        }

        public FontWeight MyFontWeight
        {
            get { return (FontWeight)GetValue(myFontWeightProperty); }
            set { SetValue(myFontWeightProperty, value); }
        }

        public FontFamily MyFontFamily
        {
            get { return (FontFamily)GetValue(myFontFamilyProperty); }
            set { SetValue(myFontFamilyProperty, value); }
        }

        public double MyMaxWidth
        {
            get { return (double)GetValue(myMaxWidthProperty); }
            set { SetValue(myFontFamilyProperty, value); }
        }

        public double MyMaxHeight
        {
            get { return (double)GetValue(myMaxHeightProperty); }
            set { SetValue(myFontFamilyProperty, value); }
        }

        static VinylTextBlock()
        {
            myIsWrappingEnabledProperty = DependencyProperty.Register("MyIsWrappingEnabled", typeof(bool), typeof(VinylTextBlock), new PropertyMetadata(myIsWrappingEnabledChanged));
            myIsTrimmingEnabledProperty = DependencyProperty.Register("MyIsTrimmingEnabled", typeof(bool), typeof(VinylTextBlock), new PropertyMetadata(myIsTrimmingEnabledChanged));
            myTextProperty = DependencyProperty.Register("MyText", typeof(string), typeof(VinylTextBlock), new PropertyMetadata(myTextChanged));
            myFontSizeProperty = DependencyProperty.Register("MyFontSize", typeof(double), typeof(VinylTextBlock), new PropertyMetadata(myFontSizeChanged));
            myFontColorProperty = DependencyProperty.Register("MyFontColor", typeof(Brush), typeof(VinylTextBlock), new PropertyMetadata(myFontColorChanged));
            myFontHorizontalSpaceProperty = DependencyProperty.Register("MyFontHorizontalSpace", typeof(double), typeof(VinylTextBlock), new PropertyMetadata(myFontHorizontalSpaceChanged));
            myFontVerticalSpaceProperty = DependencyProperty.Register("MyFontVerticalSpace", typeof(double), typeof(VinylTextBlock), new PropertyMetadata(myFontVerticalSpaceChanged));
            myFontWeightProperty = DependencyProperty.Register("MyFontWeight", typeof(FontWeight), typeof(VinylTextBlock), new PropertyMetadata(myFontWeightChanged));
            myFontFamilyProperty = DependencyProperty.Register("MyFontFamily", typeof(FontFamily), typeof(VinylTextBlock), new PropertyMetadata(myFontFamilyChanged));
            myMaxWidthProperty = DependencyProperty.Register("MyMaxWidth", typeof(double), typeof(VinylTextBlock), new PropertyMetadata(myMaxWidthChanged));
            myMaxHeightProperty = DependencyProperty.Register("MyMaxHeight", typeof(double), typeof(VinylTextBlock), new PropertyMetadata(myMaxHeightChanged));
        }

        private static void myIsWrappingEnabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myIsWrappingEnabled = bool.Parse(e.NewValue.ToString());
            m.InvalidateVisual();
        }

        private static void myIsTrimmingEnabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myIsTrimmingEnabled = bool.Parse(e.NewValue.ToString());
            m.InvalidateVisual();
        }

        private static void myTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myText = e.NewValue as string;
            if (m.myText == string.Empty || m.myText == null)
            {
                m.myText = " ";
            }
            m.InvalidateVisual();
        }


        private static void myFontSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myFontSize = double.Parse(e.NewValue.ToString());
            m.InvalidateVisual();
        }

        private static void myFontColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myFontColor = e.NewValue as Brush;
            m.InvalidateVisual();
        }

        private static void myFontHorizontalSpaceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myFontHorizontalSpace = double.Parse(e.NewValue.ToString());
            m.InvalidateVisual();
        }

        private static void myFontVerticalSpaceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myFontVerticalSpace = double.Parse(e.NewValue.ToString());
            m.InvalidateVisual();
        }

        private static void myFontWeightChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myFontWeight = (FontWeight)e.NewValue;
            m.InvalidateVisual();
        }

        private static void myFontFamilyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myFontFamily = (FontFamily)e.NewValue;
            m.InvalidateVisual();
        }

        private static void myMaxWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myMaxWidth = double.Parse(e.NewValue.ToString());
            m.InvalidateVisual();
        }

        private static void myMaxHeightChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            VinylTextBlock m = o as VinylTextBlock;
            m.myMaxHeight = double.Parse(e.NewValue.ToString());
            m.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (myIsWrappingEnabled)
            {
                List<GlyphRun> glyphRuns = BuildGlyphRuns(myText, this);
                if (glyphRuns != null)
                {
                    foreach (GlyphRun run2 in glyphRuns)
                    {
                        drawingContext.DrawGlyphRun(myFontColor, run2);
                    }
                }
            }
            else
            {
                if (myIsTrimmingEnabled)
                {
                    List<GlyphRun> glyphRuns = BuildGlyphRunsForTrimming(myText, this);
                    if (glyphRuns != null)
                    {
                        drawingContext.DrawGlyphRun(myFontColor, glyphRuns[0]);
                    }
                }
                else
                {
                    GlyphRun run = BuildGlyphRun(myText, this, 0.0, false);
                    if (run != null)
                    {
                        drawingContext.DrawGlyphRun(myFontColor, run);
                    }
                }
            }

        }

        public static GlyphRun BuildGlyphRun(string text, VinylTextBlock o, double yOffset, bool isSubOfList)
        {
            if (text == null || text == "")
            {
                o.Width = 0;
                o.Height = 0;
                return null;
            }
            double fontSize = o.myFontSize;
            GlyphRun glyphRun = null;

            Typeface font = new Typeface(o.myFontFamily, FontStyles.Normal, o.myFontWeight, FontStretches.Normal);
            GlyphTypeface glyphFace;
            if (font.TryGetGlyphTypeface(out glyphFace))
            {
                glyphRun = new GlyphRun();
                ISupportInitialize isi = glyphRun;
                isi.BeginInit();
                glyphRun.GlyphTypeface = glyphFace;
                glyphRun.FontRenderingEmSize = fontSize;

                char[] textChars = text.ToCharArray();
                glyphRun.Characters = textChars;
                ushort[] glyphIndices = new ushort[textChars.Length];
                double[] advanceWidths = new double[textChars.Length];

                for (int i = 0; i < textChars.Length; ++i)
                {
                    int codepoint = textChars[i];
                    ushort glyphIndex = glyphFace.CharacterToGlyphMap[codepoint];
                    double glyphWidth = glyphFace.AdvanceWidths[glyphIndex];

                    glyphIndices[i] = glyphIndex;
                    advanceWidths[i] = (glyphWidth * fontSize) + (o.myFontHorizontalSpace * 0.1);
                }
                try
                {
                    glyphRun.GlyphIndices = glyphIndices;
                    glyphRun.AdvanceWidths = advanceWidths;
                }
                catch
                {
                    glyphRun.GlyphIndices = new ushort[1];
                    glyphRun.AdvanceWidths = new double[1];
                }

                glyphRun.BaselineOrigin = new Point(0, glyphFace.Baseline * fontSize + yOffset);

                isi.EndInit();
            }

            if (!isSubOfList)
            {
                double myWidth = 0;
                foreach (double w in glyphRun.AdvanceWidths)
                {
                    myWidth += w;
                }

                o.Width = myWidth - (o.myFontHorizontalSpace * 0.1);
                o.Height = (glyphFace.CapsHeight * 2) * fontSize;
            }
            return glyphRun;
        }

        public static List<GlyphRun> BuildGlyphRuns(string text, VinylTextBlock o)
        {
            if (text == null || text == "")
            {
                o.Width = 0;
                o.Height = 0;
                return null;
            }
            int charCurrIndex = 0;
            int charPrevIndex = 0;
            int lineIndex = 0;
            double lineHeight = 0;
            double fontSize = o.myFontSize;
            double fontHorizontalSpace = o.myFontHorizontalSpace;
            double fontVerticalSpace = o.myFontVerticalSpace;
            char[] textChars = text.ToCharArray();
            Typeface font = new Typeface(o.myFontFamily, FontStyles.Normal, o.myFontWeight, FontStretches.Normal);
            GlyphTypeface glyphFace;

            double totalWidth = 0;
            double totalHeight = 0;

            List<GlyphRun> glyphRuns = new List<GlyphRun>(2);

            if (font.TryGetGlyphTypeface(out glyphFace))
            {
                lineHeight = (glyphFace.Height * fontSize + fontVerticalSpace);
                int numAvailableLine = (int)((o.myMaxHeight + fontVerticalSpace) / lineHeight);



                for (lineIndex = 0; lineIndex < numAvailableLine && charCurrIndex < text.Length; lineIndex++)
                {
                    // 루프 한 번 마다text line 하나를 생성
                    double textWidth = 0;
                    double advanceWidth = 0;


                    while (charCurrIndex < text.Length)
                    {
                        ushort glyphIndex = glyphFace.CharacterToGlyphMap[textChars[charCurrIndex]];
                        double glyphWidth = glyphFace.AdvanceWidths[glyphIndex];

                        advanceWidth = (glyphWidth * fontSize) + (fontHorizontalSpace * 0.1);

                        if (textWidth + advanceWidth < o.myMaxWidth)
                        {
                            textWidth += advanceWidth;
                            charCurrIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }



                    if (totalWidth < textWidth)
                    {
                        totalWidth = textWidth;
                    }

                    string newTextLine = text.Substring(charPrevIndex, charCurrIndex - charPrevIndex);
                    newTextLine = newTextLine.TrimStart(' ');

                    double yOffset = lineHeight * lineIndex;

                    GlyphRun newTextLineglyphRun = null;
                    if (newTextLine != null && newTextLine != "")
                    {
                        newTextLineglyphRun = BuildGlyphRun(newTextLine, o, yOffset, true);
                    }


                    glyphRuns.Add(newTextLineglyphRun);
                    charPrevIndex = charCurrIndex;
                }
            }

            totalHeight = lineHeight * lineIndex;


            totalWidth -= fontHorizontalSpace * 0.1;
            totalHeight -= fontVerticalSpace;


            o.Width = totalWidth;
            o.Height = totalHeight;

            return glyphRuns;
        }

        public static List<GlyphRun> BuildGlyphRunsForTrimming(string text, VinylTextBlock o)
        {
            if (text == null || text == "")
            {
                o.Width = 0;
                o.Height = 0;
                return null;
            }
            int charCurrIndex = 0;
            int charPrevIndex = 0;
            int lineIndex = 0;
            double lineHeight = 0;
            double fontSize = o.myFontSize;
            double fontHorizontalSpace = o.myFontHorizontalSpace;
            double fontVerticalSpace = o.myFontVerticalSpace;
            char[] textChars = text.ToCharArray();
            Typeface font = new Typeface(o.myFontFamily, FontStyles.Normal, o.myFontWeight, FontStretches.Normal);
            GlyphTypeface glyphFace;

            double totalWidth = 0;
            double totalHeight = 0;

            List<GlyphRun> glyphRuns = new List<GlyphRun>(2);

            if (font.TryGetGlyphTypeface(out glyphFace))
            {
                lineHeight = (glyphFace.Height * fontSize + fontVerticalSpace);
                int numAvailableLine = (int)((o.myMaxHeight + fontVerticalSpace) / lineHeight);



                for (lineIndex = 0; lineIndex < numAvailableLine && charCurrIndex < text.Length; )
                {
                    // 루프 한 번 마다text line 하나를 생성
                    double textWidth = 0;
                    double advanceWidth = 0;


                    while (charCurrIndex < text.Length)
                    {
                        ushort glyphIndex = glyphFace.CharacterToGlyphMap[textChars[charCurrIndex]];
                        double glyphWidth = glyphFace.AdvanceWidths[glyphIndex];

                        advanceWidth = (glyphWidth * fontSize) + (fontHorizontalSpace * 0.1);

                        if (textWidth + advanceWidth < o.myMaxWidth)
                        {
                            textWidth += advanceWidth;
                            charCurrIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }



                    if (totalWidth < textWidth)
                    {
                        totalWidth = textWidth;
                    }

                    string newTextLine = text.Substring(charPrevIndex, charCurrIndex - charPrevIndex);
                    newTextLine = newTextLine.TrimStart(' ');
                    if (charCurrIndex <= text.Length - 1)
                    {
                        newTextLine = newTextLine.Remove(newTextLine.Length - 1, 1) + "...";
                    }

                    double yOffset = lineHeight * lineIndex;

                    GlyphRun newTextLineglyphRun = null;
                    if (newTextLine != null && newTextLine != "")
                    {
                        newTextLineglyphRun = BuildGlyphRun(newTextLine, o, yOffset, true);
                    }


                    glyphRuns.Add(newTextLineglyphRun);
                    charPrevIndex = charCurrIndex;

                    break;
                }
            }

            totalHeight = lineHeight * lineIndex;


            totalWidth -= fontHorizontalSpace * 0.1;
            totalHeight -= fontVerticalSpace;


            o.Width = totalWidth;
            o.Height = totalHeight;

            return glyphRuns;
        }
    }

}

