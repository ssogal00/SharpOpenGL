using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;
using System.IO;
using OpenTK.Graphics.ES11;

namespace SharpOpenGL.Asset
{
    public class AssetManager
    {
        [Index(0)] public List<AssetInfo> AssetInfoList { get; protected set; } = new List<AssetInfo>();

        protected void DiscoverStaticMesh()
        {
            List<string> objFileList = new List<string>();
            List<string> mtlFileList = new List<string>();

            foreach (var file in Directory.EnumerateFiles("./Resources/ObjMesh"))
            {
                if (file.EndsWith(".obj"))
                {
                    objFileList.Add(file);
                }
                else if (file.EndsWith(".mtl"))
                {
                    mtlFileList.Add(file);
                }
            }
            

        }

        protected void ImportStaticMesh()
        {

        }
    }
}
