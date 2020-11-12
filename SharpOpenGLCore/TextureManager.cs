using Core;
using Core.Texture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SharpOpenGL.Asset;
using PixelType = OpenTK.Graphics.OpenGL.PixelType;

namespace SharpOpenGL
{
    public class TextureManager : Singleton<TextureManager>
    {
        private string importedDirName = "./Imported";

        public void ImportTextures()
        {
            if (Directory.Exists(importedDirName) == false)
            {
                Directory.CreateDirectory(importedDirName);
            }

            var files = Directory.EnumerateFiles("./Resources/SponzaTexture","*.*", SearchOption.AllDirectories)
                .Concat(Directory.EnumerateFiles("./Resources/Texture", "*.*", SearchOption.AllDirectories));

            var supportedFormats = new string[]
            {
                ".dds",".jpg",".tga",".jpeg",".hdr",".exr"
            };

            foreach (var file in files)
            {
                if (file.EndsWith(".jpeg") || file.EndsWith(".jpg") || file.EndsWith(".tga") || file.EndsWith(".jpeg") || file.EndsWith(".png") || file.EndsWith(".dds"))
                {
                    var textureAsset = new Texture2DAsset(file);

                    var dirname = Path.Combine(importedDirName, Path.GetDirectoryName(file));
                    var fileName = Path.GetFileNameWithoutExtension(file);

                    if (Directory.Exists(dirname) == false)
                    {
                        Directory.CreateDirectory(dirname);
                    }
                    
                    var importedFileName = Path.Combine(dirname, fileName + ".imported");
                    
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
                        //textureAsset.SaveImportedAsset(importedFileName);
                    }
                }
            }
        }

        private string ConvertToImportedPath(string path)
        {
            if (path.EndsWith(".imported") == false)
            {
                var dirname = Path.Combine(importedDirName, Path.GetDirectoryName(path));
                var filename = Path.GetFileNameWithoutExtension(path);
                return Path.Combine(dirname, filename + ".imported");
            }

            return path;
        }

        public void UnloadTexture(string path)
        {
            var importedPath = ConvertToImportedPath(path);

            if (TextureMap.ContainsKey(importedPath))
            {
                TextureMap[importedPath].Dispose();
                TextureMap.Remove(importedPath);
            }
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
                    data = File.ReadAllBytes("./Imported/Resources/Texture/Checker.imported");
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

                ApproximateTextureMemory += asset.ByteLength;

                if (asset.OpenglPixelType == PixelType.Float)
                {
                    newTexture.Load(asset.Floats, asset.Width, asset.Height, asset.ImagePixelInternalFormat, asset.OpenglPixelFormat);
                }
                else
                {
                    newTexture.Load(asset.Bytes, asset.Width, asset.Height, asset.ImagePixelInternalFormat, asset.OpenglPixelFormat);
                }
                Console.WriteLine("Loading {0} completed", importedPath);
                Console.WriteLine("Texture Mem : {0}",ApproximateTextureMemory);
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

        private int ApproximateTextureMemory = 0;

        Dictionary<string, TextureBase> TextureMap = new Dictionary<string, TextureBase>();
    }
}
