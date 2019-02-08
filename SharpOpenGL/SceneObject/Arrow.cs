using Core.Primitive;
using OpenTK;


namespace SharpOpenGL
{
    public class Arrow : SceneObject
    {
        // @ ISceneobject interface

        public override OpenTK.Matrix4 LocalMatrix
        {
            get
            {
                // TRS Matrix
                // from right to left
                return Matrix4.CreateScale(Scale) * Matrix4.CreateRotationY(Yaw) * Matrix4.CreateRotationX(Pitch) * Matrix4.CreateTranslation(Translation);
            }
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
        
        public override void Draw()
        {
            if (ArrowBody.IsReadyToDraw && ArrowHead.IsReadyToDraw)
            {
                ArrowBody.ParentMatrix = ParentMatrix;
                ArrowBody.Draw();

                //
                ArrowHead.ParentMatrix = ParentMatrix;
                ArrowHead.Translation = ArrowHeadTranslation;
                ArrowHead.Draw();
            }
        }

        protected Cylinder ArrowBody = new Cylinder(1,10,10);
        protected Cone ArrowHead = new Cone(2,2,10);
        protected float Length = 10.0f;

        protected Vector3 ArrowHeadTranslation ;
    }
}
