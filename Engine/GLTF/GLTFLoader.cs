using GLTF.V2;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace GLTF
{
    public class GLTFLoader
    {
        public static GLTF_V2 LoadGLTFV2(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                var result = JsonSerializer.Deserialize<GLTF_V2>(json);
                result.Path = path;
                return result;
            }
            catch (Exception e)
            {
                //
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<GLTF_V2> LoadGLTF2Async(string path)
        {
            try
            {
                var json = await File.ReadAllBytesAsync(path);
                using (var m = new MemoryStream(json))
                {
                    GLTF_V2 result = await JsonSerializer.DeserializeAsync<GLTF_V2>(m);
                    result.Path = path;
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
    }
}
