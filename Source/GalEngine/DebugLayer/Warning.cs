using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public enum Warning
    {
        /// <summary>
        /// The Property "{0}" is exist.
        /// </summary>
        ThePropertyIsExist,
        /// <summary>
        /// The Property "{0}" is not exist.
        /// </summary>
        ThePropertyIsNotExist,
        /// <summary>
        /// The Property "{0}" 's type is not right, need {1} but set {2}.
        /// </summary>
        ThePropertyTypeIsNotRight,
        /// <summary>
        /// The Window has been created.
        /// </summary>
        TheWindowIsCreated,
    }

    public static partial class DebugLayer
    {
        private static string WarningHead => "Warning: ";

        private static void MakeWarnings()
        {
            warnings.Add(Warning.ThePropertyIsExist, "The Property \"{0}\" is exist.");
            warnings.Add(Warning.ThePropertyIsNotExist, "The Property \"{0}\" is not exist.");
            warnings.Add(Warning.ThePropertyTypeIsNotRight, "The Property \"{0}\" 's type is not right, need {1} but set {2}.");
            warnings.Add(Warning.TheWindowIsCreated, "The Window has been created.");
        }
    }
}
