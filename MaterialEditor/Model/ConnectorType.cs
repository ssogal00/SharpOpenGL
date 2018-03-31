using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEditor
{
    public enum ConnectorType
    {
        Undefined,
        Input,
        Output,
    }

    public enum ConnectorDataType
    {
        InvalidDataType,
        ConstantVector4,
        ConstantVector3,
        ConstantVector2,
        ConstantFloat,
        ConstantInt,        
    }

    public static class ConnectionHelper
    {
        public static bool IsCompatibleConnector(ConnectorDataType a, ConnectorDataType b)
        {
            if(a == b)
            {
                return true;
            }
            
            return false;
        }
    }    
}
