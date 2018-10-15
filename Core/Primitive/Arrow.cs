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

            Translation = Matrix4.CreateTranslation(arrowLength, 0, 0);
        }

        public override void Initialize()
        {
            base.Initialize();
            ArrowBody = new Cylinder(10, 20, 10);
            ArrowHead = new Cone(20, 5, 10);
        }

        public void Draw(MaterialBase.MaterialBase material)
        {
            ArrowBody.Draw(material);
            ArrowHead.Draw(material);
        }

        protected Cylinder ArrowBody = null;
        protected Cone ArrowHead = null;
        protected float Length = 10.0f;

        protected Matrix4 Translation ;
    }
}
