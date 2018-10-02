
using ZeroFormatter;
using OpenTK;
using Core.MaterialBase;

namespace SharpOpenGL.Scene
{
    [ZeroFormattable]
    public abstract class SceneObject
    {
        [Index(0)]
        public virtual Vector3 Location { get; set; } = new Vector3(0, 0, 0);

        [Index(1)]
        public virtual float Scale { get; set; } = 1.0f;

        public OpenTK.Matrix4 ModelMatrix => OpenTK.Matrix4.CreateScale(Scale) * OpenTK.Matrix4.CreateTranslation(Location);

        public virtual void Draw() { }

        public virtual void Draw(MaterialBase) { }
    }
}
