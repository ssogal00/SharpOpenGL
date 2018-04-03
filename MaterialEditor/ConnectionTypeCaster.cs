using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public static class ConnectionTypeCaster
    {
        
        public static string GetCastString(ConnectorDataType from, ConnectorDataType to, string originalString)
        {
            if(ConnectionHelper.SupportsCast(from, to))
            {
                if(from == ConnectorDataType.ConstantVector4 && to == ConnectorDataType.ConstantVector3)
                {
                    return string.Format("({0}).xyz", originalString);
                }                
                else if(from == ConnectorDataType.ConstantVector4 && to == ConnectorDataType.ConstantVector2)
                {
                    return string.Format("({0}).xy", originalString);
                }
            }

            return string.Empty;
        }

        
    }
}
