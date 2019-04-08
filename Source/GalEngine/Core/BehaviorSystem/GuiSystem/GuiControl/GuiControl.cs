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
            AddComponent<VisualGuiComponent>();
            AddComponent<LogicGuiComponent>();
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
            if (logicComponent.GetEventStatus(GuiComponentStatusProperty.Show) == true)
                logicComponent.GetEventSolver(GuiComponentStatusProperty.Show)?.Invoke(
                    control: this, eventArg: new GuiComponentShowEvent(DateTime.Now, status));

            //solve some status are effect when we hide control
            if (status == false)
            {
                //for drag event, when we hide control, we need disable it
                if (logicComponent.GetEventStatus(GuiComponentStatusProperty.Drag) == true &&
                    logicComponent.GetStatus(GuiComponentStatusProperty.Drag) == true)
                {
                    logicComponent.SetStatus(GuiComponentStatusProperty.Drag, false);
                    logicComponent.GetEventSolver(GuiComponentStatusProperty.Drag)?.
                        Invoke(this, new GuiComponentDragEvent(DateTime.Now, false));
                }

                //for hover event, when we hide control, we need disable it
                if (logicComponent.GetEventStatus(GuiComponentStatusProperty.Hover) == true &&
                    logicComponent.GetStatus(GuiComponentStatusProperty.Hover) == true)
                {
                    logicComponent.SetStatus(GuiComponentStatusProperty.Hover, false);
                    logicComponent.GetEventSolver(GuiComponentStatusProperty.Hover)?.
                        Invoke(this, new GuiComponentHoverEvent(DateTime.Now, false));
                }
            }
        }
    }
}
