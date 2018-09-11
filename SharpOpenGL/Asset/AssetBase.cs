using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace SharpOpenGL.Asset
{
    [ZeroFormattable]
    public class AssetBase
    {
        public AssetBase()
        {
        }

        public virtual void ImportAssetSync()
        {   
        }

        public async virtual Task ImportAssetAsync()
        {
            return;
        }

        public virtual void LoadAssetSync()
        { 
        }

        public async virtual Task LoadAssetAsync()
        {
            return;
        }

        public virtual void SaveImportedAsset(string path)
        {

        }
    }
}
