using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Core
{
    public class OpenGLContext
    {
        public OpenGLContext(OpenTK.GameWindow window)
        {
            this.window = window;
        }        

        private OpenTK.GameWindow window = null;

        public void MakeCurrent()
        {
            window.MakeCurrent();
        }


    }
}
