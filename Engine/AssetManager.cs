using Core;
using Core.StaticMesh;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenTK.Graphics.ES11;

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
            string importedPath = $"./Resources/Imported/StaticMesh/{path}";

            if (!File.Exists(importedPath))
            {
                return await ImportAssetAsync<T>(path);
            }

            var cachedAsset = GetAsset<T>(Path.GetFileName(path));

            if (cachedAsset != null)
            {
                return cachedAsset;
            }

            return await Task.Factory.StartNew(() =>
            {
                lock (syncObject)
                {
                    byte[] data = File.ReadAllBytes(path);
                    T asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);
                    AssetMap.TryAdd(Path.GetFileName(path), asset);

                    return asset;
                }
            });
        }

        public static async Task<T> ImportAssetAsync<T>(string path) where T : AssetBase
        {
            if (!path.Contains(".staticmesh"))
            {
                throw new InvalidOperationException("invalid static mesh asset path");
            }

            string importedPath = Path.Combine("./Resources/Imported/StaticMesh", path);

            if (!File.Exists(importedPath))
            {
                var meshName =Path.GetFileNameWithoutExtension(path);
                var objPath = $"./Resources/ObjMesh/{meshName}.obj";
                var mtlPath = $"./Resources/ObjMesh/{meshName}.mtl";

                if (File.Exists(objPath) && File.Exists(mtlPath))
                {
                    var asset = await StaticMeshAsset.ImportAssetAsync(objPath, mtlPath);
                    AssetMap.TryAdd(meshName, asset);
                    return (T)asset;
                }
                else if (File.Exists(objPath) && !File.Exists(mtlPath))
                {
                    var asset = await StaticMeshAsset.ImportAssetAsync(objPath);
                    AssetMap.TryAdd(meshName, asset);
                    return (T)asset;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return await LoadAssetAsync<T>(path);
            }
        }

        public async Task ImportStaticMeshes()
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
                    await staticMesh.ImportAssetAsync();

                    string importedAssetPath = Path.Combine("./Resources/Imported/StaticMesh", Path.GetFileNameWithoutExtension(objfile) + ".staticmesh");
                    AssetMap.TryAdd(filename + ".staticmesh", staticMesh);
                }
                else
                {
                    var staticMesh = new StaticMeshAsset(objfile);
                    await staticMesh.ImportAssetAsync();

                    if (Directory.Exists("./Resources/Imported/StaticMesh") == false)
                    {
                        Directory.CreateDirectory("./Resources/Imported/StaticMesh");
                    }

                    string importedAssetPath = Path.Combine("./Resources/Imported/StaticMesh", Path.GetFileNameWithoutExtension(objfile) + ".staticmesh");
                    AssetMap.TryAdd(filename + ".staticmesh", staticMesh);
                }
            }
        }

    }
}
