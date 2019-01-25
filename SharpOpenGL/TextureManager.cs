using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Texture;

namespace SharpOpenGL
{
    public class TextureManager : Singleton<TextureManager>
    {
        public void ImportTextures()
        {


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
        

        Dictionary<string, TextureBase> TextureMap = new Dictionary<string, TextureBase>();
    }
}
