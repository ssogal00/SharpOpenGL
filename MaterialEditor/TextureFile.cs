using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;

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
