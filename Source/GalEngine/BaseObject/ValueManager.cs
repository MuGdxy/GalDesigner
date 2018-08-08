using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public delegate void OnChange<T>(T oldValue, T newValue);

    public class ValueManager<T>
    {
        private T oldValue;
        private T newValue;

        public event OnChange<T> Change;

        public T Value { set => newValue = value; get => newValue; }

        public bool IsChange
        {
            get
            {
                if (oldValue == null && newValue == null) return false;
                if (oldValue == null || newValue == null) return true;

                return !oldValue.Equals(newValue);
            }
        }

        public ValueManager(T value = default(T), OnChange<T> onChange = null)
        {
            oldValue = value;
            newValue = value;

            Change += onChange;
        }

        public ValueManager(OnChange<T> onChange)
        {
            oldValue = default(T);
            newValue = default(T);

            Change += onChange;
        }

        public void Update(bool IsTriggerChangeEvent = true)
        {
            if (IsChange is false) return;

            if (IsTriggerChangeEvent is true) Change?.Invoke(oldValue, newValue);

            oldValue = newValue;
        }

        public static implicit operator ValueManager<T>(T Value)
        {
            return new ValueManager<T>(Value);
        }
    }
}
