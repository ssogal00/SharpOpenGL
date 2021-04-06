using Core.Primitive;
using OpenTK;
using OpenTK.Mathematics;

namespace Engine
{
    public class Arrow : GameObject
    {
        // @ ISceneobject interface

        public override Matrix4 LocalMatrix
        {
            get
            {
                // TRS Matrix
                // from right to left
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
        }

        protected override void PrepareRenderingData()
        {
        }

        // @ ISceneobject interface

        public Vector3 Color= new Vector3(1,0,0);

        public Arrow(float arrowLength)
        {
            ArrowHeadTranslation = new Vector3(arrowLength, 0, 0);
        }

        public Arrow(float arrowLength, Vector3 color)
        {
            ArrowHeadTranslation=new Vector3(arrowLength, 0, 0);
            ArrowBody.Color = color;
            ArrowHead.Color = color;
        }
        
        public override void Render()
        {
            if (ArrowBody.IsReadyToDraw && ArrowHead.IsReadyToDraw)
            {
                ArrowBody.ParentMatrix = ParentMatrix;
                ArrowBody.Render();

                //
                ArrowHead.ParentMatrix = ParentMatrix;
                ArrowHead.Translation = ArrowHeadTranslation;
                ArrowHead.Render();
            }
        }

        protected Cylinder ArrowBody = new Cylinder(1,10,10);
        protected Cone ArrowHead = new Cone(2,2,10);
        protected float Length = 10.0f;

        protected Vector3 ArrowHeadTranslation ;
    }
}
