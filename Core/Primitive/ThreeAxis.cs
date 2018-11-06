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
        public Vector3 Translation { get; set; } = new Vector3(0, 0, 0);

        public float Scale { get; set; } = 1.0f;

        public OpenTK.Matrix4 ParentMatrix { get; set; } = Matrix4.Identity;

        public OpenTK.Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
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
            xAxis.ParentMatrix = LocalMatrix * ParentMatrix;
            xAxis.Draw(material);

            //
            yAxis.ParentMatrix = Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(90))* LocalMatrix * ParentMatrix;
            yAxis.Draw(material);
            //

            zAxis.ParentMatrix = Matrix4.CreateRotationY(OpenTK.MathHelper.DegreesToRadians(-90)) *LocalMatrix* ParentMatrix;
            zAxis.Draw(material);
        }
        // @ ISceneobject interface

        public ThreeAxis()
        {
        }


        protected Arrow xAxis = new Arrow(10, new Vector3(1,0,0));
        protected Arrow yAxis = new Arrow(10, new Vector3(0, 1, 0));
        protected Arrow zAxis = new Arrow(10, new Vector3(0, 0, 1));
    }
}
