using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.MaterialBase;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace Core.Primitive
{
    public class Arrow : RenderResource, ISceneObject
    {
        // @ ISceneobject interface
        public Vector3 Location { get; set; } = new Vector3(0, 0, 0);

        public float Scale { get; set; } = 1.0f;

        public OpenTK.Matrix4 ModelMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Location);
            }
        }

        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;
        public float Roll { get; set; } = 0;

        public void Draw() { }
        // @ ISceneobject interface

        public Arrow(float arrowLength)
        {
            Translation = Matrix4.CreateTranslation(20, 0, 0);
        }
        
        public void Draw(MaterialBase.MaterialBase material)
        {
            ArrowBody.Draw(material);
            //
            ArrowHead.Draw(material);
        }

        protected Cylinder ArrowBody = new Cylinder(4,20,10);
        protected Cone ArrowHead = new Cone(6,5,10);
        protected float Length = 10.0f;

        protected Matrix4 Translation ;
    }
}
