using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
    public enum LiveryType
    {
        Classic,
        Racing,
        Gradient
    }

    public class CarLivery
    {
        public LiveryType Type { get; set; }

        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string TernaryColor { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string PrimaryFamily
        {
            get
            {
                if (Primary == null) return string.Empty;
                if (Primary.Family == ColorFamily.Greyscale)
                {
                    if (Primary.ColorIndex == 0) return "Black";
                    else if (Primary.ColorIndex == 7) return "White";
                    else return "Gray";
                }
                else
                    return Primary.Family.ToString();
            }
        }

        [JsonIgnore]
        public string SecondaryFamily
        {
            get
            {
                if (Ternary == null) return string.Empty;
                if (Secondary.Family == ColorFamily.Greyscale)
                {
                    if (Secondary.ColorIndex == 0) return "Black";
                    else if (Secondary.ColorIndex == 7) return "White";
                    else return "Grey";
                }
                else
                    return Secondary.Family.ToString();
            }
        }

        [JsonIgnore]
        public string TernaryFamily
        {
            get
            {
                if (Ternary == null) return string.Empty;
                if (Ternary.Family == ColorFamily.Greyscale)
                {
                    if (Ternary.ColorIndex == 0) return "Black";
                    else if (Ternary.ColorIndex == 7) return "White";
                    else return "Grey";
                }
                else
                    return Ternary.Family.ToString();
            }
        }

        [JsonIgnore]
        public CarColor Primary { get { return CarColor.ByName(PrimaryColor); } }
        [JsonIgnore]
        public CarColor Secondary { get { return CarColor.ByName(SecondaryColor); } }
        [JsonIgnore]
        public CarColor Ternary { get { return CarColor.ByName(TernaryColor); } }

        public CarLivery()
        {
            PrimaryColor = null;
            SecondaryColor = null;
            TernaryColor = null;
        }

        public void ImportLivery(CarLivery liv)
        {
            if (liv == null)
            {
                Name = null;
                PrimaryColor = null;
                SecondaryColor = null;
                TernaryColor = null;
                Type = LiveryType.Classic;
            }
            else
            {
                Name = liv.Name;
                PrimaryColor = liv.PrimaryColor;
                SecondaryColor = liv.SecondaryColor;
                TernaryColor = liv.TernaryColor;
                Type = liv.Type;
            }
        }
    }
}
