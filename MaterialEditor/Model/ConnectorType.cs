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
        MultipleDataType,
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

        public static string GetCastString(string original, ConnectorDataType from, ConnectorDataType to)
        {
            // vec4 => vec3
            if(from == ConnectorDataType.ConstantVector4 && to == ConnectorDataType.ConstantVector3)
            {
                return string.Format("({0}).xyz", original);
            }

            // vec4 => vec2
            if(from == ConnectorDataType.ConstantVector4 && to == ConnectorDataType.ConstantVector2)
            {
                return string.Format("({0}).xy", original);
            }

            // vec4 => float
            if (from == ConnectorDataType.ConstantVector4 && to == ConnectorDataType.ConstantFloat)
            {
                return string.Format("({0}).x", original);
            }

            // vec3 => vec4
            if (from == ConnectorDataType.ConstantVector3 && to == ConnectorDataType.ConstantVector4)
            {
                return string.Format("vec4({0}, 0)", original);
            }

            // vec2 => vec3
            if (from == ConnectorDataType.ConstantVector2 && to == ConnectorDataType.ConstantVector3)
            {
                return string.Format("vec3({0}, 0)", original);
            }


            return original ;
        }
        
        public static bool SupportsCast(ConnectorDataType from, ConnectorDataType to)
        {
            if(from == to)
            {
                return true;
            }

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

            // vector3 => float
            if(from == ConnectorDataType.ConstantVector3 && to == ConnectorDataType.ConstantFloat)
            {
                return true;
            }

            // vector3 => vector2
            if(from == ConnectorDataType.ConstantVector3 && to == ConnectorDataType.ConstantVector2)
            {
                return true;
            }

            // vector2 => float
            if(from == ConnectorDataType.ConstantVector2 && to == ConnectorDataType.ConstantFloat)
            {
                return true;
            }

            return false;
        }
    }    
}
