using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{

    public class LogicGuiComponent : GuiComponent
    {
        private readonly GuiComponentStatus mGuiComponentStatus;
        private readonly GuiComponentStatus mGuiComponentEventStatus;
        private readonly Dictionary<string, GuiComponentEventSolver> mGuiComponentEventSolver;

        internal void SetStatus(string statusName, bool status) => 
            mGuiComponentStatus.SetProperty(statusName, status);

        public LogicGuiComponent()
        {
            //logic gui component is used for logic
            //if you do not have any logic requirement, you can set this logic gui component to object
            //because gui behavior system requires at least three component to run(visual, logic and transform gui component)
            BaseComponentType = typeof(LogicGuiComponent);

            //component status means the property we want to use
            //event status means the property if we want to use 
            mGuiComponentStatus = new GuiComponentStatus(GuiComponentStatusProperty.Component);
            mGuiComponentEventStatus = new GuiComponentStatus(GuiComponentStatusProperty.Event);
            mGuiComponentEventSolver = new Dictionary<string, GuiComponentEventSolver>();

            //set default status and solver
            //solver means the way we want to process the event(status must be true)
            foreach (var eventName in GuiComponentStatusProperty.Event)
            {
                mGuiComponentEventSolver.Add(eventName, null);
            }
        }

        public void SetEventStatus(string eventName, bool status) =>
            mGuiComponentStatus.SetProperty(eventName, status);
        
        public void SetEventSolver(string eventName, GuiComponentEventSolver solver) =>
            mGuiComponentEventSolver[eventName] = solver;
        
        public bool GetStatus(string statusName) =>
            mGuiComponentStatus.GetProperty(statusName);

        public bool GetEventStatus(string eventName) =>
            mGuiComponentStatus.GetProperty(eventName);

        public GuiComponentEventSolver GetEventSolver(string eventName)
        {
            if (mGuiComponentEventSolver.ContainsKey(eventName) == false) return null;

            return mGuiComponentEventSolver[eventName];
        }
    }
}
