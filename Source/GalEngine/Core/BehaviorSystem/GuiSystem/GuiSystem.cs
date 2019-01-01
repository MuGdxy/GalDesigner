using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiSystem : BehaviorSystem
    {
        public GuiSystem() : base("GuiSystem")
        {
            RequireComponents.AddRequireComponentType<TransformGuiComponent>();
            RequireComponents.AddRequireComponentType<VisualGuiComponent>();
            RequireComponents.AddRequireComponentType<LogicGuiComponent>();
        }

        protected internal override void Excute(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}
