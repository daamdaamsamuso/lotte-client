using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ShaderEffectLibrary
{
    public class ColorMulEffect : ShaderEffect
    {
        #region Variable

        private static PixelShader _pixelShader = new PixelShader();

        #endregion

        #region Property

        #region Input

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorMulEffect), 0);

        public Brush Input
        {
            get { return (Brush)this.GetValue(InputProperty); }
            set { this.SetValue(InputProperty, value); }
        }

        #endregion

        #region MapTex

        public static readonly DependencyProperty MapTexProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("MapTex", typeof(ColorMulEffect), 1);

        public Brush MapTex
        {
            get { return (Brush)this.GetValue(MapTexProperty); }
            set { this.SetValue(MapTexProperty, value); }
        }

        #endregion

        #endregion

        #region Constructor

        static ColorMulEffect()
        {
            _pixelShader.UriSource = Global.MakePackUri("Shader/ColorMulEffect.ps");
        }

        public ColorMulEffect()
        {
            this.PixelShader = _pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(MapTexProperty);
        }

        #endregion
    }
}