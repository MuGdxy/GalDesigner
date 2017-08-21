using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class DepthStencilState
    {
        private SharpDX.Direct3D11.DepthStencilStateDescription depthStencilDesc
            = SharpDX.Direct3D11.DepthStencilStateDescription.Default();

        public DepthStencilState(bool depthEnabled = false,
            bool stencilEnabled = false)
        {
            depthStencilDesc.IsDepthEnabled = depthEnabled;
            depthStencilDesc.IsStencilEnabled = stencilEnabled;
        }

        public bool IsDepthEnabled
        {
            get => depthStencilDesc.IsDepthEnabled;
            set => depthStencilDesc.IsDepthEnabled = value;
        }

        public bool IsStencilEnabled
        {
            get => depthStencilDesc.IsStencilEnabled;
            set => depthStencilDesc.IsStencilEnabled = value;
        }

        internal SharpDX.Direct3D11.DepthStencilStateDescription ID3D11DepthStencilStateDescription
        {
            get => depthStencilDesc;
        }
    }
}
