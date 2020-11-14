using Core;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SharpOpenGL
{
    internal class ConsoleCommandManager : Singleton<ConsoleCommandManager>
    {
        public void ToggleActive()
        {
            IsActive = !IsActive;
        }

        public ConsoleCommandManager()
        {
            
        }
        
        public void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                
                
                case Keys.Comma:
                    ConsoleCommandString += ",";
                    break;

                case Keys.Period:
                    ConsoleCommandString += ".";
                    break;
                    
                case Keys.Space:
                    ConsoleCommandString += " ";
                    break;

                case Keys.Tab:
                    ConsoleCommandString += "\t";
                    break;

                case Keys.Escape:
                    IsActive = false;
                    break;
            }
        }

        protected void ExecuteConsoleCommand(string command)
        {
            var commandParams = command.Split(new char [] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            if (commandParams.Length == 0)
            {
                return;
            }

            var commandName = commandParams[0].Remove(0,1);

            if (commandMap.ContainsKey(commandName))
            {
                commandMap[commandName](commandParams.Skip(1).ToArray());
            }
        }

        
        protected static void TestCommand(string[] param)
        {
            Console.WriteLine("TestCommand");
        }
        
        protected static void Test2Command(string[] param)
        {
        }

        
        

        protected delegate void ConsoleCommandFunction(string[] param);

        protected Dictionary<string, ConsoleCommandFunction> commandMap = new Dictionary<string, ConsoleCommandFunction>()
        {
            { "Test", TestCommand },
            { "Test2", TestCommand }
        };

        public string ConsoleCommandString = ">";

        public bool IsActive = false;

        protected bool CapsLock = false;

        private byte[] keyboardState = new byte[256];
    }
}
