
using ZeroFormatter;
using OpenTK;
using Core.MaterialBase;

namespace Core.Primitive
{
    public interface ISceneObject
    {
        Vector3 Translation { get; set; }

        float Scale { get; set; }

        float Yaw { get; set; }

        float Pitch { get; set; }

        float Roll { get; set; }

        OpenTK.Matrix4 ParentMatrix
        {
            get;
            set;
        }

        OpenTK.Matrix4 LocalMatrix
        {
            get;
        }

        void Draw(MaterialBase.MaterialBase material);

    }
}
