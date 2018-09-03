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

        public async virtual Task ImportAssetAsync()
        {
            return;
        }

        public virtual bool Save()
        {
            return true;
        }
    }
}
