using Core;
using Core.CustomEvent;
using Core.MaterialBase;

using Core.Texture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.PostProcess
{
    public class BlurPostProcess : SharpOpenGL.PostProcess.PostProcessBase, IResizable
    {
        public BlurPostProcess()
            : base()
        {
            ResizableManager.Get().AddResizable(this);

        }

        public void OnResize(object sender, ScreenResizeEventArgs e)
        {
            UpdateOffsetAndWeight(e.Width, e.Height);
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            PostProcessMaterial = ShaderManager.Get().GetMaterial<Blur.Blur>();
        }

        public override void Render(TextureBase input)
        {
            Output.BindAndExecute(PostProcessMaterial, () =>
            {
                PostProcessMaterial.SetUniformVector2ArrayData("BlurOffsets", ref offset);
                PostProcessMaterial.SetUniformVector2ArrayData("BlurWeights", ref weight);

                PostProcessMaterial.SetTexture("ColorTex", input);
                BlitToScreenSpace();
            });
        }

        protected float[] offset = null;
        protected float[] weight = null;

        protected void UpdateOffsetAndWeight(float Width, float Height)
        {
            var vOffset = new List<OpenTK.Vector2>();
            var vWeight = new List<OpenTK.Vector2>();

            for (int i = 0; i < 9; i++)
            {
                // Compute the offsets. We take 9 samples - 4 either side and one in the middle:
                //     i =  0,  1,  2,  3, 4,  5,  6,  7,  8
                //Offset = -4, -3, -2, -1, 0, +1, +2, +3, +4
                float fOffsetX = ((float)(i) - 4.0f) * (1.0f / Width);
                float fOffsetY = ((float)(i) - 4.0f) * (1.0f / Height);

                vOffset.Add(new OpenTK.Vector2(fOffsetX, fOffsetY));

                // 'x' is just a simple alias to map the [0,8] range down to a [-1,+1]
                float x = ((float)(i) - 4.0f) / 4.0f;

                // Use a gaussian distribution. Changing the standard-deviation
                // (second parameter) as well as the amplitude (multiplier) gives
                // distinctly different results.

                double fWeight = 0.5 * MathHelper.ComputeGaussianValue(x, 0.0f, 0.8f);
                Debug.Assert(fWeight > 0.0f);
                vWeight.Add(new OpenTK.Vector2((float)fWeight, 0));
            }

            offset = vOffset.Flatten();
            weight = vWeight.Flatten();
        }
    }
}
