using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShaderCompiler
{
    public class CodeGenerator
    {
        public void AddDependency(string DependencyName)
        {
            DependencyList.Add(DependencyName);
        }
                
        protected List<string> DependencyList = new List<string>();

        public string NameSpace = "";

        public string GetCode()
        {
            StringBuilder Builder = new StringBuilder("");

            foreach (var Dependency in DependencyList)
            {
                Builder.AppendLine(string.Format("using {0};", Dependency));
            }
            Builder.AppendLine(NameSpace);
            Builder.AppendLine("{");
            Builder.AppendLine(GetCodeContents());
            Builder.AppendLine("}");

            return Builder.ToString();
        }

        protected virtual string GetCodeContents()
        {
            return "";
        }
    }
}
