using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class SystemProperty
    {
        private static string specialThanksName = null;

        public const string Width = "Width";
        public const string Height = "Height";
        public const string AppName = "AppName";
        public const string FullScreeen = "FullScreen";
        public const string IsExit = "IsExit";

        public const string TextBrush = "TextBrush";
        public const string TextFormat = "TextFormat";
        public const string BorderBrush = "BorderBrush";
        public const string BackGroundBrush = "BackGroundBrush";
        public const string BackGroundImage = "BackGroundImage";

        public const string Name = "Name";
        public const string Text = "Text";
        public const string PositionX = "PositionX";
        public const string PositionY = "PositionY";
        public const string PositionZ = "PositionZ";
        public const string BorderSize = "BorderSize";
        public const string Opacity = "Opacity";
        public const string IsPresented = "IsPresented";
        public const string Angle = "Angle";
        public const string ScaleX = "ScaleX";
        public const string ScaleY = "ScaleY";

        public const string Code = "Code";
        public const string FilePath = "FilePath";

        public const string Red = "Red";
        public const string Green = "Green";
        public const string Blue = "Blue";
        public const string Alpha = "Alpha";

        public static string SpecialThanksName => specialThanksName;

        static SystemProperty()
        {
            specialThanksName = Encoding.UTF8.GetString(Properties.Resources.SpecialThanks);
        }
    }
}
