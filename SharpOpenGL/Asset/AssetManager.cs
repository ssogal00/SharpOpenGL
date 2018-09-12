using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using System.IO;
using OpenTK.Graphics.ES11;
using SharpOpenGL.StaticMesh;

namespace SharpOpenGL.Asset
{
    public class AssetManager
    {
        [Index(0)] public List<AssetInfo> AssetInfoList { get; protected set; } = new List<AssetInfo>();

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
        

        public void DiscoverStaticMesh()
        {
            List<string> objFileList = new List<string>();
            List<string> mtlFileList = new List<string>();

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

                    if(Directory.Exists("./Resources/Imported/StaticMesh") == false)
                    {
                        Directory.CreateDirectory("./Resources/Imported/StaticMesh");
                    }

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
