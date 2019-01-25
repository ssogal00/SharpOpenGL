
using Core.MaterialBase;
using Core.Primitive;
using OpenTK;

namespace SharpOpenGL
{
    public class StaticMeshObject : SceneObject
    {
        public override Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateTranslation(Translation);
            }
        }

        public void Draw()
        {

        }

        public override void Draw(MaterialBase material)
        {
        }
    }
}
