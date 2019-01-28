using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Texture;
using OpenTK.Graphics.ES20;
using SharpOpenGL.Asset;

namespace SharpOpenGL
{
    public class TextureManager : Singleton<TextureManager>
    {
        private string importedDirName = "./Resources/Imported/Texture";

        public void ImportTextures()
        {
            if (Directory.Exists(importedDirName) == false)
            {
                Directory.CreateDirectory(importedDirName);
            }

            var files = Directory.EnumerateFiles("./Resources/SponzaTexture").Concat(Directory.EnumerateFiles("./Resources/Texture"));

            foreach (var file in files)
            {
                if (file.EndsWith(".dds") || file.EndsWith(".jpg") || file.EndsWith(".tga") || file.EndsWith(".jpeg") || file.EndsWith(".png"))
                {
                    var textureAsset = new Texture2DAsset(file);

                    var fileName = Path.GetFileNameWithoutExtension(file);

                    var importedFileName = Path.Combine(importedDirName, fileName + ".imported");
                    
                    // check if imported asset exist
                    if (File.Exists(importedFileName))
                    {
                        Console.WriteLine(string.Format("{0} found...", importedFileName));
                    }
                    // if not exist then import
                    else
                    {
                        Console.WriteLine(string.Format("Importing Texture {0}", importedFileName));
                        textureAsset.ImportAssetSync();
                        textureAsset.SaveImportedAsset(importedFileName);
                    }
                }
            }
        }

        private string ConvertToImportedPath(string path)
        {
            if (path.EndsWith(".imported") == false)
            {
                return Path.Combine(importedDirName, Path.GetFileNameWithoutExtension(path) + ".imported");
            }

            return path;
        }

        public Texture2D LoadTexture2D(string path)
        {
            var importedPath = ConvertToImportedPath(path);

            if (TextureMap.ContainsKey(importedPath))
            {
                return (Texture2D) TextureMap[importedPath];
            }
            else
            {
                byte[] data = null;
                if (File.Exists(importedPath) == false)
                {   
                    data = File.ReadAllBytes("./Resources/Imported/Texture/checker.imported");
                }
                else
                {
                    // deserialize
                    data = File.ReadAllBytes(importedPath);
                }
                
                Texture2DAsset asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<Texture2DAsset>(data);
                
                var newTexture = new Texture2D();
                Console.WriteLine("Loading {0} started", importedPath);
                if (importedPath.Contains("spnza_bricks"))
                {
                    Console.WriteLine("Loading {0} started", importedPath);
                }
                newTexture.Load(asset.Bytes, asset.Width, asset.Height, asset.ImagePixelInternalFormat, asset.OpenglPixelFormat);
                Console.WriteLine("Loading {0} completed", importedPath);
                TextureMap.Add(importedPath, newTexture);
                return newTexture;
            }
        }

        // assume exists
        public Texture2D GetTexture2D(string path)
        {
            var importedPath = ConvertToImportedPath(path);

            if (TextureMap.ContainsKey(importedPath))
            {
                return (Texture2D)TextureMap[importedPath];
            }
            else
            {
                Debug.Assert(false, string.Format("{0} not exist", path));
                return null;
            }
        }

        public void CacheTexture2D(string path)
        {
            var importedPath = ConvertToImportedPath(path);

            if (TextureMap.ContainsKey(importedPath) == false)
            {
                LoadTexture2D(path);
            }
        }

        Dictionary<string, TextureBase> TextureMap = new Dictionary<string, TextureBase>();
    }
}
