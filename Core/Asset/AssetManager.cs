using Core;
using Core.StaticMesh;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Asset
{
    public class AssetManager : Singleton<AssetManager>
    {
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

        private static object syncObject = new object();

        public static T LoadAssetSync<T>(string path) where T : AssetBase
        {
            var cachedAsset = GetAsset<T>(Path.GetFileName(path));

            if (cachedAsset != null)
            {
                return cachedAsset;
            }

            lock (syncObject)
            {
                byte[] data = File.ReadAllBytes(path);
                T asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);
                AssetMap.TryAdd(Path.GetFileName(path), asset);

                return asset;
            }
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

                lock (syncObject)
                {
                    byte[] data = File.ReadAllBytes(path);
                    T asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);
                    AssetMap.TryAdd(Path.GetFileName(path), asset);

                    return asset;
                }
            });
        }

        public void ImportTextures()
        {

        }

        public void ImportStaticMeshes()
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
