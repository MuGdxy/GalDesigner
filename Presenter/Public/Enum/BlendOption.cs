using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public enum BlendOption
    {
        Zero = 1,
        One = 2,
        SourceColor = 3,
        InverseSourceColor = 4,
        SourceAlpha = 5,
        InverseSourceAlpha = 6,
        DestinationAlpha = 7,
        InverseDestinationAlpha = 8,
        DestinationColor = 9,
        InverseDestinationColor = 10,
        SourceAlphaSaturate = 11,
        BlendFactor = 14,
        InverseBlendFactor = 15,
        SecondarySourceColor = 16,
        InverseSecondarySourceColor = 17,
        SecondarySourceAlpha = 18,
        InverseSecondarySourceAlpha = 19
    }
}
