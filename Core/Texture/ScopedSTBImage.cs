using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageLibWrapper;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Windows.Forms;

namespace Core.Texture
{
    public class ScopedSTBImage : IDisposable
    {
        public ScopedSTBImage(string imagePath, bool bLoadAsFloat = false)
        {
            if (File.Exists(imagePath) == true)
            {
                var lib = new ImageLibrary();
                var info = new ImageInfo();

                if (bLoadAsFloat)
                {
                    FloatData = lib.LoadAsFloat(imagePath, info);
                    OpenglPixelType = PixelType.Float;
                }
                else
                {
                    ByteData = lib.LoadAsByte(imagePath, info);
                    OpenglPixelType = PixelType.UnsignedByte;
                }

                Width = info.Width;
                Channels = info.Channels;
                Height = info.Height;

                if (Channels == 1)
                {
                    ImagePixelInternalFormat = PixelInternalFormat.Luminance;
                    OpenglPixelFormat = PixelFormat.Luminance;
                }
                else if (Channels == 2)
                {
                    Debug.Assert(false);
                }
                else if (Channels == 3)
                {
                    ImagePixelInternalFormat = PixelInternalFormat.Rgb;
                    OpenglPixelFormat = PixelFormat.Bgr;
                }
                else if (Channels == 4)
                {
                    ImagePixelInternalFormat = PixelInternalFormat.Rgba;
                    OpenglPixelFormat = PixelFormat.Bgra;
                }
            }
            else
            {
                Debug.Assert(false);
            }

        }

        public void Dispose()
        {
        }

        public int Width = 0;
        public int Height = 0;
        public int Channels = 0;
        public float[] FloatData = null;
        public byte[] ByteData = null;

        public int DataLength => Width * Height * Channels;

        private System.Drawing.Imaging.PixelFormat ImagePixelFormat = System.Drawing.Imaging.PixelFormat.Max;
        public PixelInternalFormat ImagePixelInternalFormat = PixelInternalFormat.Rgb;
        public OpenTK.Graphics.OpenGL.PixelFormat OpenglPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Rgb;
        public OpenTK.Graphics.OpenGL.PixelType OpenglPixelType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
    }
}
