using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Size<T> where T : IEquatable<T>
    {
        public T Width { get; }
        public T Height { get; }

        public Size(T width, T height)
        {
            Width = width;
            Height = height;
        }

        public Size()
        {
            Width = default;
            Height = default;
        }

        public static bool operator == (Size<T> left, Size<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator != (Size<T> left, Size<T> right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return obj is Size<T> size &&
                   EqualityComparer<T>.Default.Equals(Width, size.Width) &&
                   EqualityComparer<T>.Default.Equals(Height, size.Height);
        }

        public override int GetHashCode()
        {
            var hashCode = 859600377;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Width);
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Height);
            return hashCode;
        }
    }
}
