using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Core.Texture
{
    public class Texture2DMultiSample : TextureBase
    {
        public Texture2DMultiSample()
        {
            GL.CreateTextures(TextureTarget.Texture2DMultisample, 1, out textureObject);
        }
    }
}
