using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Random;

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
