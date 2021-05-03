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
        


        private static string GetMaterialCodeContents(ShaderCompileInfo compileInfo, out ShaderProgram vsProgram)
        {
            string vsCode = string.Empty;
            string fsCode = string.Empty;
            string gsCode = string.Empty;

            List<Tuple<string,string>> vsDefines = new List<Tuple<string, string>>();
            List<Tuple<string, string>> fsDefines = new List<Tuple<string, string>>();
            List<Tuple<string, string>> gsDefines = new List<Tuple<string, string>>();

            FragmentShader fs = new FragmentShader();
            VertexShader vs = new VertexShader();
            GeometryShader gs = new GeometryShader();

            vsProgram = new ShaderProgram();

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

            if (!fragmentShaderSuccess || !vertexShaderSuccess)
            {
                Console.WriteLine("vertexShader Error in {0} : {1}",compileInfo.VertexShaderPath,  vsError);
                Console.WriteLine("fragmentShader Error in {0} : {1}", compileInfo.FragmentShaderPath, fsError);
            }

            Debug.Assert(fragmentShaderSuccess && vertexShaderSuccess);

            vsProgram.AttachShader(vs);
            vsProgram.AttachShader(fs);

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
                vsProgram.AttachShader(gs);
            }


            string result = "";
            
            string linkResult = "";
            if (vsProgram.LinkProgram(out linkResult))
            {
                var samplers = vsProgram.GetSampler2DNames();
                var generator =
                    new MaterialCodeGenerator(vsProgram,
                        composedVSCode, composedFSCode, composedGSCode, compileInfo.Name);

                result += generator.GetCodeContents();
            }
            else
            {
                Console.WriteLine("Shader Link Error : {0}", linkResult);
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
                                materialContents += GetMaterialCodeContents(item, out ShaderProgram vsProgram);

                                var vsUniformCodeGen = new ShaderUniformCodeGenerator(vsProgram, item.Name);
                                var vertexUniformVariableContents = vsUniformCodeGen.GetCodeContents();
                                uniformVariableContents += vertexUniformVariableContents;
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

                                var compileInfo = new ShaderCompileInfo();
                                compileInfo.VertexShaderPath = vsPath;
                                compileInfo.FragmentShaderPath = fsPath;
                                compileInfo.Name = materialName;

                                materialContents += GetMaterialCodeContents(compileInfo, out ShaderProgram shaderProgarm);
                                
                                var gen = new VertexAttributeCodeGenerator(shaderProgarm, materialName);
                                vertexAttributeContents += gen.GetCodeContents();

                                var vsUniformCodeGen = new ShaderUniformCodeGenerator(shaderProgarm, materialName);
                                var vertexUniformVariableContents = vsUniformCodeGen.GetCodeContents();
                                uniformVariableContents += vertexUniformVariableContents;
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
