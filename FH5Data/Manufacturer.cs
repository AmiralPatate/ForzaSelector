using Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace FH5Data
{
    public class Manufacturer
    {
        public string Name { get; set; }
        [JsonIgnore]
        public string CountryCode { get; set; }

        public override string ToString() { return Name; }

        public bool Equals(Manufacturer obj)
        {
            Manufacturer man = obj as Manufacturer;
            return Name == man.Name;
        }

        internal string ToCSV()
        {
            return Name
                + "," + CountryCode;
        }

        public string GetManfLogoPath() { return Path.Combine(Data.APPPATH, "data", "fh5", "manf", Name.ToLower() + ".png"); }

        public BitmapImage GetManfLogo()
        {
            string filename = GetManfLogoPath();
            if (!File.Exists(filename)) return null;

            return new BitmapImage(new Uri(filename, UriKind.Absolute));
        }
    }
}
