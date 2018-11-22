// ReSharper disable once CheckNamespace
namespace LogPrinter
{
    /// <summary>
    /// key setting. map the output of key
    /// </summary>
    public abstract class KeySetting
    {
        internal string GetValue()
        {
            if (EnableFormat == false) return MapMethod(this);
            return '[' + MapMethod(this) + ']';
        }
       
        protected abstract string MapMethod(KeySetting setting);
     
        public bool EnableFormat { get; set; }
        
        public KeySetting()
        {
            EnableFormat = true;
        }
    }
}