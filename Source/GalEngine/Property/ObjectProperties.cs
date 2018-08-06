using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    class PropertyGroup
    {
        public object NewValue = null;
        public object OldValue = null;

        public void FinishUpdate()
        {
            OldValue = NewValue;
        }

        public bool IsChanged
        {
            get
            {
                if (NewValue == null && OldValue == null) return false;
                if (NewValue == null || OldValue == null) return true;

                return !(NewValue.GetType() == OldValue.GetType() && NewValue.Equals(OldValue));
            }
        }
    }

    public abstract class ObjectProperties
    {
        private Dictionary<string, PropertyGroup> properties = new Dictionary<string, PropertyGroup>();

        private bool IsPropertyExist(string propertyName, bool reportIsNotExist = false)
        {
            bool result = properties.ContainsKey(propertyName);

            if (result is false && reportIsNotExist is true) DebugLayer.ReportWarning(Warning.ThePropertyIsNotExist, propertyName);

            return result;
        }

        protected abstract void MakeProperties();

        public void AddProperty(string propertyName, object defaultValue)
        {
            DebugLayer.Assert(IsPropertyExist(propertyName) is true , Warning.ThePropertyIsExist, propertyName);

            properties[propertyName] = new PropertyGroup()
            {
                NewValue = defaultValue,
                OldValue = null
            };
        }

        public void RemoveProperty(string propertyName)
        {
            if (IsPropertyExist(propertyName, true) is false) return;

            properties.Remove(propertyName);
        }

        public void SetProperty(string propertyName, object value)
        {
            properties[propertyName].NewValue = value;
        }

        public object GetProperty(string propertyName)
        {
            if (IsPropertyExist(propertyName, true) is false) return null;

            return properties[propertyName].NewValue;
        }

        public T GetProperty<T>(string propertyName)
        {
            return (T)GetProperty(propertyName);
        }

        public bool IsChanged(string propertyName)
        {
            if (IsPropertyExist(propertyName, true) is false) return false;

            return properties[propertyName].IsChanged;
        }

        public void FinishUpdate()
        {
            foreach (var item in properties)
            {
                item.Value.FinishUpdate();
            }
        }
    }

    class StaticObjectProperties : ObjectProperties
    {
        protected override void MakeProperties()
        {
        }
    }
}
