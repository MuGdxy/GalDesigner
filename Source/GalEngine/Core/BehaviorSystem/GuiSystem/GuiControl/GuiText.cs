using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiText : GuiControl
    {
        private TextGuiComponent mTextGuiComponent;
        private LogicGuiComponent mLogicGuiComponent;

        public GuiText(string name) : base(name)
        {
            AddComponent(mTextGuiComponent = new TextGuiComponent());

            mLogicGuiComponent = GetComponent<LogicGuiComponent>();
        }
    }
}
