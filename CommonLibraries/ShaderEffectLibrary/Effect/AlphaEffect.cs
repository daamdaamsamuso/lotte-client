using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ShaderEffectLibrary
{
    public class AlphaEffect : ShaderEffect
    {
        #region Variable

        private static PixelShader _pixelShader = new PixelShader();

        #endregion

        #region Property

        #region Input

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(AlphaEffect), 0);

        public Brush Input
        {
            get { return (Brush)this.GetValue(InputProperty); }
            set { this.SetValue(InputProperty, value); }
        }

        public Brush Input2
        {
            get { return (Brush)GetValue(Input2Property); }
            set { SetValue(Input2Property, value); }
        }
        public static readonly DependencyProperty Input2Property =
          ShaderEffect.RegisterPixelShaderSamplerProperty("Input2", typeof(AlphaEffect), 1);

        #endregion

        #region Threshold

        public static readonly DependencyProperty ThresholdProperty =
            DependencyProperty.Register("Threshold", typeof(double), typeof(AlphaEffect), new UIPropertyMetadata(0.1d, PixelShaderConstantCallback(0)));

        public double Threshold
        {
            get { return (double)this.GetValue(ThresholdProperty); }
            set { this.SetValue(ThresholdProperty, value); }
        }

        #endregion

        #endregion

        #region Constructor

        static AlphaEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Shader/AlphaEffect.ps");
        }

        public AlphaEffect()
        {
            this.PixelShader = _pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ThresholdProperty);
        }

        #endregion
    }
}