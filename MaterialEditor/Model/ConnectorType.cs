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
        
        public static bool SupportsCast(ConnectorDataType from, ConnectorDataType to)
        {
            // vector4 => vector3
            if(from == ConnectorDataType.ConstantVector4 && to == ConnectorDataType.ConstantVector3)
            {
                return true;
            }

            // vector4 => vector2
            if (from == ConnectorDataType.ConstantVector4 && to == ConnectorDataType.ConstantVector2)
            {
                return true;
            }

            // vector4 => float
            if (from == ConnectorDataType.ConstantVector4 && to == ConnectorDataType.ConstantFloat)
            {
                return true;
            }

            // vector3 => vector4
            if(from == ConnectorDataType.ConstantVector3 && to == ConnectorDataType.ConstantVector4)
            {
                return true;
            }

            if(from == ConnectorDataType.ConstantVector2 && to == ConnectorDataType.ConstantFloat)
            {
                return true;
            }

            return false;
        }
    }    
}
