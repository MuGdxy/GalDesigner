using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{

    public class LogicGuiComponent : GuiComponent
    {
        private Dictionary<string, bool> mGuiComponentEventStatus;
        private Dictionary<string, GuiComponentEventSolver> mGuiComponentEventSolver;

        public LogicGuiComponent()
        {
            //logic gui component is used for logic
            //if you do not have any logic requirement, you can set this logic gui component to object
            //because gui behavior system requires at least three component to run(visual, logic and transform gui component)
            BaseComponentType = typeof(LogicGuiComponent);

            mGuiComponentEventStatus = new Dictionary<string, bool>();
            mGuiComponentEventSolver = new Dictionary<string, GuiComponentEventSolver>();

            //set default status and solver
            //status means the property if we want to use 
            //solver means the way we want to process the event(status must be true)
            foreach (var eventName in StringProperty.GuiComponentEvent.Array)
            {
                mGuiComponentEventStatus.Add(eventName, false);
                mGuiComponentEventSolver.Add(eventName, null);
            }
        }

        public void SetEventStatus(string eventName, bool status)
        {
            mGuiComponentEventStatus[eventName] = status;
        }

        public void SetEventSolver(string eventName, GuiComponentEventSolver solver)
        {
            mGuiComponentEventSolver[eventName] = solver;
        }

        public bool GetEventStatus(string eventName)
        {
            if (mGuiComponentEventStatus.ContainsKey(eventName) == false) return false;

            return mGuiComponentEventStatus[eventName];
        }

        public GuiComponentEventSolver GetEventSolver(string eventName)
        {
            if (mGuiComponentEventSolver.ContainsKey(eventName) == false) return null;

            return mGuiComponentEventSolver[eventName];
        }
    }
}
