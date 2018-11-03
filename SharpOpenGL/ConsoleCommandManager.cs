using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using OpenTK;
using OpenTK.Input;

namespace SharpOpenGL
{
    internal class ConsoleCommandManager : Singleton<ConsoleCommandManager>
    {
        public void ToggleActive()
        {
            IsActive = !IsActive;
        }

        public void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.BackSpace)
            {

            }
            else if (e.Key == Key.Enter)
            {

            }
            else if (e.Key == Key.CapsLock)
            {

            }
            else if (e.Key == Key.Space)
            {

            }
        }


        public string ConsoleCommandString = "";

        public bool IsActive = false;

        protected bool CapsLock = false;


    }
}
