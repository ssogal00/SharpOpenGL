using Core;
using System;


namespace Engine
{
    class UIThread : Singleton<UIThread>
    {
        public UIThread()
        {

        }

        public void Enqueue(Action action)
        {
            /*if (editorWindow != null && editorWindow.Dispatcher != null)
            {
                if (editorWindow.Dispatcher.HasShutdownFinished == false)
                {
                    editorWindow.Dispatcher.InvokeAsync(action);
                }
            }*/
        }

        public void Run()
        {   
            /*editorWindow = new ObjectEditor.MainWindow();

            editorWindow.ObjectCreateEventHandler += Engine.Instance.OnObjectCreate;

            editorWindow.Show();

            editorWindow.Closed += (sender, args) =>
            {
                editorWindow.Dispatcher.InvokeShutdown();
            };

            */

            System.Windows.Threading.Dispatcher.Run();
            Console.WriteLine("Here");
        }

        public void RequestExit()
        {
            /*if (!editorWindow.Dispatcher.HasShutdownFinished && !editorWindow.Dispatcher.HasShutdownStarted)
            {
                editorWindow.Dispatcher.Invoke(
                () =>
                {
                    editorWindow.Close();
                });
            }*/
        }

        //public static ObjectEditor.MainWindow EditorWindow => editorWindow;
        //private static ObjectEditor.MainWindow editorWindow = null;
    }
}
