using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Runtime.Graphics
{
    public enum PrimitiveType : uint
    {
        TriangleList = 4
    }

    public struct InputElement
    {
        public int Size { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }

        public InputElement(string name, int index, int size)
        {
            Name = name;
            Index = index;
            Size = size;
        }
        
    }

    public class GpuInputLayout : IDisposable
    {
        private SharpDX.Direct3D11.InputLayout mInputLayout;

        protected GpuDevice GpuDevice { get; }

        internal SharpDX.Direct3D11.InputElement[] InputElements { get; }
        internal SharpDX.Direct3D11.InputLayout InputLayout => mInputLayout;

        public GpuInputLayout(GpuDevice device, InputElement[] inputElements, GpuVertexShader vertexShader)
        {
            GpuDevice = device;
            InputElements = new SharpDX.Direct3D11.InputElement[inputElements.Length];

            for (int i = 0; i < InputElements.Length; i++)
            {
                var element = inputElements[i];
                var inputElement = new SharpDX.Direct3D11.InputElement();

                inputElement.SemanticName = element.Name;
                inputElement.SemanticIndex = element.Index;
                inputElement.Slot = 0;
                inputElement.Classification = SharpDX.Direct3D11.InputClassification.PerVertexData;
                inputElement.InstanceDataStepRate = 0;

                if (i == 0) inputElement.AlignedByteOffset = 0;
                else inputElement.AlignedByteOffset = InputElements[i - 1].AlignedByteOffset + inputElements[i - 1].Size;

                switch (inputElements[i].Size)
                {
                    case 4:
                        inputElement.Format = SharpDX.DXGI.Format.R32_Float; break;
                    case 8:
                        inputElement.Format = SharpDX.DXGI.Format.R32G32_Float; break;
                    case 12:
                        inputElement.Format = SharpDX.DXGI.Format.R32G32B32_Float; break;
                    case 16:
                        inputElement.Format = SharpDX.DXGI.Format.R32G32B32A32_Float; break;
                    default:
                        break;
                }

                InputElements[i] = inputElement;
            }

            mInputLayout = new SharpDX.Direct3D11.InputLayout(GpuDevice.Device, vertexShader.ByteCode, InputElements);
        }

        ~GpuInputLayout() => Dispose();

        public void Dispose()
        {
            SharpDX.Utilities.Dispose(ref mInputLayout);
        }
    }
}
