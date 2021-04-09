using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Singleton<T> where T : class ,new()
    {
        protected static T instance = new T();

        public static T Instance
        {
            get => instance;
        }

    }
}
