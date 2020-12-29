using System;
using System.Collections.Generic;
using System.Text;
using GLTF.V2;
using System.IO;

namespace GLTF
{
    public class GLTFMesh
    {

        public GLTFMesh(GLTF_V2 gltf)
        {
            for (int i = 0; i < gltf.buffers.Count; ++i)
            {
                var path= gltf.buffers[i].uri;
                byte[] result = File.ReadAllBytes(path);
                mBufferDatas.Add(result);
            }


            for (int i = 0; i < gltf.accessors.Count; ++i)
            {
                gltf.accessors[i].componentType
            }

            for (int i = 0; i < gltf.bufferViews.Count; ++i)
            {
                var index = gltf.bufferViews[i].buffer;
                var offset = gltf.bufferViews[i].byteOffset;
                var length = gltf.bufferViews[i].byteLength;
                

            }
        }

        private List<byte[]> mBufferDatas = new List<byte[]>();
    }
}
