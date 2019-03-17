using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageLibWrapper;

namespace Core.Texture
{
    public class ScopedSTBImage : IDisposable
    {
        public ScopedSTBImage(string imagePath)
        {
            if (File.Exists(imagePath) == true)
            {
                var lib = new ImageLibrary();
                var info = new ImageInfo();
                Data = lib.Load(imagePath, info);
                Width = info.Width;
                Channels = info.Channels;
                Height = info.Height;
            }
            else
            {
            }
        }

        public void Dispose()
        {
        }

        public int Width = 0;
        public int Height = 0;
        public int Channels = 0;
        public float[] Data = null;
    }
}
