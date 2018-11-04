using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using OpenTK.Input;
using System.Windows.Forms;
using Core.CustomAttribute;

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
        
        public void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.BackSpace:
                    if (ConsoleCommandString.Length != 1)
                    {
                        ConsoleCommandString = ConsoleCommandString.Remove(ConsoleCommandString.Length - 1, 1);
                    }
                    break;

                case Key.CapsLock:
                case Key.ShiftLeft:
                case Key.ShiftRight:
                    break;

                case Key.Tilde:
                    ToggleActive();
                    break;
                
                case Key.Comma:
                    ConsoleCommandString += ",";
                    break;

                case Key.Period:
                    ConsoleCommandString += ".";
                    break;
                    
                case Key.Space:
                    ConsoleCommandString += " ";
                    break;

                case Key.Tab:
                    ConsoleCommandString += "\t";
                    break;

                case Key.Escape:
                    IsActive = false;
                    break;

                case Key.Enter:
                    ExecuteConsoleCommand(ConsoleCommandString);
                    IsActive = false;
                    break;

                default:
                    if (Control.IsKeyLocked(Keys.CapsLock))
                    {
                        ConsoleCommandString += e.Key.ToString();
                    }
                    else
                    {
                        ConsoleCommandString += e.Key.ToString().ToLower();
                    }
                    

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
