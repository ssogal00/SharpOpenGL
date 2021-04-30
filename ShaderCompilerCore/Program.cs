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
using Engine.Rendering;
using OpenTK.Windowing.Desktop;

namespace ShaderCompilerCore
{
    public static class Program
    {
        private static string GetMaterialCodeContents(ShaderProgram vsProgram, ShaderProgram fsProgram, string composedVSCode, string composedFSCode, string shaderName)
        {
            string materialContents = string.Empty;

            if (fsProgram.LinkProgram(out string fsResult) && vsProgram.LinkProgram(out string vsResult))
            {
                var materialCode = new MaterialCodeGenerator(vsProgram, fsProgram, composedVSCode, composedFSCode, shaderName);

                var codeContents = materialCode.GetCodeContents();

                materialContents += codeContents;
            }

            return materialContents;
        }


        private static string GetMaterialCodeContents(ShaderCompileInfo compileInfo, out ShaderProgram vsProgram, out ShaderProgram fsProgram, out ShaderProgram gsProgram)
        {
            string vsCode = string.Empty;
            string fsCode = string.Empty;
            string gsCode = string.Empty;

            List<Tuple<string,string>> vsDefines = new List<Tuple<string, string>>();
            List<Tuple<string, string>> fsDefines = new List<Tuple<string, string>>();
            List<Tuple<string, string>> gsDefines = new List<Tuple<string, string>>();

            FragmentShader fs = new FragmentShader();
            VertexShader vs = new VertexShader();
            GeometryShader gs = null;

            fsProgram = new ShaderProgram();
            vsProgram = new ShaderProgram();
            gsProgram = null;

            string result = "";

            if (File.Exists(Path.Combine(mShaderFolder, Path.GetFileName(compileInfo.VertexShaderPath))))
            {
                vsCode = File.ReadAllText(Path.Combine(mShaderFolder, Path.GetFileName(compileInfo.VertexShaderPath)));
            }

            if (File.Exists(Path.Combine(mShaderFolder, Path.GetFileName(compileInfo.FragmentShaderPath))))
            {
                fsCode = File.ReadAllText(Path.Combine(mShaderFolder, Path.GetFileName(compileInfo.FragmentShaderPath)));
            }
            
            if (compileInfo.GeometryShaderPath != null && File.Exists(Path.Combine(mShaderFolder, Path.GetFileName(compileInfo.GeometryShaderPath))))
            {
                gsCode = File.ReadAllText(Path.Combine(mShaderFolder, Path.GetFileName(compileInfo.GeometryShaderPath)));
            }

            if (compileInfo.VertexShaderDefines != null)
            {
                vsDefines = compileInfo.VertexShaderDefines.Select(x => new Tuple<string, string>(x.name, x.value)).ToList();
            }

            if (compileInfo.FragmentShaderDefines != null)
            {
                fsDefines = compileInfo.FragmentShaderDefines.Select(x => new Tuple<string, string>(x.name, x.value)).ToList();
            }

            var composedFSCode = Shader.ComposeShaderCode(fsDefines, fsCode);
            var composedVSCode = Shader.ComposeShaderCode(vsDefines, vsCode);
            string composedGSCode = "";

            bool fragmentShaderSuccess = fs.CompileShader(composedFSCode, out string fsError);
            bool vertexShaderSuccess = vs.CompileShader(composedVSCode, out string vsError);

            fsProgram.AttachShader(fs);
            vsProgram.AttachShader(vs);

            Debug.Assert(fragmentShaderSuccess && vertexShaderSuccess);

            if (gsCode.Length > 0)
            {
                if (compileInfo.GeometryShaderDefines != null)
                {
                    gsDefines = compileInfo.GeometryShaderDefines
                        .Select(x => new Tuple<string, string>(x.name, x.value)).ToList();
                }

                composedGSCode = Shader.ComposeShaderCode(gsDefines, gsCode);
                bool gsShaderSuccess = gs.CompileShader(composedGSCode, out string gsError);
                Debug.Assert(gsShaderSuccess);
                gsProgram = new ShaderProgram();
                gsProgram.AttachShader(gs);
            }

            if (gsCode.Length > 0)
            {
                if (fsProgram.LinkProgram(out string fsResult) && vsProgram.LinkProgram(out string vsResult) &&
                    gsProgram.LinkProgram(out string gsResult))
                {
                    var generator =
                        new MaterialCodeGenerator(vsProgram, fsProgram, gsProgram, 
                            composedVSCode, composedFSCode, composedGSCode, compileInfo.Name);

                    result += generator.GetCodeContents();
                }
            }
            else
            {

                if (fsProgram.LinkProgram(out string fsResult) && vsProgram.LinkProgram(out string vsResult))
                {
                    var generator =
                        new MaterialCodeGenerator(vsProgram, fsProgram, composedVSCode, composedFSCode,
                            compileInfo.Name);

                    result += generator.GetCodeContents();
                }
            }

            return result;
        }

        private static string mShaderFolder = string.Empty;

        public static void Main(string[] args)
        {
            if (args.Count() == 2)
            {
                mShaderFolder = args[0];

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
                                materialContents += GetMaterialCodeContents(item, out ShaderProgram vsProgram, out ShaderProgram fsProgram, out ShaderProgram gsProgram);

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
