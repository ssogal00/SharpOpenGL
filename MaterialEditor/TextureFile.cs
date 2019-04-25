using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace MaterialEditor
{
    public class TextureFile : AbstractModelBase
    {
        public TextureFile(string fullpath)
        {
            ImageSource = new BitmapImage(new Uri(fullpath));
            FileName = Path.GetFileName(fullpath);
        }

        private BitmapImage imageSource = null;

        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public string FileName { get; set; }
    }
}
