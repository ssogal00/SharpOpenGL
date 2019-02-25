using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpOpenGL.Light
{
    public class PointLight : LightBase
    {
        private static int PointLightCount = 0;
        public PointLight()
        : base("PointLight", PointLightCount++)
        {
        }

        public override void Draw()
        {

        }

        
    }
}
