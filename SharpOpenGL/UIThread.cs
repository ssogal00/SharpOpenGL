using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Core;

namespace SharpOpenGL
{
    class UIThread : Singleton<UIThread>
    {
        public UIThread()
        {
        }


        public void Run()
        {   
            testWindow = new ObjectEditor.MainWindow();
            testWindow.Show();

            testWindow.Closed += (sender, args) =>
            {
                testWindow.Dispatcher.InvokeShutdown();
            };

            System.Windows.Threading.Dispatcher.Run();
        }

        public void RequestExit()
        {
            bRequestExist = true;
            testWindow.Dispatcher.Invoke(() => { testWindow.Close();});
        }

        private bool bRequestExist = false;

        private ObjectEditor.MainWindow testWindow = null;
    }
}
