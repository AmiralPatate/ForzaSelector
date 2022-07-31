using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH5Data
{
    public class ImportData
    {
        private const string carfile = "cars.csv";
        private const string manfile = "manf.csv";
        private const string typefile = "types.csv";
        private const string garagefile = "garage.json";
        private const string enginefile = "engines.csv";

        public static void Export(string dirname)
        {
            File.WriteAllLines(Path.Combine(dirname, carfile), Lists.ExportModel());
            File.WriteAllLines(Path.Combine(dirname, manfile), Lists.ExportManufacturer());
            File.WriteAllLines(Path.Combine(dirname, typefile), Lists.ExportType());
            File.WriteAllText(Path.Combine(dirname, garagefile), Lists.ExportGarage());
        }

        public static void Quicksave()
        {
            Export(@"data\fh5");
        }

        public static void Import(string dirname, bool crunch = false)
        {
            Lists.Init();
            ImportManufacturers(Path.Combine(dirname, manfile)); //#1
            ImportTypes(Path.Combine(dirname, typefile)); //#2
            ImportEngines(Path.Combine(dirname, enginefile)); //#3
            ImportCars(Path.Combine(dirname, carfile)); //#4
            ImportGarage(Path.Combine(dirname, garagefile)); //#5
        }

        internal static void ImportEngines(string filename)
        {
            string[] raw = File.ReadAllLines(filename);
            int id = 1;
            Lists.Add(EngineSwap.Stock);
            foreach (string line in raw)
            {
                string[] bits = line.Split(',');

                EngineSwap ENG = new EngineSwap();
                //ForzaName,StockHP,MaxHP,Conf,Cylinders,Induction,Manf,RealName

                ENG.EngineId = id++;
                ENG.ForzaName = bits[0].Trim();
                ENG.StockHP = int.Parse(bits[1]);
                ENG.MaxHP = int.Parse(bits[2]);
                ENG.Configuration = (EngineConfiguration)(Enum.Parse(typeof(EngineConfiguration), bits[3]));
                ENG.Cylinders = int.Parse(bits[4]);
                ENG.Induction = (InductionType)(Enum.Parse(typeof(InductionType), bits[5]));
                ENG.Manufacturer = bits[6];
                ENG.RealName = bits[7];
                Lists.Add(ENG);
            }
        }

        internal static void ImportGarage(string filename)
        {
            if (File.Exists(filename))
            {
                string raw = File.ReadAllText(filename);
                var list = JsonConvert.DeserializeObject<List<Car>>(raw);
                foreach (Car CAR in list)
                {
                    CAR.Model = Lists.FindModel(CAR.Model.Year, CAR.Model.Manufacturer.Name, CAR.Model.Name);
                    Lists.Add(CAR);
                }
            }
        }

        internal static void ImportManufacturers(string filename)
        {
            string[] raw = File.ReadAllLines(filename);

            foreach (string line in raw)
            {
                string[] bits = line.Split(',');

                Manufacturer MNF = new Manufacturer();
                MNF.Name = bits[0].Trim();
                MNF.CountryCode = bits[1].Trim();
                Lists.Add(MNF);
            }
        }

        internal static void ImportTypes(string filename)
        {
            string[] raw = File.ReadAllLines(filename);

            foreach (string line in raw)
            {
                CarType TYP = new CarType();
                TYP.Name = line.Trim();
                Lists.Add(TYP);
            }
        }

        internal static void ImportCars(string filename)
        {
            string[] raw = File.ReadAllLines(filename);

            foreach (string line in raw)
            {
                string[] bits = line.Split(',');

                //year,manf,model,family,rarity,pi,speed,handling,acceleration,launch,braking,offroad,type

                Model MDL = new Model();
                MDL.Year = int.Parse(bits[0].Trim());
                MDL.Manufacturer = Lists.ManfFromName(bits[1].Trim());
                MDL.Name = bits[2].Trim();
                MDL.ModelFamily = bits[3].Trim();
                switch (bits[4])
                {
                    case "COMMON": MDL.Rarity = Rarity.Common; break;
                    case "RARE": MDL.Rarity = Rarity.Rare; break;
                    case "EPIC": MDL.Rarity = Rarity.Epic; break;
                    case "LEGENDARY": MDL.Rarity = Rarity.Legendary; break;
                    case "FORZA EDITION": MDL.Rarity = Rarity.ForzaEdition; break;
                }
                CarStats STA = new CarStats();
                if (bits[5] != string.Empty) STA.PI = int.Parse(bits[5], NumberStyles.Float, CultureInfo.InvariantCulture);

                if (bits.Length >= 6 && bits[6] != string.Empty) STA.Speed = double.Parse(bits[6], NumberStyles.Float, CultureInfo.InvariantCulture);
                if (bits.Length >= 7 && bits[7] != string.Empty) STA.Handling = double.Parse(bits[7], NumberStyles.Float, CultureInfo.InvariantCulture);
                if (bits.Length >= 8 && bits[8] != string.Empty) STA.Acceleration = double.Parse(bits[8], NumberStyles.Float, CultureInfo.InvariantCulture);
                if (bits.Length >= 9 && bits[9] != string.Empty) STA.Launch = double.Parse(bits[9], NumberStyles.Float, CultureInfo.InvariantCulture);
                if (bits.Length >= 10 && bits[10] != string.Empty) STA.Braking = double.Parse(bits[10], NumberStyles.Float, CultureInfo.InvariantCulture);
                if (bits.Length >= 11 && bits[11] != string.Empty) STA.Offroad = double.Parse(bits[11], NumberStyles.Float, CultureInfo.InvariantCulture);

                if (bits.Length >= 12 && bits[12] != string.Empty) MDL.Type = Lists.TypeByName(bits[12].Trim());
                MDL.Stats = STA;

                Lists.Add(MDL);
            }
        }
    }
}
