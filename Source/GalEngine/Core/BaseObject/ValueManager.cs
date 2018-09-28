using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public delegate void OnChange<T>(object owner, T oldValue, T newValue);

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

        public ValueManager(T Value = default(T), OnChange<T> OnChange = null)
        {
            oldValue = Value;
            newValue = Value;

            Change += OnChange;
        }

        public ValueManager(OnChange<T> OnChange)
        {
            oldValue = default(T);
            newValue = default(T);

            Change += OnChange;
        }

        public void Update(object owner, bool isTriggerChangeEvent = true)
        {
            if (IsChange is false) return;

            if (isTriggerChangeEvent is true) Change?.Invoke(owner, oldValue, newValue);

            oldValue = newValue;
        }

        public static implicit operator ValueManager<T>(T value)
        {
            return new ValueManager<T>(value);
        }
    }
}
