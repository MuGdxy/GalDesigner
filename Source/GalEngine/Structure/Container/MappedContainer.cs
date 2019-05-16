using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class MappedContainer<T> where T : class
    {
        private Dictionary<string, T> mDictionary;

        public MappedContainer()
        {
            mDictionary = new Dictionary<string, T>();
        }

        public void Add(string name, T value)
        {
            mDictionary[name] = value;
        }

        public void Remove(string name)
        {
            mDictionary.Remove(name);
        }

        public bool Contain(string name)
        {
            return mDictionary.ContainsKey(name);
        }

        public T Get(string name)
        {
            if (mDictionary.ContainsKey(name) == false) return null;

            return mDictionary[name];
        }
    }
}
