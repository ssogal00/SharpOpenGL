using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Reflection;
using SharpOpenGL.StaticMesh;
using System.Xml.Linq;
using Core.OpenGLShader;
using System.Threading.Tasks;
using Core;
using Core.MaterialBase;

namespace SharpOpenGL.Asset
{
    public class AssetManager : Singleton<AssetManager>
    {
        private static object AssetMapLock = new object();

        protected static ConcurrentDictionary<string, AssetBase> AssetMap = new ConcurrentDictionary<string, AssetBase>();

        protected static string ImportedDirectory = "./Resources/Imported";

        public static T GetAsset<T>(string name) where T : AssetBase
        {
            if (AssetMap.ContainsKey(name))
            {
                return (T) AssetMap[name];
            }

            return null;
        }

        public static T LoadAssetSync<T>(string path) where T : AssetBase
        {
            var cachedAsset = GetAsset<T>(Path.GetFileName(path));

            if (cachedAsset != null)
            {
                return cachedAsset;
            }

            byte[] data = File.ReadAllBytes(path);
            T asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);
            AssetMap.TryAdd(Path.GetFileName(path), asset);

            if (RenderingThread.Get().IsInRenderingThread())
            {
                asset.InitializeInRenderThread();
            }
            else
            {
                RenderingThread.Get().Enqueue(
                () =>
                {
                    asset.InitializeInRenderThread();
                });
            }
            
            return asset;
        }

        public static async Task<T> LoadAssetAsync<T>(string path) where T : AssetBase
        {
            return await Task.Factory.StartNew(() =>
            {
                var cachedAsset = GetAsset<T>(Path.GetFileName(path));

                if (cachedAsset != null)
                {
                    return cachedAsset;
                }

                byte[] data = File.ReadAllBytes(path);
                T asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);
                AssetMap.TryAdd(Path.GetFileName(path), asset);

                RenderingThread.Get().Enqueue(() =>
                {
                    asset.InitializeInRenderThread();
                });
                
                return asset;
            });
        }

        public void DiscoverShader()
        {
            Debug.Assert(RenderingThread.Get().IsInRenderingThread());

            if(File.Exists("./Resources/Shader/MaterialList.xml") == false)
            {
                return;
            }

            if(Directory.Exists("./Resources/Imported/Shader") == false)
            {
                Directory.CreateDirectory("./Resources/Imported/Shader");
            }
            
            var compiledShaderAssembly = Assembly.Load("CompiledShader");
            var types = compiledShaderAssembly.GetTypes();

            foreach (var t in types)
            {
                if (t.IsSubclassOf(typeof(MaterialBase)))
                {
                    var instance = (MaterialBase)Activator.CreateInstance(t);
                    AssetMap.TryAdd(t.Name, instance);
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
                var staticMeshAsset = AssetManager.LoadAssetSync<StaticMeshAsset>(file);
                var name = Path.GetFileName(file);
                Console.WriteLine("[AssetManager] Found {0}...", name);
                AssetMap.TryAdd(name, staticMeshAsset);
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
                    AssetMap.TryAdd(filename + ".staticmesh", staticMesh);
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
                    AssetMap.TryAdd(filename + ".staticmesh", staticMesh);
                }
            }
        }

    }
}
