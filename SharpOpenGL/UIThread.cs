using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                testWindow = new ObjectEditor.MainWindow();
                testWindow.Show();
            }));
        }

        private ObjectEditor.MainWindow testWindow = null;
    }
}
