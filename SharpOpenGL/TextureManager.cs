using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Texture;
using SharpOpenGL.Asset;

namespace SharpOpenGL
{
    public class TextureManager : Singleton<TextureManager>
    {
        public void ImportTextures()
        {
            foreach (var file in Directory.EnumerateFiles("./Resources/SponzaTexture"))
            {
                if (file.EndsWith(".dds") || file.EndsWith(".jpg") || file.EndsWith(".tga") || file.EndsWith(".jpeg"))
                {
                    var textureAsset = new Texture2DAsset(file);
                    //textureAsset.OriginalFilePath = file;
                    
                    // check if imported asset exist
                    string importedPath = "";
                    if (File.Exists(importedPath))
                    {
                        Console.WriteLine(string.Format("{0} found...", importedPath));
                    }
                    // if not exist then import
                    textureAsset.ImportAssetSync();
                }
            }
        }

        public Texture2D LoadTexture2D(string path)
        {
            if (TextureMap.ContainsKey(path))
            {
                return (Texture2D) TextureMap[path];
            }
            else
            {
                if (RenderingThread.Get().IsInRenderingThread())
                {
                    var newTexture = new Texture2D();
                }
            }
            return null;
        }

        protected int approxMemoryTotal = 0;
        Dictionary<string, TextureBase> TextureMap = new Dictionary<string, TextureBase>();
    }
}
