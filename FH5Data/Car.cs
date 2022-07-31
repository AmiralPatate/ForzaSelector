using DataModel;
using Newtonsoft.Json;
using System.Windows.Media;

namespace FH5Data
{
    public class Car
    {
        public Car()
        {
            Stats = new CarStats();
            Model = new Model();
            Livery = new CarLivery();
        }

        public Model Model { get; set; }
        public CarStats Stats { get; set; }
        public Setup Setup { get; set; }
        public Drive Drivetrain { get; set; }
        public string SpecName { get; set; }
        public CarLivery Livery { get; set; }
        public bool IsDriven { get; set; }
        public int EngineID { get; set; }

        [JsonIgnore]
        public EngineSwap Engine { get { return Lists.FindEngine(EngineID); } }

        [JsonIgnore]
        public bool HasCustomSpecs { get { return SpecName != null && SpecName != string.Empty; } }
        [JsonIgnore]
        public bool HasCustomLivery { get { return Livery.Name != null && Livery.Name != string.Empty; } }

        //For use to bind on Datagrid columns
        //Set is empty because no set breaks the double click for some reason
        [JsonIgnore]
        public string PerfClass { get { return Stats.PI.ClassFromPi().ToString(); } set { } }
        [JsonIgnore]
        public SolidColorBrush ClassColor { get { return Stats.PI.ClassFromPi().Color(); } set { } }
        [JsonIgnore]
        public Brush RarityColor { get { return Model.Rarity.Color(); } set { } }
        [JsonIgnore]
        public string RarityString { get { return Model.Rarity.GetName(); } set { } }
        [JsonIgnore]
        public string CountryCode { get { return Model.Manufacturer.CountryCode.ToUpper(); } set { } }



        public override string ToString() { return Model.ToString(); }
    }
}
