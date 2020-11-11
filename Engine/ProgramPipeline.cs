using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Core.OpenGLShader
{
    public class ProgramPipeline : IDisposable, IBindable
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

        public void UseVertexShaderProgram(VertexShader shader)
        {
            //GL.UseProgramStages(pipelineObject, ProgramStageMask.VertexShaderBit,);
        }

        public void UseVertexShaderProgram(int vsProgramObject)
        {
            GL.UseProgramStages(pipelineObject, ProgramStageMask.VertexShaderBit, vsProgramObject);
        }

        public void UseFragmentShaderProgram(int fsProgramObject)
        {
            GL.UseProgramStages(pipelineObject, ProgramStageMask.FragmentShaderBit, fsProgramObject);
        }

        public void UseTessEvalShaderProgram(int teProgramObject)
        {
            GL.UseProgramStages(pipelineObject, ProgramStageMask.TessEvaluationShaderBit, teProgramObject);
        }

        public void UseTessControlShaderProgram(int tcProgramObject)
        {
            GL.UseProgramStages(pipelineObject, ProgramStageMask.TessControlShaderBit, tcProgramObject);
        }

        public void Bind()
        {
            GL.BindProgramPipeline(pipelineObject);
        }

        public void Unbind()
        {
            GL.BindProgramPipeline(0);
        }

        public bool IsValid => pipelineObject != -1;

        private int pipelineObject = -1;
    }
}
