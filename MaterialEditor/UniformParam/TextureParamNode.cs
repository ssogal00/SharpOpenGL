using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Media.Imaging;
using Core.MaterialBase;


namespace MaterialEditor
{
    public class TextureParamNode : NodeViewModel
    {
        static int TextureParamCount = 0;

        public TextureParamNode()
            :base("TextureParam")
        {   
            UniformName = string.Format("Texture_{0}", TextureParamCount);
            TextureParamCount++;
        }

        protected override void CreateInputOutputConnectors()
        {
            base.CreateInputOutputConnectors();

            OutputConnectors.Add(new ConnectorViewModel("Out", ConnectorDataType.ConstantVector4,0));
            OutputConnectors.Add(new ConnectorViewModel("R", ConnectorDataType.ConstantFloat,1));
            OutputConnectors.Add(new ConnectorViewModel("G", ConnectorDataType.ConstantFloat,2));            
            OutputConnectors.Add(new ConnectorViewModel("A", ConnectorDataType.ConstantFloat,3));
        }        

        public string UniformName
        {
            get;
            set;
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
        
        public override string GetExpressionForOutput(int outputIndex)
        {
            if(OutputConnectors.Count > outputIndex)
            {
                if(outputIndex == 0)
                {
                    return string.Format("texture({0} , {1}.TexCoord)", UniformName, MaterialBase.StageInputName);
                }
                else if(outputIndex == 1)
                {
                    return string.Format("texture({0} , {1}.TexCoord).r", UniformName, MaterialBase.StageInputName);
                }
                else if (outputIndex == 2)
                {
                    return string.Format("texture({0} , {1}.TexCoord).g", UniformName, MaterialBase.StageInputName);
                }
                else if (outputIndex == 3)
                {
                    return string.Format("texture({0} , {1}.TexCoord).b", UniformName, MaterialBase.StageInputName);
                }
            }

            return string.Empty;
        }
    }
}
