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

                    StringBuilder CompiledShaderVariableBuilder = new StringBuilder("");
                    CompiledShaderVariableBuilder.AppendLine("using System.Runtime.InteropServices;");
                    CompiledShaderVariableBuilder.AppendLine("namespace SharpOpenGL");
                    CompiledShaderVariableBuilder.AppendLine("{");

                    StringBuilder CompiledVertexAttributeBuilder = new StringBuilder("");
                    CompiledVertexAttributeBuilder.AppendLine("using System;");
                    CompiledVertexAttributeBuilder.AppendLine("using System.Runtime.InteropServices;");
                    CompiledVertexAttributeBuilder.AppendLine("using OpenTK;");
                    CompiledVertexAttributeBuilder.AppendLine("using OpenTK.Graphics.OpenGL;");

                    CompiledVertexAttributeBuilder.AppendLine("namespace SharpOpenGL");
                    CompiledVertexAttributeBuilder.AppendLine("{");

                    StringBuilder CompiledSamplerVariableBuilder = new StringBuilder("");
                    CompiledSamplerVariableBuilder.AppendLine("using System.Runtime.InteropServices;");
                    CompiledSamplerVariableBuilder.AppendLine("namespace SharpOpenGL");
                    CompiledSamplerVariableBuilder.AppendLine("{");

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

                                var Names = program.ActiveUniformBlockNames;

                                for (int i = 0; i < Names.Count(); ++i)
                                {
                                    ShaderBindings sb = new ShaderBindings(program, i, "VS");

                                    var Contents = sb.TransformText();

                                    CompiledShaderVariableBuilder.Append(Contents);
                                }

                                VertexAttributeGenerator vag = new VertexAttributeGenerator(program, filename + "VertexAttributes");
                                
                                var Contents2 = vag.TransformText();

                                CompiledVertexAttributeBuilder.Append(Contents2);
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

                                CompiledShaderVariableBuilder.Append(Contents);

                                SamplerGenerator sampler = new SamplerGenerator(program, filename + "_Sampler");

                                var Contents2 = sampler.TransformText();

                                CompiledSamplerVariableBuilder.Append(Contents2);
                            }
                        }
                    }

                    CompiledShaderVariableBuilder.AppendLine("}");

                    CompiledVertexAttributeBuilder.AppendLine("}");

                    CompiledSamplerVariableBuilder.AppendLine("}");


                    if(File.Exists(Path.Combine(args[1], "CompiledShaderVariables.cs")))
                    {
                        var PrevContents = File.ReadAllText(Path.Combine(args[1], "CompiledShaderVariables.cs"));
                        if(PrevContents != CompiledSamplerVariableBuilder.ToString())
                        {
                            File.WriteAllText(Path.Combine(args[1], "CompiledShaderVariables.cs"), CompiledShaderVariableBuilder.ToString());       
                        }
                    }                    

                    File.WriteAllText(Path.Combine(args[1], "CompiledVertexAttributes.cs"), CompiledVertexAttributeBuilder.ToString());

                    File.WriteAllText(Path.Combine(args[1], "CompiledSamplerVariables.cs"), CompiledSamplerVariableBuilder.ToString());
                }
            }
        }
    }
}
