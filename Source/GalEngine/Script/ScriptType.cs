using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public enum ScriptType : int
    {
        Unknown,
        Brush,
        Image,
        Voice,
        TextFormat,
        VisualObject,
        Script,
        Animation,
        Animator,
        Scene,
        Config,
        Count
    }
}
