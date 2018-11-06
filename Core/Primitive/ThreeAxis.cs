using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Core.Primitive
{
    public class ThreeAxis : RenderResource , ISceneObject
    {
        // @ ISceneobject interface

        public float Scale { get; set; } = 1.0f;

        public OpenTK.Matrix4 ParentMatrix { get; set; } = Matrix4.Identity;

        public OpenTK.Matrix4 ModelMatrix
        {
            get
            {
            }
        }

        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;
        public float Roll { get; set; } = 0;

        public void Draw()
        {
        }

        public void Draw(MaterialBase.MaterialBase material)
        {
            // 
            xAxis.Draw(material);

            //
            yAxis.Yaw = OpenTK.MathHelper.DegreesToRadians(90);
            yAxis.Draw(material);
            //
            
        }
        // @ ISceneobject interface

        public ThreeAxis()
        {
        }


        protected Arrow xAxis = new Arrow(10);
        protected Arrow yAxis = new Arrow(10);
        protected Arrow zAxis = new Arrow(10);
    }
}
