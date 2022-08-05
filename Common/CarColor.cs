using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Common
{
    public enum ColorFamily
    {
        Greyscale,
        Blue,
        Green,
        Yellow,
        Red,
        Purple,
    }

    public class CarColor
    {
        public ColorFamily Family { get; private set; }
        public int ColorIndex { get; private set; }
        public SolidColorBrush Brush { get; private set; }
        public string Name { get; private set; }

        static CarColor()
        {
            List = new List<CarColor>();

            int i = 0;
            List.Add(new CarColor(ColorFamily.Greyscale, i++, Brushes.Black, "Black"));
            List.Add(new CarColor(ColorFamily.Greyscale, i++, Brushes.DimGray, "Dim Gray"));
            List.Add(new CarColor(ColorFamily.Greyscale, i++, Brushes.DarkGray, "Dark Gray"));
            List.Add(new CarColor(ColorFamily.Greyscale, i++, Brushes.Silver, "Silver"));
            List.Add(new CarColor(ColorFamily.Greyscale, i++, Brushes.LightGray, "Light Gray"));
            List.Add(new CarColor(ColorFamily.Greyscale, i++, Brushes.White, "White"));

            i = 0;
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.Navy, "Dark Blue"));
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.Blue, "Blue"));
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.RoyalBlue, "Royal Blue"));
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.DodgerBlue, "Dodger Blue"));
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.DeepSkyBlue, "Deep Sky Blue"));
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.LightSkyBlue, "Light Sky Blue"));
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.Cyan, "Cyan"));
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.Turquoise, "Turquoise"));
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.PaleTurquoise, "Pale Turquoise"));
            List.Add(new CarColor(ColorFamily.Blue, i++, Brushes.Teal, "Teal"));

            i = 0;
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.SeaGreen, "Sea Green"));
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.DarkGreen, "Dark Green"));
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.ForestGreen, "Forest Green"));
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.SpringGreen, "Spring Green"));
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.LimeGreen, "Lime Green"));
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.Lime, "Lime"));
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.PaleGreen, "Pale Green"));
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.LawnGreen, "Lawn Green"));
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.YellowGreen, "Yellow Green"));
            List.Add(new CarColor(ColorFamily.Green, i++, Brushes.DarkOliveGreen, "Dark Olive Green"));

            i = 0;
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.GreenYellow, "Green Yellow"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.OrangeRed, "Orange Red"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.DarkOrange, "Dark Orange"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.Orange, "Orange"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.Gold, "Gold"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.Yellow, "Yellow"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.LemonChiffon, "Lemon Chiffon"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.Khaki, "Khaki"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.DarkKhaki, "Dark Khaki"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.Peru, "Peru"));
            List.Add(new CarColor(ColorFamily.Yellow, i++, Brushes.SaddleBrown, "Saddle Brown"));

            i = 0;
            List.Add(new CarColor(ColorFamily.Red, i++, Brushes.DarkRed, "Dark Red"));
            List.Add(new CarColor(ColorFamily.Red, i++, Brushes.Red, "Red"));
            List.Add(new CarColor(ColorFamily.Red, i++, Brushes.Crimson, "Crimson"));
            List.Add(new CarColor(ColorFamily.Red, i++, Brushes.Brown, "Brown"));

            i = 0;
            List.Add(new CarColor(ColorFamily.Purple, i++, Brushes.Indigo, "Indigo"));
            List.Add(new CarColor(ColorFamily.Purple, i++, Brushes.DarkViolet, "Dark Violet"));
            List.Add(new CarColor(ColorFamily.Purple, i++, Brushes.Magenta, "Magenta"));
            List.Add(new CarColor(ColorFamily.Purple, i++, Brushes.DeepPink, "Deep Pink"));
            List.Add(new CarColor(ColorFamily.Purple, i++, Brushes.Violet, "Violet"));
            List.Add(new CarColor(ColorFamily.Purple, i++, Brushes.HotPink, "Hot Pink"));
            List.Add(new CarColor(ColorFamily.Purple, i++, Brushes.Pink, "Pink"));

            //Transparent,
        }

        public const int CarColorIndexMax = 8;
        private CarColor(ColorFamily fam, int index, SolidColorBrush brush, string name)
        {
            Family = fam;
            ColorIndex = index;
            Brush = brush;
            Name = name;
        }

        public static CarColor ByName(string name)
        {
            return List.Where(col => col.Name == name).FirstOrDefault();
        }

        public static List<CarColor> List { get; private set; }
    }
}
