using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Engine.Light
{

    public class SpotLight : LightBase
    {
        private static int SpotLightCount = 0;
        public override void Render()
        {

        }

        protected override void PrepareRenderingData()
        {
        }

        public SpotLight()
            : base("SpotLight", SpotLightCount++)
        { }

        public float Penumbra { get; set; } = OpenTK.Mathematics.MathHelper.PiOver6;
        public float Umbra { get; set; } = OpenTK.Mathematics.MathHelper.PiOver3;
    }

}
