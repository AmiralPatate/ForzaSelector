using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FH5Data
{
    public class ClassName : Attribute
    {
        public int MaxPI { get; private set; }
        public ClassName(int maxPi) { MaxPI = maxPi; }
    }

    public class RarityName : Attribute
    {
        public string Name { get; private set; }
        public RarityName(string name) { Name = name; }
        public string Shorten(int i)
        {
            if (Name.Length > i) return Name.Substring(0, i).Trim();
            else return Name;
        }
    }

    public static class EnumUtils
    {
        public static int GetMaxPI<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(ClassName), false)
                            .FirstOrDefault() as ClassName;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.MaxPI;
                        }
                    }
                }
            }

            return 0;
        }

        public static string GetName<T>(this T e, bool @short = false, int chars = 1) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(RarityName), false)
                            .FirstOrDefault() as RarityName;

                        if (descriptionAttribute != null)
                        {
                            if (@short) return descriptionAttribute.Shorten(chars);
                            else return descriptionAttribute.Name;
                        }
                    }
                }
            }

            return e.ToString();
        }
    }

    public static class Extensions
    {
        public static Brush Color(this Rarity ra)
        {
            SolidColorBrush COM = new SolidColorBrush(System.Windows.Media.Color.FromRgb(122, 203, 73));
            SolidColorBrush RAR = new SolidColorBrush(System.Windows.Media.Color.FromRgb(45, 205, 249));
            SolidColorBrush EPI = new SolidColorBrush(System.Windows.Media.Color.FromRgb(210, 71, 237));
            SolidColorBrush LEG = new SolidColorBrush(System.Windows.Media.Color.FromRgb(254, 167, 29));
            LinearGradientBrush FOR = new LinearGradientBrush(System.Windows.Media.Color.FromRgb(235, 64, 224), System.Windows.Media.Color.FromRgb(73, 115, 241), 0);
            SolidColorBrush UNK = Brushes.Black;

            switch (ra)
            {
                case Rarity.Common: return COM;
                case Rarity.Rare: return RAR;
                case Rarity.Epic: return EPI;
                case Rarity.Legendary: return LEG;
                case Rarity.ForzaEdition: return FOR;
                default: return UNK;
            }
        }

        public static SolidColorBrush Color(this CarClass cl)
        {
            SolidColorBrush D = new SolidColorBrush(System.Windows.Media.Color.FromRgb(61, 186, 234));
            SolidColorBrush C = new SolidColorBrush(System.Windows.Media.Color.FromRgb(246, 191, 49));
            SolidColorBrush B = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 101, 51));
            SolidColorBrush A = new SolidColorBrush(System.Windows.Media.Color.FromRgb(252, 53, 90));
            SolidColorBrush S1 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(189, 94, 228));
            SolidColorBrush S2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 103, 214));
            SolidColorBrush X = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 64));

            switch (cl)
            {
                default:
                case CarClass.D: return D;
                case CarClass.C: return C;
                case CarClass.B: return B;
                case CarClass.A: return A;
                case CarClass.S1: return S1;
                case CarClass.S2: return S2;
                case CarClass.X: return X;
            }
        }

        public static CarClass ClassFromPi(this int pi)
        {
            if (pi <= CarClass.D.GetMaxPI()) return CarClass.D;
            else if (pi <= CarClass.C.GetMaxPI()) return CarClass.C;
            else if (pi <= CarClass.B.GetMaxPI()) return CarClass.B;
            else if (pi <= CarClass.A.GetMaxPI()) return CarClass.A;
            else if (pi <= CarClass.S1.GetMaxPI()) return CarClass.S1;
            else if (pi <= CarClass.S2.GetMaxPI()) return CarClass.S2;
            else if (pi <= CarClass.X.GetMaxPI()) return CarClass.X;
            return CarClass.D;
        }
    }

    public enum CarClass
    {
        [ClassName(500)]
        D,
        [ClassName(600)]
        C,
        [ClassName(700)]
        B,
        [ClassName(800)]
        A,
        [ClassName(900)]
        S1,
        [ClassName(998)]
        S2,
        [ClassName(999)]
        X
    }

    public enum Rarity
    {
        [RarityName("Common")]
        Common,
        [RarityName("Rare")]
        Rare,
        [RarityName("Epic")]
        Epic,
        [RarityName("Legendary")]
        Legendary,
        [RarityName("Forza Edition")]
        ForzaEdition,
    }

    public enum Setup
    {
        Road, Offroad, Drift, Drag
    }

    public enum Drive
    {
        [Description("Rear-Wheel Drive")]
        RWD,
        [Description("All-Wheel Drive")]
        AWD,
        [Description("Front-Wheel Drive")]
        FWD
    }
}
