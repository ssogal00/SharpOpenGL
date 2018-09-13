using System;
using OpenTK.Graphics.OpenGL;
using Core.Texture;

namespace Core
{
    public class OpenGLContext
    {
        protected static OpenGLContext Singleton = new OpenGLContext(null);

        public static OpenGLContext Get() { return Singleton; }

        public void SetGameWindow(OpenTK.GameWindow window)
        {
            this.window = window;
        }

        public TextureBase CreateTexture()
        {
            return null;
        }

        public bool IsValid { get { return this.window != null; } }

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
