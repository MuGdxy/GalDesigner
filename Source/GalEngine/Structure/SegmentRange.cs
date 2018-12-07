using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class SegmentRange<T>
    {
        public T Start { get; }
        public T End { get; }
        
        public SegmentRange(T start, T end)
        {
            Start = start;
            End = end;
        }
    }
}
