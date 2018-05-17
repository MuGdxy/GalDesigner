using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    /// <summary>
    /// Debug Layer Error Type.
    /// </summary>
    public enum ErrorType : int
    {
        /// <summary>
        /// Get Unknown Resource Type in resList.
        /// </summary>
        UnknownResourceType,
        /// <summary>
        /// Resource Parameters are inconsistent. At Line: {0}, FileName: {1}.
        /// </summary>
        InconsistentResourceParameters,
        /// <summary>
        /// The Resource format is not right. At Line: {0}, FileName: {1}.
        /// </summary>
        InvalidResourceFormat,
        /// <summary>
        /// The FilePath {0} is not exist.
        /// </summary>
        FileIsNotExist,
        /// <summary>
        /// The value's type is not support.
        /// </summary>
        InvalidValueType,
        /// <summary>
        /// The file's type is not support. At line: {0}, FileName: {1}.
        /// </summary>
        InvaildFileType,
        /// <summary>
        /// The name is invaild in {0}.
        /// </summary>
        InvaildName,
        /// <summary>
        /// The member value's name {0} is not contained.
        /// </summary>
        InvaildMemberValueName,
        /// <summary>
        /// You can not add the animation: {0} when the animator: {1} is running.
        /// </summary>
        CanNotAddAnimationWhenAnimatorRun,
        /// <summary>
        /// The Animation named {0} must have more than two keyframes.
        /// </summary>
        MoreFramesNeedInAnimation
    }
}
