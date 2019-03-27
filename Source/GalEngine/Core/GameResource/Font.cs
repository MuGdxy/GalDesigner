using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpFont;
using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    public class CharacterCodeMetrics : IDisposable
    {
        private Texture2D mTexture;

        public Texture2D Texture { get => mTexture; internal set { mTexture = value; } }

        public int Advance { get; internal set; }
        public int HoriBearingX { get; internal set; }
        public int HoriBearingY { get; internal set; }

        ~CharacterCodeMetrics() => Dispose();

        public void Dispose()
        {
            Utility.Dispose(ref mTexture);
        }
    }

    public class Font : IDisposable
    {
        private static readonly Library mLibrary = new Library();

        private Face mFace;
        private GpuDevice mDevice;

        private Dictionary<char, CharacterCodeMetrics> mCharacterIndex;

        public int Size { get; }

        public Font(int size, byte[] fontData, GpuDevice device)
        {
            mFace = new Face(mLibrary, fontData, 0);
            mDevice = device;
            Size = size;

            mFace.SetPixelSizes(0, (uint)size);
            mFace.SelectCharmap(SharpFont.Encoding.Unicode);
        }

        ~Font() => Dispose();

        public void FreeCache()
        {
            foreach (var characterCode in mCharacterIndex) characterCode.Value.Dispose();

            mCharacterIndex.Clear();
        }

        public CharacterCodeMetrics GetCharacterCodeMetrics(char character)
        {
            if (mCharacterIndex.ContainsKey(character) == true) return mCharacterIndex[character];

            var index = mFace.GetCharIndex(character);

            mFace.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);

            if (mFace.Glyph.Format != GlyphFormat.Bitmap) mFace.Glyph.RenderGlyph(RenderMode.Normal);

            var codeMetrics = new CharacterCodeMetrics();

            codeMetrics.Advance = (mFace.Glyph.Advance.X.Value >> 6);
            codeMetrics.HoriBearingX = (mFace.Glyph.Metrics.HorizontalBearingX.Value >> 6);
            codeMetrics.HoriBearingY = (mFace.Glyph.Metrics.HorizontalBearingY.Value >> 6);
            codeMetrics.Texture = new Texture2D(
                new Size<int>(mFace.Glyph.Bitmap.Width, mFace.Glyph.Bitmap.Rows),
                PixelFormat.Alpha8bit, mFace.Glyph.Bitmap.BufferData);

            mCharacterIndex.Add(character, codeMetrics);

            return codeMetrics;
        }

        public void Dispose()
        {
            FreeCache();
        }
    }
}
