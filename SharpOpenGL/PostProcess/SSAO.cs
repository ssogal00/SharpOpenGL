using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using OpenTK;
using MathHelper = Core.MathHelper;

namespace SharpOpenGL.PostProcess
{
    public class SSAO : PostProcessBase
    {
        public SSAO()
        : base()
        {
        }

        public override void OnGLContextCreated(object sender, EventArgs e)
        {
            base.OnGLContextCreated(sender, e);

            BuildKernel();
        }

        private void BuildRandomRotationTexture()
        {

            int size = 4;

            List<float> randomDirections = new List<float>(3 * size * size);

            for (int i = 0; i < size * size; ++i)
            {
                var v = MathHelper.UniformHemisphere();
                randomDirections[i * 3] = v.X;
                randomDirections[i * 3 + 1] = v.Y;
                randomDirections[i * 3 + 2] = v.Z;
                
            }
        }

        private void BuildKernel()
        {
            KernelList.Clear();
            
            int KernelSize = 64;

            for (int i = 0; i < KernelSize; ++i)
            {
                Vector3 randDir = Core.MathHelper.UniformHemisphere();

                float scale = (float) (i * i) / (float) (KernelSize * KernelSize);
                randDir *= scale;
                KernelList.Add(randDir);
            }
        }


        private List<Vector3> KernelList = new List<Vector3>();
    }
}
