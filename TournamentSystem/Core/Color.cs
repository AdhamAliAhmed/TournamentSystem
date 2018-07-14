using System;
using System.Globalization;

namespace TournamentSystem.Core
{
    public struct Color
    {
        public int Alpha { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public string HexValue { get; set; }

        public Color(int alpha, int red, int green, int blue)
        {
            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;

            HexValue = Red.ToString("X2") + Green.ToString("X2") + Blue.ToString("X2");
        }
        public static Color FromHex(string hexValue)
        {
            //Remove # if present
            if (hexValue.IndexOf('#') != -1)
                hexValue = hexValue.Replace("#", "");

            int red = 0;
            int green = 0;
            int blue = 0;

            if (hexValue.Length == 6)
            {
                //#RRGGBB
                red = int.Parse(hexValue.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexValue.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexValue.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            }
            else if (hexValue.Length == 3)
            {
                //#RGB
                red = int.Parse(hexValue[0].ToString() + hexValue[0].ToString(), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexValue[1].ToString() + hexValue[1].ToString(), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexValue[2].ToString() + hexValue[2].ToString(), NumberStyles.AllowHexSpecifier);
            }

            return new Color(255, red, green, blue);
        }

        public static Color FromArgb(int alpha, int red, int green, int blue) =>
            new Color(alpha, red, green, blue);

        public static Color FromRgb(int red, int green, int blue) =>
            new Color(255, red, green, blue);

        public static string ToHex(Color color) =>
            color.HexValue;

    }
}