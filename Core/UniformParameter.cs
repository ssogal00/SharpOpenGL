using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    public class UniformParameterSet
    {
        protected Dictionary<string, UniformVariableParameter> ParamSet =
            new Dictionary<string, UniformVariableParameter>();

        public void SetParameters()
        {
            foreach (var param in ParamSet.Values)
            {
                param.SetParameter();
            }
        }
    }

    public abstract class UniformVariableParameter
    {
        public UniformVariableParameter(string name, int programObject)
        {
            ParamterName = name;
            ProgramObject = programObject;
        }

        protected string ParamterName = "";
        protected int ProgramObject = 0;

        public virtual void SetParameter()
        {
        }

        public virtual void SetValue(float newValue) { }
        public virtual void SetValue(OpenTK.Vector2 newValue) { }
        public virtual void SetValue(OpenTK.Vector3 newValue) { }

        public virtual void SetValue(OpenTK.Vector4 newValue) {}
    }


    public class UniformVariableFloatParameter : UniformVariableParameter
    {
        public UniformVariableFloatParameter(string name, int programObject, float value = 0.0f)
        : base(name, programObject)
        {
            FloatValue = value;
        }

        protected float FloatValue = 0;

        public override void SetParameter()
        {
            var Location = GL.GetUniformLocation(this.ProgramObject, ParamterName);
            GL.Uniform1(Location, FloatValue);
        }
    }

    public class UniformVariableVec2Parameter : UniformVariableParameter
    {
        public UniformVariableVec2Parameter(string name, int programObject)
            : base(name, programObject)
        {   
        }

        public UniformVariableVec2Parameter(string name, int programObject, OpenTK.Vector2 value)
            : base(name, programObject)
        {
            VectorValue = value;
        }

        protected OpenTK.Vector2 VectorValue = Vector2.Zero;

        public override void SetParameter()
        {
            var Location = GL.GetUniformLocation(this.ProgramObject, ParamterName);
            GL.Uniform2(Location, VectorValue);
        }
    }

    public class UniformVariableVec3Parameter : UniformVariableParameter
    {
        public UniformVariableVec3Parameter(string name, int programObject)
            : base(name, programObject)
        {
        }

        public UniformVariableVec3Parameter(string name, int programObject, OpenTK.Vector3 value)
            : base(name, programObject)
        {
            VectorValue = value;
        }

        protected OpenTK.Vector3 VectorValue = Vector3.Zero;

        public override void SetParameter()
        {
            var Location = GL.GetUniformLocation(this.ProgramObject, ParamterName);
            GL.Uniform3(Location, VectorValue);
        }
    }


}
