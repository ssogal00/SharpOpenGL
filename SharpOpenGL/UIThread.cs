using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void Enqueue(Action action)
        {
            if (editorWindow != null && editorWindow.Dispatcher != null)
            {
                editorWindow.Dispatcher.InvokeAsync(action);
            }
        }

        public void Run()
        {   
            editorWindow = new ObjectEditor.MainWindow();

            editorWindow.ObjectCreateEventHandler += Engine.Get().OnObjectCreate;

            editorWindow.Show();

            editorWindow.Closed += (sender, args) =>
            {
                editorWindow.Dispatcher.InvokeShutdown();
            };

            System.Windows.Threading.Dispatcher.Run();

            Console.WriteLine("Here");
        }

        public void RequestExit()
        {
            bRequestExist = true;
            editorWindow.Dispatcher.Invoke(() => { editorWindow.Close();});
        }

        private bool bRequestExist = false;

        public ObjectEditor.MainWindow EditorWindow => editorWindow;

        private ObjectEditor.MainWindow editorWindow = null;
    }
}
