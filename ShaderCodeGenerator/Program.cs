using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using Core.OpenGLShader;

using System.Diagnostics;
using OpenTK;
using OpenTK.Platform;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using System.IO;
using System.Xml.Linq;

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

                    var shaderpath = Path.GetFullPath(args[0]);
                    var outputpath = Path.GetFullPath(args[1]);

                    if (Directory.Exists(shaderpath))
                    {
                        string materialXml = Path.Combine(args[0], "MaterialList.xml");
                        string dir = Path.GetDirectoryName(materialXml);

                        string vertexAttributeContents = "";
                        string uniformVariableContents = "";
                        string materialContents = "";

                        if(File.Exists(materialXml))
                        {
                            var Root = XElement.Load(materialXml);
                            foreach(var Node in Root.Descendants("Material"))
                            {
                                string vsPath = Node.Attribute("vertexShader").Value;
                                string fsPath = Node.Attribute("fragmentShader").Value;
                                string materialName = Node.Attribute("name").Value;

                                FragmentShader fs = new FragmentShader();
                                VertexShader vs = new VertexShader();

                                ShaderProgram fsProgram = new ShaderProgram();
                                ShaderProgram vsProgram = new ShaderProgram();

                                fs.CompileShader(File.ReadAllText( Path.Combine(dir, fsPath)));
                                fsProgram.AttachShader(fs);

                                vs.CompileShader(File.ReadAllText(Path.Combine(dir, vsPath)));
                                vsProgram.AttachShader(vs);

                                string fsResult = "";
                                string vsResult = "";

                                if(fsProgram.LinkProgram(out fsResult) && vsProgram.LinkProgram(out vsResult))
                                {
                                    var materialCode = new MaterialCodeGenerator(vsProgram, fsProgram, File.ReadAllText(Path.Combine(dir, vsPath)), File.ReadAllText(Path.Combine(dir, fsPath)), materialName );

                                    var codeContents = materialCode.GetCodeContents();

                                    materialContents += codeContents;
                                }
                                else
                                {
                                    var sb = new StringBuilder();
                                    sb.AppendLine(string.Format(@"Vertex Shader : {0} , Error : {1}", vsPath, vsResult));
                                    sb.AppendLine(string.Format(@"Fragment Shader : {0}, Error : {1}", fsPath, fsResult));
                                    //MessageBox.Show(CompileResult);
                                    Console.Write(sb.ToString());
                                    Debug.Assert(false, sb.ToString());
                                }

                                var gen = new VertexAttributeCodeGenerator(vsProgram, materialName);
                                vertexAttributeContents += gen.GetCodeContents();                                

                                var vsUniformCodeGen = new ShaderUniformCodeGenerator(vsProgram, materialName);
                                var vertexUniformVariableContents = vsUniformCodeGen.GetCodeContents();
                                uniformVariableContents += vertexUniformVariableContents;

                                var fsUniformCodeGen = new ShaderUniformCodeGenerator(fsProgram, materialName);
                                var fragmentUniformVariableContents = fsUniformCodeGen.GetCodeContents();
                                if (vertexUniformVariableContents != fragmentUniformVariableContents)
                                {
                                    uniformVariableContents += fragmentUniformVariableContents;
                                }
                            }

                            var VertexAttributeOutputFilename = "CompiledVertexAttributes.cs";
                            File.WriteAllText(Path.Combine(outputpath, VertexAttributeOutputFilename), CodeGenerator.GetCodeWithNamesapceAndDependency(vertexAttributeContents));

                            var UniformVariableOutputFilename = "CompiledUniformVariable.cs";
                            File.WriteAllText(Path.Combine(outputpath, UniformVariableOutputFilename), CodeGenerator.GetCodeWithNamesapceAndDependency(uniformVariableContents));

                            var CompiledMaterialOutputFilename = "CompiledMaterial.cs";
                            File.WriteAllText(Path.Combine(outputpath, CompiledMaterialOutputFilename), CodeGenerator.GetCodeWithNamesapceAndDependency(materialContents));
                        }
                    }
                }
            }
        }
    }
}
