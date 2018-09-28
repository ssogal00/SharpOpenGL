using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using System.IO;

namespace SharpOpenGL.Asset
{
    public class AssetBase
    {
        public AssetBase()
        {
        }

        public virtual void ImportAssetSync()
        {   
        }

        public virtual Task ImportAssetAsync()
        {
            return null;
        }

        
        public virtual void OnPostLoad()
        {
            bReady = true;
        }

        public virtual void SaveImportedAsset(string path)
        {

        }

        protected bool bReady = false;
    }
}
