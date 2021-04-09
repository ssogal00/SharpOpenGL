using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
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

        public OpenGLBuffer(int size)
        {
            mBufferHandle = GL.GenBuffer();
        }

        protected void AllocateBuffer(int size)
        {
            Bind();
            GL.BufferData(mBufferTarget, new IntPtr(size), new IntPtr(0), mHint);
            mBufferCreated = true;
            
            CreateCachedBufferData(size);
        }

        private void CreateCachedBufferData(int size)
        {
            if (mCachedBufferData == null)
            {
                mCachedBufferData = new byte[size];
                mTempStorage = new byte[size];
                mCachedDataPtr = Marshal.AllocHGlobal(size);
            }
        }

        protected void AllocateBuffer<T>(T data) where T : struct
        {
            Bind();
            var size = new IntPtr(Marshal.SizeOf(data));
            GL.BufferData<T>(mBufferTarget, size, ref data, mHint);
            mBufferCreated = true;

            UpdateCachedBufferData(data);
        }

        protected void UpdateCachedBufferData<T>(T data) where T : struct
        {
            int size = Marshal.SizeOf(data);

            if (mCachedBufferData == null)
            {
                mCachedBufferData = new byte[size];
                mTempStorage = new byte[size];
                mCachedDataPtr = Marshal.AllocHGlobal(size);
            }
            
            Marshal.StructureToPtr(data, mCachedDataPtr, true);
            Marshal.Copy(mCachedDataPtr, mCachedBufferData, 0, size);
        }

        // return true if equals
        protected bool CompareCachedBufferData<T>(T data, int offset) where T : struct
        {
            if (mCachedBufferData == null)
            {
                return false;
            }

            int size = Marshal.SizeOf(data);
            Marshal.StructureToPtr(data,mCachedDataPtr,true);
            Marshal.Copy(mCachedDataPtr, mTempStorage,offset,size);

            byte[] prevData = mCachedBufferData.AsSpan(offset, size).ToArray();

            bool bEqual = prevData.SequenceEqual(mTempStorage.Skip(offset).Take(size));
            if (!bEqual)
            {
                System.Buffer.BlockCopy(mTempStorage, offset, mCachedBufferData, offset, size);
            }

            return bEqual;
        }
        
        public void Dispose()
        {
            Unbind();
            GL.DeleteBuffer(mBufferHandle);
        }

        public virtual void Bind()
        {
            GL.BindBuffer(mBufferTarget, mBufferHandle);
            bBind = true;
        }

        public bool IsBind
        {
            get { return bBind; }
        }

        public void Unbind()
        {
            GL.BindBuffer(mBufferTarget, 0);
            bBind = false;
        }

        public bool IsValid()
        {
            return mBufferHandle != -1;
        }

        public void BufferData(IntPtr Size, IntPtr Data)
        {
            Bind();
            GL.BufferData(mBufferTarget, Size, Data, mHint);
        }

        public void BufferData<T>(T data) where T : struct
        {   
            Bind();

            var size = new IntPtr(Marshal.SizeOf(data));
            
            if (mBufferCreated == false)
            {
                GL.BufferData<T>(mBufferTarget, size, ref data, mHint);
                mBufferCreated = true;
            }
            else
            {
                GL.BufferSubData(mBufferTarget, new IntPtr(0),  size, ref data);
            }

            UpdateCachedBufferData(data);
        }
     

        public void BufferSubData<T>(T data, int offset) where T : struct
        {
            Debug.Assert(mBufferCreated);

            bool bEqual = CompareCachedBufferData(data, offset);
            if (bEqual)
            {
                return;
            }
            
            Bind();
            var size = new IntPtr(Marshal.SizeOf(data));
            GL.BufferSubData<T>(mBufferTarget, new IntPtr(offset), size, ref data);
            UpdateCachedBufferData(data);
        }
     

        public T GetBufferWholeData<T>() where T: struct
        {
            T result = new T();

            Bind();

            GL.GetBufferSubData<T>(mBufferTarget, new IntPtr(0), Marshal.SizeOf(result), ref result);

            return result;
        }

        public void BufferData<T>(T[] Data) where T: struct
        {
            Bind();
            if (Data != null)
            {
                var Size = new IntPtr(Marshal.SizeOf(Data[0]) * Data.Length);
                GL.BufferData<T>(mBufferTarget, Size, Data, mHint);
            }
        }

        public IntPtr MapBuffer(BufferAccess access)
        {
            Bind();
            return GL.MapBuffer(mBufferTarget, access);
        }
        
        public BufferUsageHint UsageHint
        {
            get { return mHint; }
            set { mHint = value; }
        }

        public BufferTarget Target 
        { 
            get { return mBufferTarget; }
            protected set { mBufferTarget = value; }
        }

        public int BufferHandle
        {
            get { return mBufferHandle; }
            protected set { mBufferHandle = value; }
        }

        protected BufferTarget mBufferTarget;

        protected BufferUsageHint mHint;

        protected bool mBufferCreated = false;

        protected int mBufferHandle = -1;

        protected bool bBind = false;

        protected byte[] mCachedBufferData = null;

        protected byte[] mTempStorage = null;

        private IntPtr mCachedDataPtr = IntPtr.Zero;

    }
}
