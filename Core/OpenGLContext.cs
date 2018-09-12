using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class OpenGLContext
    {
        public OpenGLContext(OpenTK.GameWindow window)
        {
            this.window = window;
        }
        
        private OpenTK.GameWindow window = null;

        public static EventHandler<EventArgs> OpenGLContextCreated;

        public void MakeCurrent()
        {
            window.MakeCurrent();
        }

        public int GetActiveTexture()
        {
            return GL.GetInteger(GetPName.ActiveTexture);
        }

        public int GetMaxElementsIndices()
        {
            return GL.GetInteger(GetPName.MaxElementsIndices);
        }

        public int GetMaxElementsVertices()
        {
            return GL.GetInteger(GetPName.MaxElementsVertices);
        }

        public bool IsDepthTestEnabled()
        {            
            return GL.GetBoolean(GetPName.DepthTest);
        }

        public int GetMaxFragmentUniformBlocks()
        {            
            return GL.GetInteger(GetPName.MaxFragmentUniformBlocks);
        }

        public int GetMaxTextureUnits()
        {            
            return GL.GetInteger(GetPName.MaxTextureUnits);
        }

        
    }
}
