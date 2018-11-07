using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited= true)]
    public class ExposeUI : System.Attribute
    {
        public ExposeUI(string description="")
        {
            this.description = description;
        }

        public string Description => description;

        private string description = "";
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class UIGroup : System.Attribute
    {
        private string groupName = "";

        public string GroupName => groupName;

        public UIGroup(string givenGroupName)
        {
            groupName = givenGroupName;
        }
    }
}
