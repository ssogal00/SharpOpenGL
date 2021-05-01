using Core;
using Core.Texture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace Engine
{
    public class TextureManager : Singleton<TextureManager>
    {
        public void UnloadTexture(string path)
        {
            if (TextureMap.ContainsKey(path))
            {
                TextureMap[path].Dispose();
                TextureMap.Remove(path);
            } 
        }
        
        public Texture2D LoadTexture2D(string path)
        {
            //
            if (!RenderingThread.IsInRenderingThread())
            {
                return null;
            }

            if (TextureMap.ContainsKey(path))
            {
                return (Texture2D) TextureMap[path];
            }

            if (File.Exists(path) == false)
            {
                return null;
            }

            Texture2D result = new Texture2D();
            bool bSuccess = false;
            // dds
            if (path.EndsWith(".dds", StringComparison.InvariantCultureIgnoreCase))
            {
                bSuccess = result.LoadFromDDSFile(path);
            }
            // jpg
            else if (path.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase) ||
                     path.EndsWith(".jpeg", StringComparison.InvariantCultureIgnoreCase) || 
                     path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
            {
                bSuccess = result.LoadFromJPGFile(path);
            }
            // tga
            else if (path.EndsWith(".tga", StringComparison.InvariantCultureIgnoreCase))
            {
                bSuccess = result.LoadFromTGAFile(path);
            }

            //
            if (bSuccess)
            {
                TextureMap.Add(path, result);
                return result;
            }
            else
            {
                result.Dispose();
                return null;
            }
        }


        // assume exists
        public Texture2D GetTexture2D(string path)
        {
            if (TextureMap.ContainsKey(path))
            {
                return (Texture2D) TextureMap[path];
            }
            else
            {
                Debug.Assert(false, string.Format("{0} not exist", path));
                return null;
            }
        }

        public void CacheTexture2D(string path)
        {
            if (TextureMap.ContainsKey(path) == false)
            {
                LoadTexture2D(path);
            }
        }

        Dictionary<string, TextureBase> TextureMap = new Dictionary<string, TextureBase>();
    }
}
