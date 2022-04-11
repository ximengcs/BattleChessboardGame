//------------------------------------------------------------
// 2021-11-10
// 带颜色的Log，好看
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public static partial class Log
    {
        public static Style GY = new Style(nameof(Color.green), nameof(Color.yellow));
        public static Style YG = new Style(nameof(Color.yellow), nameof(Color.green));
        public static Style RG = new Style(nameof(Color.red), nameof(Color.green));
        public static Style GR = new Style(nameof(Color.green), nameof(Color.red));
        public static Style RY = new Style(nameof(Color.red), nameof(Color.yellow));
        public static Style YR = new Style(nameof(Color.yellow), nameof(Color.red));
        public static Style BG = new Style(nameof(Color.blue), nameof(Color.green));
        public static Style GB = new Style(nameof(Color.green), nameof(Color.blue));
        public static Style CB = new Style(nameof(Color.cyan), nameof(Color.blue));
        public static Style BC = new Style(nameof(Color.blue), nameof(Color.cyan));
        public static Style CG = new Style(nameof(Color.cyan), nameof(Color.green));
        public static Style GC = new Style(nameof(Color.green), nameof(Color.cyan));
        public static Style CY = new Style(nameof(Color.cyan), nameof(Color.yellow));
        public static Style YC = new Style(nameof(Color.yellow), nameof(Color.cyan));
        public static Style MC = new Style(nameof(Color.magenta), nameof(Color.cyan));
        public static Style CM = new Style(nameof(Color.cyan), nameof(Color.magenta));

        public static void Debug(string message, Style style, string prefix)
        {
            Debug("<color=" + style.PrefixColor + ">[{0}] </color><color=" + style.ContentColor + ">{1}</color>", prefix, message);
        }

        public class Style
        {
            internal string PrefixColor;
            internal string ContentColor;

            internal Style(string prefixColor, string contentColor)
            {
                PrefixColor = prefixColor;
                ContentColor = contentColor;
            }
        }
    }
}
