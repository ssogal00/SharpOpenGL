using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace SharpOpenGL.Light
{

    public class SpotLight : LightBase
    {
        private static int SpotLightCount = 0;
        public override void Draw()
        {

        }

        public SpotLight()
            : base("SpotLight", SpotLightCount++)
        { }

        public float Penumbra { get; set; } = OpenTK.MathHelper.PiOver6;
        public float Umbra { get; set; } = OpenTK.MathHelper.PiOver3;
    }

}
