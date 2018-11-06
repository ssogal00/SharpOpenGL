
using ZeroFormatter;
using OpenTK;
using Core.MaterialBase;

namespace Core.Primitive
{
    public interface ISceneObject
    {
        Vector3 Location { get; set; }

        float Scale { get; set; }

        float Yaw { get; set; }

        float Pitch { get; set; }

        float Roll { get; set; }

        OpenTK.Matrix4 ModelMatrix
        {
            get;
        }

        void Draw(MaterialBase.MaterialBase material);
    }
}
