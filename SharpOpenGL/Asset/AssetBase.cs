using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using System.IO;

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

        public static T LoadAssetSync<T>(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            T asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);
            return asset;
        }

        public static async Task<T> LoadAssetAsync<T>(string path)
        {
            return await Task.Factory.StartNew(() =>
            {
                byte[] data = File.ReadAllBytes(path);
                T asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);
                return asset;
            });
        }

        public virtual void SaveImportedAsset(string path)
        {

        }
    }
}
