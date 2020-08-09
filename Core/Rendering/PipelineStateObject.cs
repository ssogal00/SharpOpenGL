using Core.Buffer;
using Core.OpenGLShader;
using System;

namespace Core.Rendering
{
    public class PipelineStateObject : IEquatable<PipelineStateObject>
    {
        private OpenGLBuffer vertexBuffer;
        private OpenGLBuffer indexBuffer;
        private Shader vertexShader;
        private Shader pixelShader;
        private MaterialBase.MaterialBase material;

        public virtual void BindState()
        {
            material.Bind();
            vertexBuffer.Bind();
            indexBuffer.Bind();
        }

        public bool Equals(PipelineStateObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(vertexBuffer, other.vertexBuffer) && Equals(indexBuffer, other.indexBuffer) && Equals(vertexShader, other.vertexShader) && Equals(pixelShader, other.pixelShader);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PipelineStateObject) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (vertexBuffer != null ? vertexBuffer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (indexBuffer != null ? indexBuffer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (vertexShader != null ? vertexShader.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (pixelShader != null ? pixelShader.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
