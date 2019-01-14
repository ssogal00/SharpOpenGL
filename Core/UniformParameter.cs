using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class UniformParameterSet
    {
        protected Dictionary<string, UniformVariableParameter> ParamSet = new Dictionary<string, UniformVariableParameter>();

        public void SetParameters()
        {
            foreach (var param in ParamSet.Values)
            {
                param.SetParameter();
            }
        }
    }

    public class UniformVariableParameter 
    {
        public UniformVariableParameter(string name, int location)
        {
            ParamterName = name;
            Location = location;
        }

        protected string ParamterName = "";
        protected int Location = -1;

        public virtual void SetParameter()
        {
        }
    }

    public class UniformVariableFloatParameter : UniformVariableParameter
    {
        public UniformVariableFloatParameter(string name, int location, float value)
        : base(name, location)
        {
            FloatValue = value;
        }

        protected float FloatValue = 0;

        public override void SetParameter()
        {
            GL.Uniform1(Location, FloatValue);
        }
    }
}
