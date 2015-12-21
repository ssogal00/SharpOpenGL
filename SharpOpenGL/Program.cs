using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using SharpOpenGL.Buffer;
using System.Drawing;

namespace SharpOpenGL
{
    public class MainWindow : GameWindow
    {
        TestShaderVertexAttributes[] Vertices = new TestShaderVertexAttributes[]
        {
            new TestShaderVertexAttributes{ VertexColor = new Vector3(1,0,0), VertexPosition = new Vector3(-0.5f,-0.5f, 0.5f)},
            new TestShaderVertexAttributes{ VertexColor = new Vector3(1,0,0), VertexPosition = new Vector3( 0.5f,-0.5f, 0.5f)},
            new TestShaderVertexAttributes{ VertexColor = new Vector3(1,0,0), VertexPosition = new Vector3( 0.5f, 0.5f, 0.5f)},
            new TestShaderVertexAttributes{ VertexColor = new Vector3(1,0,0), VertexPosition = new Vector3(-0.5f, 0.5f, 0.5f)},
            new TestShaderVertexAttributes{ VertexColor = new Vector3(1,0,0), VertexPosition = new Vector3(-0.5f,-0.5f,-0.5f)},
            new TestShaderVertexAttributes{ VertexColor = new Vector3(1,0,0), VertexPosition = new Vector3( 0.5f,-0.5f,-0.5f)},
            new TestShaderVertexAttributes{ VertexColor = new Vector3(1,0,0), VertexPosition = new Vector3( 0.5f, 0.5f,-0.5f)},
            new TestShaderVertexAttributes{ VertexColor = new Vector3(1,0,0), VertexPosition = new Vector3(-0.5f, 0.5f,-0.5f)},
        };

        ushort[] CubeElements = new ushort[]
        {
            0, 1, 2, 2, 3, 0, // front face
            3, 2, 6, 6, 7, 3, // top face
            7, 6, 5, 5, 4, 7, // back face
            4, 0, 3, 3, 7, 4, // left face
            0, 1, 5, 5, 4, 0, // bottom face
            1, 5, 6, 6, 2, 1, // right face
        };


        protected Matrix4 ModelView = new Matrix4();
        protected Matrix4 Projection = new Matrix4();
        
        protected UniformBuffer ShaderUniformBuffer = null;        
        protected TestShader_VS_Transform Transform = new TestShader_VS_Transform();
        protected StaticVertexBuffer<TestShaderVertexAttributes> VB = null;
        protected IndexBuffer IB = null;

        protected ShaderProgram ProgramObject = null;

        protected override void OnLoad(EventArgs e)
        {   
            VSync = VSyncMode.Off;

            GL.ClearColor(System.Drawing.Color.White);

            VertexShader vs = new VertexShader();

            var dir = Directory.GetCurrentDirectory();
            
            var content = File.ReadAllText("..\\..\\Shader\\TestShader.vs");

            vs.CompileShader(content);

            FragmentShader fs = new FragmentShader();

            var fscontent = File.ReadAllText("..\\..\\Shader\\TestShader.fs");

            fs.CompileShader(fscontent);

            ProgramObject = new ShaderProgram();

            ProgramObject.AttachShader(vs);
            ProgramObject.AttachShader(fs);

            String result;
            if (ProgramObject.LinkProgram(out result))
            {
                ProgramObject.UseProgram();

                // init uniform buffer
                ShaderUniformBuffer = new UniformBuffer();
                ShaderUniformBuffer.Bind();
                
                // init vertex buffer
                VB = new StaticVertexBuffer<TestShaderVertexAttributes>();
                VB.Bind();
                VB.BufferData<TestShaderVertexAttributes>(ref Vertices);
                
                // init index buffer
                IB = new IndexBuffer();
                IB.Bind();
                IB.BufferData<ushort>(ref CubeElements);
            }
            else
            {
                
                MessageBox.Show(result);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            VB.Bind();
            VB.VertexAttribPointer(Vertices);
            IB.Bind();

            GL.DrawElements(PrimitiveType.Triangles, CubeElements.Length, DrawElementsType.UnsignedShort, IntPtr.Zero);

            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            float fAspectRatio = Width / (float)Height;

            Transform.Proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, fAspectRatio, 1, 100);

            Transform.ModelView = Matrix4.LookAt(new Vector3(1, 1, 1), new Vector3(0, 0, 0), Vector3.UnitY);
            
            ShaderUniformBuffer.BufferData<TestShader_VS_Transform>(ref Transform);

            var BlockIndex = ProgramObject.GetUniformBlockIndex("Transform");

            ShaderUniformBuffer.BindBufferBase(BlockIndex);
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (MainWindow example = new MainWindow())            
            {   
                example.Run(60);
            }
        }
    }
}
