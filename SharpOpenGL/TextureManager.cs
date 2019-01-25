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



        Dictionary<string, TextureBase> TextureMap = new Dictionary<string, TextureBase>();
    }
}
