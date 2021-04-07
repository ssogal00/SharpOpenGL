using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.RightsManagement;
using Core.Buffer;
using Core.Texture;
using OpenTK.Graphics.OpenGL;

namespace Core
{
    // change render state or draw 
    public class RenderCommand
    {
        public RenderCommand()
        {

        }

        //
        protected virtual void Setup()
        {
        }

        public virtual void Execute()
        {

        }

        public int Order => mOrder;
        public string CommandName => mCommandName;

        protected int mOrder = 0;

        protected string mCommandName = string.Empty;
    }

    public class ClearRenderCommand : RenderCommand
    {
        public ClearRenderCommand(Color clearColor, ClearBufferMask mask=ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit)
        {
        }

        public override void Execute()
        {
            GL.ClearColor(mClearColor);
            GL.Clear(mBufferMask);
        }

        private Color mClearColor = Color.White;
        private ClearBufferMask mBufferMask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit;
    }

    public class BlendingOnCommand : RenderCommand
    {
        public override void Execute()
        {
            GL.Enable(EnableCap.Blend);
        }
    }

    public class BlendingOffCommand : RenderCommand
    {
        public override void Execute()
        {
            GL.Disable(EnableCap.Blend);
        }
    }

    public class BlendingFuncCommand : RenderCommand
    {
        public BlendingFuncCommand(BlendingFactor src, BlendingFactor dst)
        {
            mSrc = src;
            mDst = dst;
        }
        public override void Execute()
        {
            GL.BlendFunc(mSrc, mDst);
        }

        protected BlendingFactor mSrc;
        protected BlendingFactor mDst;
    }

    public class MaterialSetUniformBufferCommand<T> : RenderCommand where T : struct
    {
        public MaterialSetUniformBufferCommand(MaterialBase.MaterialBase material, string name, ref T paramValue)
        {
            mMaterial = material;
            mName = name;
            mParamValue = paramValue;
        }

        public override void Execute()
        {
            mMaterial.SetUniformBufferValue<T>(mName, mParamValue);
        }

        public void UpdateValue(ref T paramValue)
        {
            mParamValue = paramValue;
        }

        private MaterialBase.MaterialBase mMaterial;
        private string mName;
        private T mParamValue;
    }

    public class MaterialSetTextureCommand : RenderCommand
    {
        public MaterialSetTextureCommand(MaterialBase.MaterialBase material, TextureBase texture, int location)
        {
            mMaterial = material;
            mLocation = location;
            mTexture = texture;
        }

        public override void Execute()
        {
            mMaterial.SetTextureByIndex(mLocation, mTexture, Sampler.DefaultLinearSampler);
        }

        private MaterialBase.MaterialBase mMaterial;
        private int mLocation;
        private TextureBase mTexture;
    }

    public class BindCommand : RenderCommand
    {
        public BindCommand(IBindable bindable)
        {
            mBindableList.Add(bindable);
        }

        public BindCommand(IBindable[] bindlist)
        {
            mBindableList.AddRange(bindlist);
        }

        public override void Execute()
        {
            foreach (var b in mBindableList)
            {
                b.Bind();
            }
        }

        private List<IBindable> mBindableList = new List<IBindable>();
    }
    
    

    public class DrawWithIndexCommand : RenderCommand
    {
        public DrawWithIndexCommand(VertexArray va, uint[] indices)
        {
            mVA = va;
            mIndices = indices;
        }

        public override void Execute()
        {
            mVA.Bind();

            GL.DrawElements(PrimitiveType.Triangles, mIndices.Length, DrawElementsType.UnsignedInt, mIndices);

            mVA.Unbind();
        }

        protected VertexArray mVA;
        protected uint[] mIndices;
    }

    public class DrawWithoutIndexCommand : RenderCommand
    {
        public DrawWithoutIndexCommand(VertexArray va)
        {
            mVA = va;
        }

        public override void Execute()
        {
            mVA.Bind();
            
            mVA.Unbind();
        }

        protected VertexArray mVA;
    }


}
