
using Core.MaterialBase;
using Core.Primitive;
using OpenTK;

namespace SharpOpenGL.Scene
{
    public class StaticMeshObject : ISceneObject
    {
        public Vector3 Translation { get; set; }

        public float Scale { get; set; }

        public OpenTK.Matrix4 ParentMatrix { get; set; } = Matrix4.Identity;

        public Matrix4 LocalMatrix
        {
            get
            {
                return Matrix4.CreateScale(Scale) * Matrix4.CreateTranslation(Translation);
            }
        }

        public float Yaw { get; set; } = 0;
        public float Pitch { get; set; } = 0;
        public float Roll { get; set; } = 0;

        public void Draw()
        {

        }

        public void Draw(MaterialBase material)
        {
        }
        
    }
}
