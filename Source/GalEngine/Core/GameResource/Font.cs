using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpFont;
using GalEngine.Runtime.Graphics;

namespace GalEngine.GameResource
{
    public class CharacterCodeMetrics : IDisposable
    {
        private Image mTexture;

        public Image Texture { get => mTexture; internal set { mTexture = value; } }

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
        private static readonly Font mInternalFont = new Font("Internal.UbuntuMono-R.Font", 17, Properties.Resources.UbuntuMono_R);

        private Face mFace;
        private readonly GpuDevice mDevice;

        private readonly Dictionary<char, CharacterCodeMetrics> mCharacterIndex;

        internal Face FontFace => mFace;
        internal GpuDevice GpuDevice => mDevice;

        public int Size { get; }

        public string Name { get; }

        public static Font Default => mInternalFont;

        public Font(string name, int size, byte[] fontData) : this(name, size, fontData, GameSystems.GpuDevice)
        {

        }

        public Font(string name, int size, byte[] fontData, GpuDevice device)
        {
            //create font
            mFace = new Face(mLibrary, fontData, 0);
            mDevice = device;
            Size = size;
            Name = name;

            //set font size and encoding
            mFace.SetPixelSizes(0, (uint)size);
            mFace.SelectCharmap(SharpFont.Encoding.Unicode);

            mCharacterIndex = new Dictionary<char, CharacterCodeMetrics>();
        }

        ~Font() => Dispose();

        public void FreeCache()
        {
            //dispose the CharacterCodeMetrics that cache in the font
            foreach (var characterCode in mCharacterIndex) characterCode.Value.Dispose();

            mCharacterIndex.Clear();
        }

        public CharacterCodeMetrics GetCharacterCodeMetrics(char character)
        {
            //search it in cache, if it is found return it
            //if we do not find, we create it and cache it
            if (mCharacterIndex.ContainsKey(character) == true) return mCharacterIndex[character];

            //find index(see more in freetype)
            var index = mFace.GetCharIndex(character);

            //nonsupport character, we return space
            if (index == 0)
            {
                index = mFace.GetCharIndex(' ');

                LogEmitter.Apply(LogLevel.Warning, "There are some nonsupport character used in [{0}] font.", Name);
            }

            //load glyph
            mFace.LoadGlyph(index, LoadFlags.Default, LoadTarget.Normal);

            //test the format of glyph, if it is not bitmap, we convert its format to bitmap
            if (mFace.Glyph.Format != GlyphFormat.Bitmap) mFace.Glyph.RenderGlyph(RenderMode.Normal);

            //create metrices
            var codeMetrics = new CharacterCodeMetrics();

            codeMetrics.Advance = (mFace.Glyph.Advance.X.Value >> 6);
            codeMetrics.HoriBearingX = (mFace.Glyph.Metrics.HorizontalBearingX.Value >> 6);
            codeMetrics.HoriBearingY = (mFace.Glyph.Metrics.HorizontalBearingY.Value >> 6);
            codeMetrics.Texture = mFace.Glyph.Bitmap.Buffer != IntPtr.Zero ? new Image(
                new Size<int>(mFace.Glyph.Bitmap.Width, mFace.Glyph.Bitmap.Rows),
                PixelFormat.Alpha8bit, mFace.Glyph.Bitmap.BufferData) : 
                new Image(new Size<int>(0,0), PixelFormat.Alpha8bit);

            mCharacterIndex.Add(character, codeMetrics);

            return codeMetrics;
        }

        public void Dispose()
        {
            //dispose the CharacterCodeMetrics that cache in the font
            FreeCache();

            //dispose the font class
            Utility.Dispose(ref mFace);
        }
    }
}
