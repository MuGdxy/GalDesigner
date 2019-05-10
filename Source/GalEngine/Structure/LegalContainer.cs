using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class LegalContainer<T>
    {
        private readonly HashSet<T> mLegal;

        public LegalContainer(params T[] legal)
        {
            mLegal = new HashSet<T>();

            foreach (var element in legal)
            {
                mLegal.Add(element);
            }
        }

        public bool Contain(T value)
        {
            return mLegal.Contains(value);
        }

        public bool Legal(T value)
        {
            return Contain(value);
        }
    }
}
