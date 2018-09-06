using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace SharpOpenGL.Asset
{
    public class AssetBase
    {
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

        [Index(0)]
        public virtual string SavedAssetPath { get; protected set; } = "";
    }
}
