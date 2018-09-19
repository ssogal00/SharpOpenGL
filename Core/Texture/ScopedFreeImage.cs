using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeImageAPI;

namespace Core.Texture
{
    public class ScopedFreeImage : IDisposable
    {
        public ScopedFreeImage(string imagePath)
        {
            FreeImageHelper.Load(imagePath, out Width, out Height);
        }
        public void Dispose()
        {
            FreeImage.Unload(bitmap);
        }

        public FIBITMAP bitmap;
        public int Width = 0;
        public int Height = 0;
    }
}
