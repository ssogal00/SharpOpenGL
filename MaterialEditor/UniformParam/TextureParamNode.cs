using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Media.Imaging;


namespace MaterialEditor
{
    public class TextureParamNode : NodeViewModel
    {
        public TextureParamNode()
            :base("TextureParam")
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SponzaTexture", "Background_Albedo.dds");
            ImageSource = new BitmapImage(new Uri(imagePath));
        }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();

            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector4));
        }

        public string UniformName
        {
            get;set;
        }

        private BitmapImage imageSource = null;

        public BitmapImage ImageSource
        {
            get
            {
                return imageSource;
            }

            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public override string ToExpression()
        {
            return string.Format("texture({0} ,InTexCoord)");
        }
    }
}
