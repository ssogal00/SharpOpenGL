
using ZeroFormatter;
using OpenTK;
using Core.MaterialBase;

namespace SharpOpenGL.Scene
{
    
    public abstract class SceneObject
    {
        
        public Vector3 Location { get; set; } = new Vector3(0, 0, 0);
        
        public float Scale { get; set; } = 1.0f;
        
        public OpenTK.Matrix4 ModelMatrix => OpenTK.Matrix4.CreateScale(Scale) * OpenTK.Matrix4.CreateTranslation(Location);

        public virtual void Draw() { }

        public virtual void Draw(MaterialBase material) { }
    }
}
