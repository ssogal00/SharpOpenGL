using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
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
