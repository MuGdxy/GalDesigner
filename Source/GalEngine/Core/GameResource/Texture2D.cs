using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    public class Texture2D : IDisposable
    {
        private GpuDevice mDevice;

        private GpuTexture2D mTexture;
        private GpuRenderTarget mRenderTarget;
        private GpuResourceUsage mResourceUsage;

        internal GpuTexture2D GpuTexture => mTexture;
        internal GpuRenderTarget GpuRenderTarget => mRenderTarget == null ? mRenderTarget = new GpuRenderTarget(mDevice, mTexture) : mRenderTarget;
        internal GpuResourceUsage GpuResourceUsage => mResourceUsage == null ? mResourceUsage = new GpuResourceUsage(mDevice, mTexture) : mResourceUsage;

        public Size<int> Size => mTexture.Size;
        public int SizeInBytes => mTexture.SizeInBytes;
        public PixelFormat PixelFormat => (PixelFormat)mTexture.PixelFormat;

        public Texture2D(Size<int> size, PixelFormat pixelFormat) : this(size, pixelFormat, GameSystems.GpuDevice)
        {

        }

        public Texture2D(Size<int> size, PixelFormat pixelFormat, byte[] data) : this(size, pixelFormat)
        {
            mTexture.Update(data);
        }
        
        public Texture2D(Size<int> size, PixelFormat pixelFormat, GpuDevice device)
        {
            mDevice = device;

            mTexture = new GpuTexture2D(size, (GpuPixelFormat)pixelFormat, device,
               new GpuResourceInfo(GpuBindUsage.ShaderResource | GpuBindUsage.RenderTarget));
        }

        ~Texture2D() => Dispose();

        public void Dispose()
        {
            Utility.Dispose(ref mResourceUsage);
            Utility.Dispose(ref mRenderTarget);
            Utility.Dispose(ref mTexture);
        }
    }
}
