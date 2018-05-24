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

        public static string Width => "Width";
        public static string Height => "Height";
        public static string AppName => "AppName";
        public static string FullScreeen => "FullScreen";
        
        public static string TextBrush => "TextBrush";
        public static string TextFormat => "TextFormat";
        public static string BorderBrush => "BorderBrush";
        public static string BackGroundBrush => "BackGroundBrush";
        public static string BackGroundImage => "BackGroundImage";

        public static string Text => "Text";
        public static string PositionX => "PositionX";
        public static string PositionY => "PositionY";
        public static string BorderSize => "BorderSize";
        public static string Opacity => "Opacity";
        public static string IsPresented => "IsPresented";
        public static string Angle => "Angle";
        public static string ScaleX => "ScaleX";
        public static string ScaleY => "ScaleY";

        public static string SpecialThanksName => specialThanksName;

        static SystemProperty()
        {
            specialThanksName = Encoding.UTF8.GetString(Properties.Resources.SpecialThanks);
        }
    }
}
