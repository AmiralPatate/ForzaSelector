using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using System.Linq;

namespace Common
{
    public delegate void GenericEvent();

    public static class Data
    {
        public static readonly string APPPATH = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private static Dictionary<string, string> CountryCodes;

        public static List<string> CountryCodeList
        {
            get
            {
                if (CountryCodes == null) InitCountryCodes();
                return new List<string>(CountryCodes.Keys).OrderBy(x => x).ToList();
            }
        }

        private static void InitCountryCodes()
        {
            CountryCodes = new Dictionary<string, string>();
            string[] raw = File.ReadAllLines(Path.Combine(APPPATH, "data", "country.csv"));

            foreach (string line in raw)
            {
                string[] bits = line.Split(',');
                CountryCodes.Add(bits[0], bits[1]);
            }
        }

        public static BitmapImage GetCountryFlagBitmap(string code)
        {
            if (CountryCodes == null) InitCountryCodes();

            code = code.ToLower();
            string filename = null;
            if (CountryCodes.ContainsKey(code))
                filename = Path.Combine(APPPATH, "data", "flags", code + ".png");
            else
                return null;

            if (!File.Exists(filename)) return null;

            return new BitmapImage(new Uri(filename, UriKind.Absolute));
        }

        public static string GetCountryName(string code)
        {
            if (CountryCodes == null) InitCountryCodes();

            code = code.ToLower();
            if (CountryCodes.ContainsKey(code))
                return CountryCodes[code];
            else
                return "Unknown";
        }

    }
}
