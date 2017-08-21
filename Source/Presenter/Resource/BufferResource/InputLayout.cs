using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Presenter
{


    public partial class InputLayout 
    {
        private SharpDX.Direct3D11.InputElement[] layoutDesc;

        public InputLayout(Element[] elements)
        {
            layoutDesc = new SharpDX.Direct3D11.InputElement[elements.Length];

            int bit_off = 0;

            for (int i = 0; i < elements.Length; i++)
            {
                layoutDesc[i].SemanticName = elements[i].Tag;

                int add_off = 0;

                switch (elements[i].Size)
                {
                    case ElementSize.eFloat1:
                        add_off = 4;
                        layoutDesc[i].Format = SharpDX.DXGI.Format.R8G8B8A8_UInt;
                        break;
                    case ElementSize.eFloat2:
                        add_off = 8;
                        layoutDesc[i].Format = SharpDX.DXGI.Format.R32G32_Float;
                        break;
                    case ElementSize.eFloat3:
                        add_off = 12;
                        layoutDesc[i].Format = SharpDX.DXGI.Format.R32G32B32_Float;
                        break;
                    case ElementSize.eFlaot4:
                        add_off = 16;
                        layoutDesc[i].Format = SharpDX.DXGI.Format.R32G32B32A32_Float;
                        break;
                    default:
                        break;
                }

                layoutDesc[i].Classification = SharpDX.Direct3D11.InputClassification.PerVertexData;

                layoutDesc[i].AlignedByteOffset = bit_off;
                bit_off += add_off;

            }


        }


        internal SharpDX.Direct3D11.InputElement[] Elements => layoutDesc;
    }
}
