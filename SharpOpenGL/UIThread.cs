using Core;
using System;
using System.Runtime.Remoting.Channels;
using MaterialEditor;

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
                if (editorWindow.Dispatcher.HasShutdownFinished == false)
                {
                    editorWindow.Dispatcher.InvokeAsync(action);
                }
            }
        }

        public void Run()
        {   
            editorWindow = new ObjectEditor.MainWindow();

            materialEditorWindow = new MaterialEditor.MainWindow();

            editorWindow.ObjectCreateEventHandler += Engine.Get().OnObjectCreate;

            editorWindow.Show();
            materialEditorWindow.Show();

            editorWindow.Closed += (sender, args) =>
            {
                editorWindow.Dispatcher.InvokeShutdown();
            };
            materialEditorWindow.Closed += (sender, args) =>
            {
                materialEditorWindow.Dispatcher.InvokeShutdown();
            };

            System.Windows.Threading.Dispatcher.Run();

            Console.WriteLine("Here");
        }

        public void RequestExit()
        {
            if (!editorWindow.Dispatcher.HasShutdownFinished && !editorWindow.Dispatcher.HasShutdownStarted)
            {
                editorWindow.Dispatcher.Invoke(
                () =>
                {
                    editorWindow.Close();
                });
            }

            if (!materialEditorWindow.Dispatcher.HasShutdownFinished &&
                !materialEditorWindow.Dispatcher.HasShutdownStarted)
            {
                materialEditorWindow.Dispatcher.Invoke(
                    () =>
                    {
                        materialEditorWindow.Close();
                    });
            }
        }

        public static ObjectEditor.MainWindow EditorWindow => editorWindow;
        private static ObjectEditor.MainWindow editorWindow = null;

        private static MaterialEditor.MainWindow materialEditorWindow = null;
        public static MaterialEditor.MainWindow MaterialEditorWindow => materialEditorWindow;
    }
}
