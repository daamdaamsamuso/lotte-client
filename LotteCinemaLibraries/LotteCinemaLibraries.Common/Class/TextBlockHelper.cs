using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;

namespace LotteCinemaLibraries.Common.Class
{
    public class TextBlockHelper
    {
        public static void SetEditText(TextBlock tb, List<string> strList)
        {
            if (strList == null || strList.Count == 0) return;

            Dictionary<TextRange, string> dicTextRange = new Dictionary<TextRange, string>();

            foreach (var str in strList)
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    // CharacterColor : 0-4:#FFFF0000|12:#FFE4B5FF
                    // CharacterBold : 0-4:Bold|7:Bold|12-13:Bold
                    var charSplit = str.Split('|');

                    if (charSplit.Length > 0)
                    {
                        foreach (var ch in charSplit)
                        {
                            var split = ch.Split(':');

                            if (split.Length == 2)
                            {
                                var param = split[1];

                                int startIndex = 0;
                                int endIndex = 0;

                                if (split[0].Contains("-"))
                                {
                                    var strIndex = split[0].Split('-');
                                    startIndex = int.Parse(strIndex[0]);
                                    endIndex = int.Parse(strIndex[1]);
                                }
                                else
                                {
                                    startIndex = int.Parse(split[0]);
                                    endIndex = int.Parse(split[0]);
                                }

                                endIndex += 1;

                                var sp = tb.ContentStart.GetPositionAtOffset(startIndex);
                                var ep = tb.ContentStart.GetPositionAtOffset(endIndex);

                                if (sp != null && ep != null)
                                {
                                    var tr = new TextRange(sp, ep);
                                    dicTextRange.Add(tr, param);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var tr in dicTextRange)
            {  
                if (tr.Value.StartsWith("#"))
                {
                    var color = (Color)ColorConverter.ConvertFromString(tr.Value);
                    var brush = new SolidColorBrush(color);
                    tr.Key.ApplyPropertyValue(TextBlock.ForegroundProperty, brush);
                }
                else if (tr.Value == "Bold")
                {
                    tr.Key.ApplyPropertyValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                }
            }
        }
    }
}