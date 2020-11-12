using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace SharpOpenGL.Asset
{
    public class AssetInfo
    {
        [Index(0)] public virtual string AssetPath { get; protected set; } = "";

        [Index(1)] public virtual string AssetName { get; protected set; } = "";
    }
}
