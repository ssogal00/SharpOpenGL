using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SharpOpenGL.StaticMesh;
using System.Xml.Linq;
using Core.OpenGLShader;
using OpenTK.Graphics.OpenGL;

namespace SharpOpenGL.Asset
{
    public class AssetManager
    {
        protected static AssetManager SingletonInstance = new AssetManager();

        protected Dictionary<string, AssetBase> AssetMap = new Dictionary<string, AssetBase>();

        protected static string ImportedDirectory = "./Resources/Imported";

        public static AssetManager Get()
        {
            return SingletonInstance;
        }

        public T GetAsset<T>(string name) where T : AssetBase
        {
            if (AssetMap.ContainsKey(name))
            {
                return (T) AssetMap[name];
            }

            return null;
        }

        public void DiscoverShader()
        {
            if(File.Exists("./Resources/Shader/MaterialList.xml") == false)
            {
                return;
            }

            if(Directory.Exists("./Resources/Imported/Shader") == false)
            {
                Directory.CreateDirectory("./Resources/Imported/Shader");
            }

            var root = XElement.Load("./Resources/Shader/MaterialList.xml");

            foreach (var node in root.Descendants("Material"))
            {
                int result = 0;
                GL.GetInteger(GetPName.NumProgramBinaryFormats, out result);
                var binaryFormatList = new int[result];
                GL.GetInteger(GetPName.ProgramBinaryFormats, binaryFormatList);

                string materialName = node.Attribute("name").Value;
                string vsPath = Path.Combine("./Resources", "Shader", node.Attribute("vertexShader").Value);
                string fsPath = Path.Combine("./Resources", "Shader", node.Attribute("fragmentShader").Value);
                var vs = new VertexShader();
                var fs = new FragmentShader();

                vs.CompileShader(File.ReadAllText(vsPath));
                fs.CompileShader(File.ReadAllText(fsPath));

                var program = new ShaderProgram(vs, fs);

                byte[] data = new byte[1024 * 1024 * 512]; // 512kb

                int binaryLength = 0;

                program.GetProgramBinary(ref data, out binaryLength);

                string importedFileName = string.Format("./Resources/Imported/Shader/{0}.material", materialName);

                using (var filestream = new FileStream(importedFileName, FileMode.CreateNew))
                {
                    filestream.Write(data, 0, binaryLength);
                }
            }
        }


        public void DiscoverStaticMesh()
        {
            List<string> objFileList = new List<string>();
            List<string> mtlFileList = new List<string>();

            if (Directory.Exists("./Resources/Imported/StaticMesh") == false)
            {
                Directory.CreateDirectory("./Resources/Imported/StaticMesh");
            }

            // discover importable obj , mtl files first
            foreach (var file in Directory.EnumerateFiles("./Resources/ObjMesh"))
            {
                if (file.EndsWith(".obj"))
                {
                    objFileList.Add(file);
                }
                else if (file.EndsWith(".mtl"))
                {
                    mtlFileList.Add(file);
                }
            }

            foreach (var file in Directory.EnumerateFiles(Path.Combine(ImportedDirectory, "StaticMesh")))
            {
                var staticMeshAsset = AssetBase.LoadAssetSync<StaticMeshAsset>(file);
                var name = Path.GetFileName(file);
                Console.WriteLine("[AssetManager] Found {0}...", name);
                AssetMap.Add(name, staticMeshAsset);
            }

            foreach (var objfile in objFileList)
            {
                var filename = Path.GetFileNameWithoutExtension(objfile);

                var importedPath = Path.GetFullPath (Path.Combine(ImportedDirectory, "StaticMesh", filename + ".staticmesh"));

                if (File.Exists(importedPath))
                {
                    continue;
                }
                
                var mtlFileName = mtlFileList.Where(x => Path.GetFileNameWithoutExtension(x).StartsWith(filename)).FirstOrDefault();
                
                if (mtlFileName != null)
                {
                    var staticMesh = new StaticMeshAsset(objfile, mtlFileName);
                    staticMesh.ImportAssetSync();

                    string importedAssetPath = Path.Combine("./Resources/Imported/StaticMesh", Path.GetFileNameWithoutExtension(objfile) + ".staticmesh");
                    staticMesh.SaveImportedAsset(importedAssetPath);
                    AssetMap.Add(filename + ".staticmesh", staticMesh);
                }
                else
                {
                    var staticMesh = new StaticMeshAsset(objfile);
                    staticMesh.ImportAssetSync();

                    if (Directory.Exists("./Resources/Imported/StaticMesh") == false)
                    {
                        Directory.CreateDirectory("./Resources/Imported/StaticMesh");
                    }

                    string importedAssetPath = Path.Combine("./Resources/Imported/StaticMesh", Path.GetFileNameWithoutExtension(objfile) + ".staticmesh");
                    staticMesh.SaveImportedAsset(importedAssetPath);
                    AssetMap.Add(filename + ".staticmesh", staticMesh);
                }
            }
        }

    }
}
