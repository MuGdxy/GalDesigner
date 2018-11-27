using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class PackageProvider : GameObject
    {
        private PackageComponent mPackageComponent;
        private LogComponent mLogComponent;

        public PackageProvider(string name) : base(name)
        {
            AddComponent(mPackageComponent = new PackageComponent(name));
            AddComponent(mLogComponent = new LogComponent(name));
            
            mLogComponent.Log("[Log] [object] [time] : [Initialize Package Finish].");
        }

        public PackageBytesResource Load(string name)
        {
            mLogComponent.Log("[Log] [object] [time] : [Load] [name = {0}].", name);

            return mPackageComponent.Load(name);
        }

        public PackageBytesResource LoadRange(string name, int start, int size)
        {
            mLogComponent.Log("[Log] [object] [time] : [LoadRange] [name = {0}] [start = {1}] [size = {2}].",
                name, start, size);
            
            return mPackageComponent.LoadRange(name, start, size);
        }

        public void UnLoad(string name)
        {
            mLogComponent.Log("[Log] [object] [time] : [UnLoad] [name = {0}].", name);

            mPackageComponent.UnLoad(name);
        }
    }
}
