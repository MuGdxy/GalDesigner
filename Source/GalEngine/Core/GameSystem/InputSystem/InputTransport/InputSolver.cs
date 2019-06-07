using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    
    public class InputSolver
    {
        public Dictionary<string, ButtonInputActionSolvers> ButtonInputAction { get; }
        public Dictionary<string, AxisInputActionSolvers> AxisInputAction { get; }

        public InputSolver()
        {
            ButtonInputAction = new Dictionary<string, ButtonInputActionSolvers>();
            AxisInputAction = new Dictionary<string, AxisInputActionSolvers>();
        }

        public InputSolver(InputSolver other) : base() =>
            Override(other);

        public void Override(InputSolver other)
        {
            foreach (var action in other.ButtonInputAction)
                ButtonInputAction[action.Key] = action.Value;
            foreach (var action in other.AxisInputAction)
                AxisInputAction[action.Key] = action.Value;
        }
    }
}
