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
        public Vector3 Translation { get; set; } = new Vector3(0, 0, 0);

        public float Scale { get; set; } = 1.0f;

        public OpenTK.Matrix4 ParentMatrix { get; set; } = Matrix4.Identity;

        public OpenTK.Matrix4 ModelMatrix
        {
            get
            {
                // TRS Matrix
                // from right to left
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;
        public float Roll { get; set; } = 0;

        public void Draw() { }
        // @ ISceneobject interface

        public Vector3 Color= new Vector3(1,0,0);

        public Arrow(float arrowLength)
        {
            ArrowHeadTranslation = new Vector3(10, 0, 0);
        }
        
        public void Draw(MaterialBase.MaterialBase material)
        {
            //
            ArrowBody.ParentMatrix = ParentMatrix;
            ArrowBody.Draw(material);

            //
            ArrowHead.ParentMatrix = ParentMatrix;
            ArrowHead.Translation = ArrowHeadTranslation;
            ArrowHead.Draw(material);
        }

        protected Cylinder ArrowBody = new Cylinder(1,10,10);
        protected Cone ArrowHead = new Cone(2,2,10);
        protected float Length = 10.0f;

        protected Vector3 ArrowHeadTranslation ;
    }
}
