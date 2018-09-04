using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpOpenGL.Asset
{
    public enum AssetType
    {
        None,
        StaticMesh,
        Texture,
        CubeMapTexture,
    }

    public class AssetInfo
    {
        public string AssetPath;
        public AssetType AssetType;
    }
}
