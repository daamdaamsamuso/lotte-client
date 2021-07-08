using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ShaderEffectLibrary
{
    public class ColorKeyEffect : ShaderEffect
    {
        #region Variable

        private static PixelShader _pixelShader = new PixelShader();

        #endregion

        #region Property

        #region Input

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorKeyEffect), 0);

        public Brush Input
        {
            get { return (Brush)this.GetValue(InputProperty); }
            set { this.SetValue(InputProperty, value); }
        }

        #endregion

        #region ColorKey

        public static readonly DependencyProperty ColorKeyProperty =
            DependencyProperty.Register("ColorKey", typeof(Color), typeof(ColorKeyEffect), new UIPropertyMetadata(Colors.Black, PixelShaderConstantCallback(0)));

        public Color ColorKey
        {
            get { return (Color)this.GetValue(ColorKeyProperty); }
            set { this.SetValue(ColorKeyProperty, value); }
        }

        #endregion

        #region Threshold

        public static readonly DependencyProperty ThresholdProperty =
            DependencyProperty.Register("Threshold", typeof(double), typeof(ColorKeyEffect), new UIPropertyMetadata(0.8d, PixelShaderConstantCallback(1)));

        public double Threshold
        {
            get { return (double)this.GetValue(ThresholdProperty); }
            set { this.SetValue(ThresholdProperty, value); }
        }

        #endregion

        #endregion

        #region Constructor

        static ColorKeyEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Shader/ColorKeyEffect.ps");
        }

        public ColorKeyEffect()
        {
            this.PixelShader = _pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ColorKeyProperty);
            UpdateShaderValue(ThresholdProperty);
        }

        #endregion
    }
}