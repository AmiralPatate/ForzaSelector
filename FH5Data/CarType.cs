using Common;
using Newtonsoft.Json;
using System;
using System.Windows.Media.Imaging;
using System.IO;

namespace FH5Data
{
    public class CarType
    {
        public string Name { get; set; }

        public override string ToString() { return Name; }

        [JsonIgnore]
        public BitmapImage Logo
        {
            get
            {
                string filename = Path.Combine(Data.APPPATH, "data", "fh5", "types", Name.ToLower().Replace("'", "") + ".png");
                if (File.Exists(filename))
                    return new BitmapImage(new Uri(filename, UriKind.Absolute));
                else
                    return null;
            }
        }

        public bool Equals(CarType obj)
        {
            CarType typ = obj as CarType;
            return Name == typ.Name;
        }

        internal string ToCSV()
        {
            return Name;
        }
    }
}
