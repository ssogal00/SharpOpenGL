using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpOpenGL
{
    public class ThreadJob
    {
        public ThreadJob()
        {
        }

        public virtual void Do()
        { }
    }

    public class ActionJob : ThreadJob
    {
        public ActionJob(Action action)
        {
            this.action = action;
        }

        public override void Do()
        {
            if (this.action != null)
            {
                this.action();
            }
        }

        private Action action = null;
    }



    public class TextureLoadJob : ThreadJob
    {
        public TextureLoadJob(string path)
        {
            texturePath = path;
        }
        public override void Do()
        {

        }

        private string texturePath = "";
    }
}
