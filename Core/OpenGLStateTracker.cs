using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class OpenGLStateTracker : Singleton<OpenGLStateTracker>
    {
        private int viewportSizeX;  
        private int viewportSizeY;

        public void ChangeViewportSize(int newViewportSizeX, int newViewportSizeY)
        {
            if (viewportSizeX != newViewportSizeX)
            {
                viewportSizeX = newViewportSizeX;
            }

            if (viewportSizeY != newViewportSizeY)
            {
                viewportSizeY = newViewportSizeY;
            }
        }
    }
}
