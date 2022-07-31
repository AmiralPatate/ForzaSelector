using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FH5Data
{
    public static class Cruncher
    {
        //With data from https://forza.fandom.com/wiki/Forza_Horizon_5/Cars

        internal class Item
        {
            internal int Year { get; set; }
            internal string ManfModel { get; set; }
            internal string Rarity { get; set; }
            internal string Stats { get; set; }

            internal string Manf { get; set; }
            internal string Model { get; set; }
            internal string Family { get; set; }

            internal double Speed { get; set; }
            internal double Handling { get; set; }
            internal double Acceleration { get; set; }
            internal double Launch { get; set; }
            internal double Braking { get; set; }
            internal double Offroad { get; set; }
            internal int PI { get; set; }
        }

        private static bool IsRarity(string str)
        {
            return str == "COMMON" || str == "RARE" || str == "EPIC" || str == "LEGENDARY" || str == "FORZA EDITION";
        }

        public static void Crunch()
        {
            Output(Load());
        }

        private static void Output(List<Item> list)
        {
            List<string> lines = new List<string>();
            foreach (Item I in list)
            {
                string csv = I.Year.ToString("0000")
                    + "," + I.Manf
                    + "," + I.Model
                    + "," + I.Family
                    + "," + I.Rarity
                    + "," + I.PI.ToString("000")
                    + "," + I.Speed.ToString(CultureInfo.InvariantCulture)
                    + "," + I.Handling.ToString(CultureInfo.InvariantCulture)
                    + "," + I.Acceleration.ToString(CultureInfo.InvariantCulture)
                    + "," + I.Launch.ToString(CultureInfo.InvariantCulture)
                    + "," + I.Braking.ToString(CultureInfo.InvariantCulture)
                    + "," + I.Offroad.ToString(CultureInfo.InvariantCulture)
                    + ","
                    ;
                lines.Add(csv);
            }
            File.WriteAllLines(Path.Combine(Data.APPPATH, "data", "fh5", "cars.csv"), lines);
        }

        private static List<Item> Load()
        {
            string path = Path.Combine(Data.APPPATH, "data", "fh5", "raw");
            string[] raw = File.ReadAllLines(path);

            List<string> data = new List<string>();
            for (var i = 0; i < raw.Length - 1; i++)
            {
                int currentLine = 0;
                List<string> item = new List<string>();
                do
                {
                    item.Add(raw[i]);
                    i++;
                } while (i < raw.Length && !int.TryParse(raw[i], out currentLine));
                i--;
                data.Add(string.Join(";", item));
            }

            //Example: full
            //1973, ,Alpine A110 1600S,Autoshow,98,000 CR,RARE,5.0	4.4	4.1	3.1	3.0	5.3	C550
            //Example:some info missing
            //2013, ,Wuling Sunshine S,\"Hard to Find\": Festival Playlist/Forzathon Shop,? CR,RARE,D207

            //#0 is Year
            //#1 is Empty
            //#2 is Model
            //#J is any rarity string
            //#J+1 is Stats
            List<Item> data2 = new List<Item>();
            foreach (string line in data)
            {
                string[] bits = line.Split(';');

                Item I = new Item();
                I.Year = int.Parse(bits[0]);
                I.ManfModel = bits[2];

                int j = 3;
                while (j < bits.Length)
                {
                    if (IsRarity(bits[j])) break;
                    j++;
                }

                //Extract Manf
                List<string> MULTIMAN = new List<string>();
                var MANLIST = Lists.Manufacturers();

                foreach (var manf in MANLIST)
                {
                    if (I.ManfModel.IndexOf(manf.Name) >= 0)
                    {
                        MULTIMAN.Add(manf.Name);
                    }
                }
                //Case 1 manufacturer detected
                if (MULTIMAN.Count == 1)
                {
                    I.Manf = MULTIMAN[0];
                }
                //Case multiple manufacturer detected: pick first in the string
                else
                {
                    //Exception cases
                    if (MULTIMAN.Count == 0)
                    {
                        //DeBerti Design
                        if (I.ManfModel.IndexOf("DeBerti") == 0)
                        {
                            I.Manf = Lists.ManfFromName("DeBerti Design").Name;
                        }
                        //MINI
                        else if (I.ManfModel.IndexOf("MINI") == 0)
                        {
                            I.Manf = Lists.ManfFromName("Mini").Name + "!";
                        }
                        //RAM
                        else if (I.ManfModel.IndexOf("RAM") == 0)
                        {
                            I.Manf = Lists.ManfFromName("Ram").Name + "!";
                        }
                    }
                    //Regular cases
                    else
                        I.Manf = MULTIMAN.OrderBy(man => I.ManfModel.IndexOf(man)).First();
                }

                //Extract Model
                //Exception case: DMC-12
                if (I.ManfModel == "DeLorean DMC-12")
                {
                    I.Model = I.ManfModel;
                }
                //Exception case: DeBerti
                else if (I.Manf == "DeBerti Design")
                {
                    I.Model = I.ManfModel.Remove(I.ManfModel.IndexOf("DeBerti"), "DeBerti".Length).Trim();
                }
                //Exception case: MINI
                else if (I.Manf == "Mini!")
                {
                    I.Manf = "Mini";
                    I.Model = I.ManfModel.Remove(I.ManfModel.IndexOf("MINI"), "MINI".Length).Trim();
                }
                //Exception case: RAM
                else if (I.Manf == "Ram!")
                {
                    I.Manf = "Ram";
                    I.Model = I.ManfModel.Remove(I.ManfModel.IndexOf("RAM"), "RAM".Length).Trim();
                }
                //Else remove manufacturer from string, and trim
                else
                {
                    I.Model = I.ManfModel.Remove(I.ManfModel.IndexOf(I.Manf), I.Manf.Length).Trim();
                }

                //Family
                if (I.Model.Contains("NSX"))
                {
                    I.Family = "NSX";
                }
                else if (I.Model.Contains("Aventador"))
                {
                    I.Family = "Aventador";
                }
                else if (I.Model.Contains("M3"))
                {
                    I.Family = "M3";
                }
                else if (I.Model.Contains("M5"))
                {
                    I.Family = "M5";
                }
                else if (I.Model.Contains("Corvette"))
                {
                    I.Family = "Corvette";
                }
                else if (I.Model.Contains("Camaro"))
                {
                    I.Family = "Camaro";
                }
                else if (I.Model.Contains("Charger"))
                {
                    I.Family = "Charger";
                }
                else if (I.Model.Contains("Challenger"))
                {
                    I.Family = "Challenger";
                }
                else if (I.Model.Contains("Viper"))
                {
                    I.Family = "Viper";
                }
                else if (I.Model.Contains("Mustang") || I.Model.Contains("Shelby GT"))
                {
                    I.Family = "Mustang";
                }
                else if (I.Model.Contains("Escort"))
                {
                    I.Family = "Escort";
                }
                else if (I.Model.Contains("Focus"))
                {
                    I.Family = "Focus";
                }
                else if (I.Model.Contains("Fiesta"))
                {
                    I.Family = "Fiesta";
                }
                else if (I.Model.Contains("Civic"))
                {
                    I.Family = "Civic";
                }
                else if (I.Model.Contains("Lancer"))
                {
                    I.Family = "Lancer";
                }
                else if (I.Model.Contains("Skyline") || I.Model.StartsWith("GT-R") || I.Model.StartsWith("NISMO GT-R"))
                {
                    I.Family = "Skyline";
                }
                else if (I.Model.Contains("Fairlady Z") || I.Model.Contains("370Z") || I.Model.Contains("350Z"))
                {
                    I.Family = "Z-Car";
                }
                else if (I.Model.Contains("Silvia"))
                {
                    I.Family = "Silvia";
                }
                else if (I.Model.Contains("911") || I.Model == "Carrera 2.7 RS")
                {
                    I.Family = "911";
                }
                else if (I.Model.Contains("Impreza") || I.Model == "WRX STI" || I.Model == "STI S209")
                {
                    I.Family = "Impreza";
                }
                else if (I.Model.Contains("Supra"))
                {
                    I.Family = "Supra";
                }
                else if (I.Model.Contains("Celica"))
                {
                    I.Family = "Celica";
                }
                else if (I.Model.Contains("Golf") || I.Model == "GTI VR6 MK3")
                {
                    I.Family = "Golf";
                }

                I.Rarity = bits[j];
                I.Stats = bits[j + 1];

                //Only the PI
                if (char.IsLetter(I.Stats[0]))
                {
                    I.PI = int.Parse(I.Stats.Substring(I.Stats.Length - 3));
                }
                //Full stats
                else
                {
                    string[] subbits = I.Stats.Split('\t');
                    I.Speed = double.Parse(subbits[0], new CultureInfo("en-US", false));
                    I.Handling = double.Parse(subbits[1], new CultureInfo("en-US", false));
                    I.Acceleration = double.Parse(subbits[2], new CultureInfo("en-US", false));
                    I.Launch = double.Parse(subbits[3], new CultureInfo("en-US", false));
                    I.Braking = double.Parse(subbits[4], new CultureInfo("en-US", false));
                    I.Offroad = double.Parse(subbits[5], new CultureInfo("en-US", false));
                    I.PI = int.Parse(subbits[6].Substring(subbits[6].Length - 3), new CultureInfo("en-US", false));
                }

                data2.Add(I);
            }

            return data2;
        }
    }
}
