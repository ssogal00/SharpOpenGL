using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using SharpOpenGL;

using OpenTK;
using OpenTK.Platform;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using System.IO;

namespace ShaderCompiler
{
    public static class Program
    {        
        public static void Main(string[] args)
        {
            if(args.Count() == 2)
            {
                Control c = new Control();
                IntPtr hWnd = c.Handle;
                
                using (OpenTK.Platform.IWindowInfo windowInfo = OpenTK.Platform.Utilities.CreateWindowsWindowInfo(hWnd))
                using (OpenTK.Graphics.GraphicsContext context = new OpenTK.Graphics.GraphicsContext(GraphicsMode.Default, windowInfo))
                {
                    context.MakeCurrent(windowInfo);

                    context.LoadAll();

                    StringBuilder builder = new StringBuilder("");
                    builder.AppendLine("using System.Runtime.InteropServices;");
                    builder.AppendLine("namespace SharpOpenGL");
                    builder.AppendLine("{");                    

                    if (Directory.Exists(args[0]))
                    {
                        // generate code for vertex shader files
                        foreach (var vsFile in Directory.EnumerateFiles(args[0], "*.vs"))
                        {
                            VertexShader vs = new VertexShader();
                            ShaderProgram program = new ShaderProgram();

                            vs.CompileShader(File.ReadAllText(vsFile));
                            program.AttachShader(vs);

                            String result;
                            if (program.LinkProgram(out result))
                            {
                                var filename = Path.GetFileNameWithoutExtension(vsFile);

                                ShaderBindings sb = new ShaderBindings(program, 0, filename + "_VS");

                                var Contents = sb.TransformText();

                                builder.Append(Contents);                            
                            }                            
                        }

                        // generate code for fragment shader files
                        foreach (var fsFile in Directory.EnumerateFiles(args[0], "*.fs"))
                        {
                            FragmentShader fs = new FragmentShader();
                            ShaderProgram program = new ShaderProgram();

                            fs.CompileShader(File.ReadAllText(fsFile));
                            program.AttachShader(fs);

                            String result;
                            if (program.LinkProgram(out result))
                            {
                                var filename = Path.GetFileNameWithoutExtension(fsFile);

                                ShaderBindings sb = new ShaderBindings(program, 0, filename + "_FS");

                                var Contents = sb.TransformText();

                                builder.Append(Contents);
                            }
                        }
                    }

                    builder.AppendLine("}");

                    File.WriteAllText(Path.Combine(args[1], "ShaderVariable.cs"), builder.ToString());
                }
            }
        }
    }
}
