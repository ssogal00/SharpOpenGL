﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace Core
{
    public class TextureConverter : Singleton<TextureConverter>
    {
        public async Task<bool> ConvertTexturesInFolderAsync(string folder)
        {
            bool bSuccess = true;
            foreach (var file in Directory.EnumerateFiles(folder))
            {
                if(file.EndsWith(".tga", StringComparison.InvariantCultureIgnoreCase) 
                   || file.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase)
                   || file.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase)
                   || file.EndsWith("jpeg", StringComparison.InvariantCultureIgnoreCase))
                {
                    bool bResult = await ConvertTexture2DAsync(file);
                    bSuccess &= bResult;
                }
            }

            return bSuccess;
        }

        public async Task<bool> ConvertTexture2DAsync(string path)
        {
            var outputFile= Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)) + ".dds";
            if (File.Exists(outputFile))
            {
                return true;
            }

            if (path.EndsWith(".tga", StringComparison.InvariantCultureIgnoreCase) 
                || path.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase)
                || path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
            {
                int exitcode = await Task.Factory.StartNew(()=>
                {
                    //
                    ProcessStartInfo info = new ProcessStartInfo();
                    var p = new Process();
                    p.StartInfo.FileName = m_NVTTToolPath;
                    // bc7
                    p.StartInfo.ArgumentList.Add("-f");
                    p.StartInfo.ArgumentList.Add("23");
                    // texture2d
                    p.StartInfo.ArgumentList.Add("-t");
                    p.StartInfo.ArgumentList.Add("0");
                    p.StartInfo.ArgumentList.Add(path);
                    p.Start();
                    p.WaitForExit(10000);
                    
                    return p.ExitCode;
                });

                return exitcode == 0;
            }

            return false;
        }

        //
        private string m_NVTTToolPath = "nvtt_export.exe";
    }
}
