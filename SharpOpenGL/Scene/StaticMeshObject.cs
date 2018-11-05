
using Core.MaterialBase;
using Core.Primitive;
using OpenTK;

namespace SharpOpenGL.Scene
{
    public class StaticMeshObject : ISceneObject
    {
        public Vector3 Location { get; set; }

        public float Scale { get; set; }

        public Matrix4 ModelMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateTranslation(Location);
            }
        }

        public void Draw()
        {

        }

        public void Draw(MaterialBase material)
        {
        }
        
    }
}
