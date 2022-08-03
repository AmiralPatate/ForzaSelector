using Newtonsoft.Json;
using System;
using System.Globalization;

namespace FH5Data
{
    public class Model
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public string ModelFamily { get; set; }
        public Manufacturer Manufacturer { get; set; }
        [JsonIgnore]
        public CarType Type { get; set; }
        [JsonIgnore]
        public Rarity Rarity { get; set; }
        [JsonIgnore]
        public CarStats Stats { get; set; }
        [JsonIgnore]
        public Drive Drivetrain { get; set; }

        [JsonIgnore]
        public bool HasFamily { get { return ModelFamily != null && ModelFamily != string.Empty; } }
        [JsonIgnore]
        public bool HasType { get { return Type != null && Type.Name != null && Type.Name != string.Empty; } }

        public override string ToString() { return Year.ToString("0000") + " " + Manufacturer.Name + " " + Name; }

        public bool Equals(Model obj)
        {
            Model mod = obj as Model;
            return Manufacturer.Equals(mod.Manufacturer)
                && Year == mod.Year
                && Name == mod.Name
                && Type == mod.Type
                && ModelFamily == mod.ModelFamily;
        }

        internal string ToCSV()
        {
            //year,manf,model,family,rarity,pi,speed,handling,acceleration,launch,braking,offroad,type,drive

            return Year.ToString("0000")
                + "," + Manufacturer.Name
                + "," + Name
                + "," + ModelFamily
                + "," + this.Rarity.GetName().ToUpper()
                + "," + Stats.PI.ToString("000")
                + "," + Stats.Speed.ToString(CultureInfo.InvariantCulture)
                + "," + Stats.Handling.ToString(CultureInfo.InvariantCulture)
                + "," + Stats.Acceleration.ToString(CultureInfo.InvariantCulture)
                + "," + Stats.Launch.ToString(CultureInfo.InvariantCulture)
                + "," + Stats.Braking.ToString(CultureInfo.InvariantCulture)
                + "," + Stats.Offroad.ToString(CultureInfo.InvariantCulture)
                + "," + (Type != null ? Type.Name : "")
                + "," + Drivetrain.ToString()
                ;
        }
    }

}
