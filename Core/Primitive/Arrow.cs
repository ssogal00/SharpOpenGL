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
    public class Arrow : RenderResource
    {
        public Arrow(float arrowLength)
        {
            Translation = Matrix4.CreateTranslation(20, 0, 0);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Draw(MaterialBase.MaterialBase material)
        {
            ArrowBody.Draw(material);
            material.SetUniformVarData("Model", ref Translation);
            ArrowHead.Draw(material);
        }

        

        protected Cylinder ArrowBody = new Cylinder(4,20,10);
        protected Cone ArrowHead = new Cone(6,5,10);
        protected float Length = 10.0f;

        protected Matrix4 Translation ;
    }
}
