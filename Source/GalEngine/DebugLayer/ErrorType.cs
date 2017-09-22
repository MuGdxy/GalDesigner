using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public enum ErrorType : int
    {
        /// <summary>
        /// Get Unknown Resource Type in resList.
        /// </summary>
        UnknownResourceType,
        /// <summary>
        /// Resource Parameters are inconsistent. At Line: {0}, FileTag: {1}
        /// </summary>
        InconsistentResourceParameters,
        /// <summary>
        /// The Resource format is not right. At Line: {0}, FileTag: {1}
        /// </summary>
        InvalidResourceFormat,
        /// <summary>
        /// The FilePath {0} is not exist.
        /// </summary>
        FileIsNotExist,
        /// <summary>
        /// The value's type is not support.
        /// </summary>
        InvalidValueType
    }
}
