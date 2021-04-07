using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace Core.Buffer
{
    public interface IBufferDataSetup
    {
        public void BufferData<T>(ref T[] Data) where T : struct;
        public void BufferData<T1, T2>(ref T1[] Data1, ref T2[] Data2) where T1 : struct where T2 : struct;
        public void BufferData<T1, T2, T3>(ref T1[] Data1, ref T2[] Data2, ref T3[] Data3) where T1 : struct where T2 : struct where T3 : struct;

        public void BufferData<T>(ref T Data) where T : struct;
        public void BufferData<T1, T2>(ref T1 Data1, ref T2 Data2) where T1 : struct where T2 : struct;
        public void BufferData<T1, T2, T3>(ref T1 Data1, ref T2 Data2, ref T3 Data3) where T1 : struct where T2 : struct where T3 : struct;
    }

    public abstract class OpenGLBuffer : IDisposable, IBindable
    {
        public OpenGLBuffer()
        {
            mBufferHandle = GL.GenBuffer();
        }

        public void Dispose()
        {
            Unbind();
            GL.DeleteBuffer(mBufferHandle);
        }

        public virtual void Bind()
        {
            GL.BindBuffer(bufferTarget, mBufferHandle);
            bBind = true;
        }

        public bool IsBind
        {
            get { return bBind; }
        }

        public void Unbind()
        {
            GL.BindBuffer(bufferTarget, 0);
            bBind = false;
        }

        public bool IsValid()
        {
            return mBufferHandle != -1;
        }

        public void BufferData(IntPtr Size, IntPtr Data)
        {
            Bind();
            GL.BufferData(bufferTarget, Size, Data, hint);            
        }

        public void BufferData<T>(T Data) where T : struct
        {   
            Bind();
            
            var Size = new IntPtr(Marshal.SizeOf(Data));
            GL.BufferData<T>(bufferTarget, Size, ref Data, hint);            
        }
     
        public void BufferWholeData<T>(T Data) where T: struct
        {            
            Bind();
            GL.BufferSubData<T>(bufferTarget, new IntPtr(0), Marshal.SizeOf(Data), ref Data);
	    }

        public void BufferSubData<T>(T Data, int Offset) where T : struct
        {
            Bind();
            GL.BufferSubData<T>(bufferTarget, new IntPtr(Offset), Marshal.SizeOf(Data), ref Data);
        }
        
        public void GetBufferWholeData<T>(T Data) where T : struct
        {
            Bind();
            GL.GetBufferSubData<T>(bufferTarget, new IntPtr(0), Marshal.SizeOf(Data), ref Data);
        }

        public T GetBufferWholeData<T>() where T: struct
        {
            T result = new T();

            Bind();

            GL.GetBufferSubData<T>(bufferTarget, new IntPtr(0), Marshal.SizeOf(result), ref result);

            return result;
        }

        public void BufferData<T>(T[] Data) where T: struct
        {
            Bind();
            if (Data != null)
            {
                var Size = new IntPtr(Marshal.SizeOf(Data[0]) * Data.Length);
                GL.BufferData<T>(bufferTarget, Size, Data, hint);
            }
        }

        public IntPtr MapBuffer(BufferAccess access)
        {
            Bind();
            return GL.MapBuffer(bufferTarget, access);
        }
        
        public BufferUsageHint UsageHint
        {
            get { return hint; }
            set { hint = value; }
        }

        public BufferTarget Target 
        { 
            get { return bufferTarget; }
            protected set { bufferTarget = value; }
        }

        public int BufferHandle
        {
            get { return mBufferHandle; }
            protected set { mBufferHandle = value; }
        }

        protected BufferTarget bufferTarget;

        protected BufferUsageHint hint;

        protected int mBufferHandle = -1;

        protected bool bBind = false;

        public string DebugName = "";
    }
}
