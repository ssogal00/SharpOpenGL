
using ZeroFormatter;
using OpenTK;
using Core.MaterialBase;

namespace SharpOpenGL.Scene
{
    
    public interface ISceneObject
    {
        Vector3 Location { get; set; }
        
        float Scale { get; set; } 

        OpenTK.Matrix4 ModelMatrix
        {
            get;
        }

        void Draw();

        void Draw(MaterialBase material);
    }
}
