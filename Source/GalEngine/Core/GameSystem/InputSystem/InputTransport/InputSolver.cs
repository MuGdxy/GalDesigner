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

        public List<InputMapped> InputMappeds { get; }

        public InputSolver()
        {
            ButtonInputAction = new Dictionary<string, ButtonInputActionSolvers>();
            AxisInputAction = new Dictionary<string, AxisInputActionSolvers>();
            InputMappeds = new List<InputMapped>();
        }

        public InputSolver(InputSolver other) : base() =>
            Override(other);

        public void Override(InputSolver other)
        {
            foreach (var action in other.ButtonInputAction)
                ButtonInputAction[action.Key] = action.Value;
            foreach (var action in other.AxisInputAction)
                AxisInputAction[action.Key] = action.Value;

            InputMappeds.InsertRange(0, other.InputMappeds);
        }

        public string MappedInput(string input)
        {
            foreach (var inputMapped in InputMappeds)
            {
                if (inputMapped.IsMapped(input)) return inputMapped.MappedInput(input);
            }
            
            return input;
        }

        public void Execute(InputAction action)
        {
            switch (action)
            {
                case ButtonInputAction button:
                    ButtonInputAction[MappedInput(button.Name)]?.ForEach(button); break;
                case AxisInputAction axis:
                    AxisInputAction[MappedInput(axis.Name)]?.ForEach(axis); break;
                default: throw new Exception("Invalid Input Action.");
            }
        }
    }
}
