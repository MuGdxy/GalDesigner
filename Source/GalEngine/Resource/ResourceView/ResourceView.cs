using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    abstract class ResourceView
    {
        private string name;

        private object resource;

        public ResourceView(string Name)
        {
            name = Name;
            resource = null;

            GlobalResource.SetValue(this);
        }

        public object Resource
        {
            set => resource = value;
            get
            {
                if (resource is null)
                {
                    switch (this)
                    {
                        case ImageView imageView:
                            ResourcePool.SetResourceTo(ref imageView);

                            break;

                        case BrushView brushView:
                            ResourcePool.SetResourceTo(ref brushView);

                            break;

                        case VoiceView voiceView:
                            ResourcePool.SetResourceTo(ref voiceView);

                            break;

                        case TextFormatView textFormatView:
                            ResourcePool.SetResourceTo(ref textFormatView);

                            break;
                        default:
                            break;
                    }
                }

                return resource;
            }
        }

        public string Name => name;
    }
}
