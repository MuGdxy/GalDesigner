using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiControl : GameObject
    {
        public GuiControl()
        {
            //add requirement component
            AddComponent<VisualGuiComponent>();
            AddComponent<LogicGuiComponent>();
            AddComponent<TransformGuiComponent>();
        }

        public GuiControl(string name) : base(name)
        {
            //add requirement component
            AddComponent<LogicGuiComponent>();
            AddComponent<VisualGuiComponent>();
            AddComponent<TransformGuiComponent>();
        }

        public void SetShowStatus(bool status)
        {
            var logicComponent = GetComponent<LogicGuiComponent>();

            //status is not changed
            if (status == logicComponent.GetStatus(GuiComponentStatusProperty.Show)) return;

            //update status
            logicComponent.SetStatus(GuiComponentStatusProperty.Show, status);

            //invoke the event
            logicComponent.GetEventSolver(GuiComponentStatusProperty.Show)?.Invoke(
                control: this, eventArg: new GuiComponentShowEvent(DateTime.Now, status));
        }
    }
}
