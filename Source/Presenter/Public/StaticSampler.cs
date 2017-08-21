using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class StaticSampler
    {
        private TextureAddressMode addressU;
        private TextureAddressMode addressV;
        private TextureAddressMode addressW;

        private TextureFilter filter;

        public StaticSampler(TextureAddressMode AddressU,
            TextureAddressMode AddressV, TextureAddressMode AddressW,
            TextureFilter Filter = TextureFilter.MinMagMipLinear)
        {
            addressU = AddressU;
            addressV = AddressV;
            addressW = AddressW;

            filter = Filter;
        }

        public StaticSampler(TextureAddressMode addressUVW,
            TextureFilter Filter = TextureFilter.MinMagMipLinear)
        {
            addressU = addressV = addressW = addressUVW;

            filter = Filter;
        }

        public TextureAddressMode AddressU
        {
            get => addressU;
            set => addressU = value;
        }

        public TextureAddressMode AddressV
        {
            get => addressV;
            set => addressV = value;
        }

        public TextureAddressMode AddressW
        {
            get => addressW;
            set => addressW = value;
        }

        public TextureFilter Filter
        {
            get => filter;
            set => filter = value;
        }

        public TextureAddressMode AddressUVW
        {
            set
            {
                addressU = addressV = addressW = value;
            }
        }



    }
}
