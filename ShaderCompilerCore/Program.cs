using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using Core.OpenGLShader;

using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using OpenTK.Windowing.Desktop;

namespace ShaderCompilerCore
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Count() == 2)
            {
                Control c = new Control();
                IntPtr hWnd = c.Handle;

                GameWindow window = new GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default);
                {
                    window.MakeCurrent();

                    var shaderpath = Path.GetFullPath(args[0]);
                    var outputpath = Path.GetFullPath(args[1]);

                    if (Directory.Exists(shaderpath))
                    {
                        string materialXml = Path.Combine(args[0], "MaterialList.xml");
                        string dir = Path.GetDirectoryName(materialXml);

                        string vertexAttributeContents = "";
                        string uniformVariableContents = "";
                        string materialContents = "";

                        var jsonPath = Path.Combine(args[0], "ShaderDefines.json");
                        var json = File.ReadAllText(jsonPath);
                        var result = JsonSerializer.Deserialize<ShaderListToCompile>(json);
                        
                        if (result != null)
                        {
                            foreach (var item in result.ShaderList)
                            {
                                string vsCode = "";
                                string fsCode = "";
                                
                                vsCode = File.ReadAllText(Path.Combine(args[0], Path.GetFileName(item.VertexShaderPath)));
                                fsCode = File.ReadAllText(Path.Combine(args[0], Path.GetFileName(item.FragmentShaderPath)));

                                var vsDefines = item.VertexShaderDefines.Select(x => new Tuple<string, string>(x.name, x.value)).ToList();
                                var fsDefines = item.FragmentShaderDefines.Select(x => new Tuple<string, string>(x.name, x.value)).ToList();
                                
                                FragmentShader fs = new FragmentShader();
                                VertexShader vs = new VertexShader();
                                
                                ShaderProgram fsProgram = new ShaderProgram();
                                ShaderProgram vsProgram = new ShaderProgram();

                                var composedFSCode = Shader.ComposeShaderCode(fsDefines, fsCode);
                                var composedVSCode = Shader.ComposeShaderCode(vsDefines, vsCode);

                                fs.CompileShader(composedFSCode);
                                fsProgram.AttachShader(fs);

                                vs.CompileShader(composedVSCode);
                                vsProgram.AttachShader(vs);

                                string fsResult = "";
                                string vsResult = "";

                                if (fsProgram.LinkProgram(out fsResult) && vsProgram.LinkProgram(out vsResult))
                                {
                                    var materialCode = new MaterialCodeGenerator(vsProgram, fsProgram, 
                                        composedVSCode, composedFSCode, item.Name);

                                    var codeContents = materialCode.GetCodeContents();
                                    materialContents += codeContents;
                                }
                                else
                                {
                                    var sb = new StringBuilder();
                                    sb.AppendLine(string.Format(@"Vertex Shader : {0} , Error : {1}", item.VertexShaderPath, vsResult));
                                    sb.AppendLine(string.Format(@"Fragment Shader : {0}, Error : {1}", item.FragmentShaderPath, fsResult));
                                    
                                    Console.Write(sb.ToString());
                                    Debug.Assert(false, sb.ToString());
                                }

                                var vsUniformCodeGen = new ShaderUniformCodeGenerator(vsProgram, item.Name);
                                var vertexUniformVariableContents = vsUniformCodeGen.GetCodeContents();
                                uniformVariableContents += vertexUniformVariableContents;

                                var fsUniformCodeGen = new ShaderUniformCodeGenerator(fsProgram, item.Name);
                                var fragmentUniformVariableContents = fsUniformCodeGen.GetCodeContents();
                                if (vertexUniformVariableContents != fragmentUniformVariableContents)
                                {
                                    uniformVariableContents += fragmentUniformVariableContents;
                                }
                            }
                        }

                        if (File.Exists(materialXml))
                        {
                            var Root = XElement.Load(materialXml);
                            foreach (var node in Root.Descendants("Material"))
                            {
                                string vsPath = node.Attribute("vertexShader").Value;
                                string fsPath = node.Attribute("fragmentShader").Value;
                                string materialName = node.Attribute("name").Value;

                                FragmentShader fs = new FragmentShader();
                                VertexShader vs = new VertexShader();

                                ShaderProgram fsProgram = new ShaderProgram();
                                ShaderProgram vsProgram = new ShaderProgram();

                                fs.CompileShader(File.ReadAllText(Path.Combine(dir, fsPath)));
                                fsProgram.AttachShader(fs);

                                vs.CompileShader(File.ReadAllText(Path.Combine(dir, vsPath)));
                                vsProgram.AttachShader(vs);

                                string fsResult = "";
                                string vsResult = "";

                                if (fsProgram.LinkProgram(out fsResult) && vsProgram.LinkProgram(out vsResult))
                                {
                                    var materialCode = new MaterialCodeGenerator(vsProgram, fsProgram, File.ReadAllText(Path.Combine(dir, vsPath)), File.ReadAllText(Path.Combine(dir, fsPath)), materialName);

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
                            File.WriteAllText(Path.Combine(outputpath, VertexAttributeOutputFilename), CodeGenerator.GetCodeWithNamesapceAndDependency(vertexAttributeContents, "CompiledMaterial"));

                            var UniformVariableOutputFilename = "CompiledUniformVariable.cs";
                            File.WriteAllText(Path.Combine(outputpath, UniformVariableOutputFilename), CodeGenerator.GetCodeWithNamesapceAndDependency(uniformVariableContents, "CompiledMaterial"));

                            var CompiledMaterialOutputFilename = "CompiledMaterial.cs";
                            File.WriteAllText(Path.Combine(outputpath, CompiledMaterialOutputFilename), CodeGenerator.GetCodeWithNamesapceAndDependency(materialContents, "CompiledMaterial"));
                        }
                    }
                }
            }
        }
    }
}
