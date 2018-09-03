using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Asset
{
    public class Asset
    {
        public virtual void ImportAssetSync()
        {   
        }

        public virtual Task ImportAssetAsync()
        {
            return null;
        }

        public virtual bool Save()
        {
            return true;
        }
    }
}
