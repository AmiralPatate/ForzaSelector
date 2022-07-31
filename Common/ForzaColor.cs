using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Common
{
    public partial class ForzaColor
    {
        private static List<ForzaColor> SavedColors = new List<ForzaColor>();
        private const string FILEPATH = @"data\mycolors.csv";
        
        public static void Save()
        {
            File.WriteAllLines(FILEPATH, SerialiseToCSV());
        }

        public static void Load()
        {
            if (File.Exists(FILEPATH))
                UnserialiseFromCSV(File.ReadAllLines(FILEPATH));
        }

        public static void Add(ForzaColor fc, bool clone = true)
        {
            if (clone)
                SavedColors.Add(fc.Clone());
            else
                SavedColors.Add(fc);
        }
        public static void Remove(ForzaColor fc)
        {
            SavedColors.RemoveAll(c => c.Equals(fc));
        }
        public static List<ForzaColor> GetAll() { return SavedColors.OrderBy(x => x.Hue).ThenBy(x => x.Value).ThenBy(x => x.Saturation).ToList(); }

        public static string[] SerialiseToCSV()
        {
            return SavedColors.Select(x => x.ToCSV()).ToArray();
        }

        public static void UnserialiseFromCSV(string[] csv)
        {
            SavedColors.Clear();
            foreach (var line in csv)
            {
                Add(new ForzaColor(line));
            }
        }
    }

    public partial class ForzaColor
    {
        private const char CSVSEP = ',';
        public double Hue { get; private set; }
        public double Saturation { get; private set; }
        public double Value { get; private set; }
        public string Name { get; set; }

        ///https://www.w3.org/TR/WCAG20/#relativeluminancedef
        ///Rs = Rb/255
        ///Gs = Gb/255
        ///Bs = Bb/255
        ///if Rs <= 0.03928 then R = Rs/12.92 else R = ((Rs+0.055)/1.055) ^ 2.4
        ///if Gs <= 0.03928 then G = Gs/12.92 else G = ((Gs+0.055)/1.055) ^ 2.4
        ///if Bs <= 0.03928 then B = Bs/12.92 else B = ((Bs+0.055)/1.055) ^ 2.4
        ///L = 0.2126 * R + 0.7152 * G + 0.0722 * B
        public double RelativeLuminance
        {
            get
            {
                byte Rb, Gb, Bb;
                GetBrush(out Rb, out Gb, out Bb);

                double Rs = Rb / 255.0;
                double Gs = Gb / 255.0;
                double Bs = Bb / 255.0;

                double R, G, B;
                if (Rs <= 0.03928) R = Rs / 12.92;
                else R = Math.Pow((Rs + 0.055) / 1.055, 2.4);
                if (Gs <= 0.03928) G = Gs / 12.92;
                else G = Math.Pow((Gs + 0.055) / 1.055, 2.4);
                if (Bs <= 0.03928) B = Bs / 12.92;
                else B = Math.Pow((Bs + 0.055) / 1.055, 2.4);

                return 0.2126 * R + 0.7152 * G + 0.0722 * B;
            }
        }

        public ForzaColor(byte r, byte g, byte b) { Set(r, g, b); }
        public ForzaColor(byte r, byte g, byte b, string name) : this(r, g, b) { Name = name; }
        public ForzaColor(string csv) { FromCSV(csv); }

        public ForzaColor Clone()
        {
            ForzaColor clone = new ForzaColor(0, 0, 0);
            clone.Hue = Hue;
            clone.Saturation = Saturation;
            clone.Value = Value;
            clone.Name = Name;
            return clone;
        }

        public bool Equals(ForzaColor other)
        {
            var result = Name == other.Name && Saturation == other.Saturation && Value == other.Value;
            if (result)
            {
                if (double.IsNaN(Hue)) result &= double.IsNaN(other.Hue);
                else result &= Hue == other.Hue;
            }
            return result;
        }

        private void Set(Color color) { Set(color.R, color.G, color.B); }
        private void Set(byte r, byte g, byte b) { RGBtoHSV(r, g, b); }

        public void FromCSV(string csv)
        {
            string[] bits = csv.Split(CSVSEP);
            Name = bits[0];
            Hue = double.Parse(bits[1], CultureInfo.InvariantCulture);
            Saturation = double.Parse(bits[2], CultureInfo.InvariantCulture);
            Value = double.Parse(bits[3], CultureInfo.InvariantCulture);
        }
        public string ToCSV()
        {
            return Name.Replace(CSVSEP.ToString(), "") + CSVSEP + Hue.ToString("F3", CultureInfo.InvariantCulture) + CSVSEP + Saturation.ToString("F3", CultureInfo.InvariantCulture) + CSVSEP + Value.ToString("F3", CultureInfo.InvariantCulture);
        }

        public SolidColorBrush GetBrush(out byte R, out byte G, out byte B)
        {
            double H = Hue * 360;
            double S = Saturation;
            double V = Value;

            double C = V * S;
            double X = C * (1 - Math.Abs((H / 60) % 2 - 1));
            double m = V - C;

            double R1 = 0, G1 = 0, B1 = 0;
            if (H < 60) { R1 = C; G1 = X; B1 = 0; }
            else if (H < 120) { R1 = X; G1 = C; B1 = 0; }
            else if (H < 180) { R1 = 0; G1 = C; B1 = X; }
            else if (H < 240) { R1 = 0; G1 = X; B1 = C; }
            else if (H < 300) { R1 = X; G1 = 0; B1 = C; }
            else if (H < 360) { R1 = C; G1 = 0; B1 = X; }

            R = (byte)((R1 + m) * 255);
            G = (byte)((G1 + m) * 255);
            B = (byte)((B1 + m) * 255);

            return new SolidColorBrush(Color.FromRgb(R, G, B));
        }

        public SolidColorBrush GetBrush()
        {
            byte R, G, B;
            return GetBrush(out R, out G, out B);
        }

        private void RGBtoHSV(byte r, byte g, byte b)
        {
            float min, max, delta;
            min = Math.Min(r, Math.Min(g, b));
            max = Math.Max(r, Math.Max(g, b));

            Value = max / 255.0;               // v
            delta = max - min;
            if (max != 0)
                Saturation = delta / max;       // s
            else
            {
                // r = g = b = 0 // s = 0, v is undefined
                Saturation = 0;
                Hue = double.NaN;//Hue = -1;
                return;
            }

            if (r == max)
                Hue = (g - b) / delta;       // between yellow & magenta
            else if (g == max)
                Hue = 2 + (b - r) / delta;   // between cyan & yellow
            else
                Hue = 4 + (r - g) / delta;   // between magenta & cyan
            Hue *= 60;               // degrees
            if (Hue < 0)
                Hue += 360;

            Hue /= 360.0;
        }
    }
}
