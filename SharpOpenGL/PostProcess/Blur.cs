using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Texture;
using Core.MathHelper;
using System.Diagnostics;
using Core.CustomEvent;

namespace SharpOpenGL.PostProcess
{
    public class Blur : Core.PostProcess
    {
        public Blur(Core.MaterialBase.MaterialBase material, RenderTargetTexture input)
            : base(material)
        {
            Input = input;
            UpdateOffsetAndWeight(Input.Width, Input.Height);
        }

        public void OnResize(object sender, ScreenResizeEventArgs e)
        {
            UpdateOffsetAndWeight(e.Width, e.Height);
        }
        public override void Render()
        {
            PostProcessMaterial.Setup();
            PostProcessMaterial.SetTexture("ColorTex", Input);
            PostProcessMaterial.SetUniformVector2ArrayData("BlurOffsets", ref m_Offset);
            PostProcessMaterial.SetUniformVector2ArrayData("BlurWeights", ref m_Weight);
        }

        protected RenderTargetTexture Input = null;

        protected float[] m_Offset = null;
        protected float[] m_Weight = null;

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

            m_Offset = vOffset.Flatten();
            m_Weight = vWeight.Flatten();
        }
    }
}
