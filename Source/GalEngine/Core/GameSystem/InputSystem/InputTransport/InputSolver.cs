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
    }
}
