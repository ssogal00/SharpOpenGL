
using Core.CustomAttribute;
using ZeroFormatter;
using OpenTK;
using Core.MaterialBase;

namespace Core.Primitive
{
    public interface ISceneObject
    {
        [ExposeUI("Translation")]
        Vector3 Translation { get; set; }

        [ExposeUI]
        float Scale { get; set; }

        [ExposeUI, UIGroup("Rotation")]
        float Yaw { get; set; }

        [ExposeUI, UIGroup("Rotation")]
        float Pitch { get; set; }

        [ExposeUI, UIGroup("Rotation")]
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
