using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Core.OpenGLShader
{
    public class ProgramPipeline : IDisposable
    {

        public ProgramPipeline()
        {
            GL.CreateProgramPipelines(1, out pipelineObject);
        }

        public void Dispose()
        {
            GL.DeleteProgramPipeline(pipelineObject);
            pipelineObject = -1;
        }

        public void UseVertexShaderProgram(int vertexShaderObject)
        {
            GL.UseProgramStages(pipelineObject, ProgramStageMask.VertexShaderBit, vertexShaderObject);
        }

        public void UseFragmentShaderProgram(int fragmentShaderObject)
        {
            GL.UseProgramStages(pipelineObject, ProgramStageMask.FragmentShaderBit, fragmentShaderObject);
        }

        public void UseTessEvalShaderProgram(int tesselationShaderObject)
        {
            GL.UseProgramStages(pipelineObject, ProgramStageMask.TessEvaluationShaderBit, tesselationShaderObject);
        }

        public void UseTessControlShaderProgram(int tesselationShaderObject)
        {
            GL.UseProgramStages(pipelineObject, ProgramStageMask.TessControlShaderBit, tesselationShaderObject);
        }

        public void Bind()
        {
            GL.BindProgramPipeline(pipelineObject);
        }

        public bool IsValid => pipelineObject != -1;

        private int pipelineObject = -1;
    }
}
