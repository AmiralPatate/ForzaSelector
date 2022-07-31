using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH5Data
{
    public class Filter
    {
        //if ByXxxx true = filter in all matching, false = don't filter, null = filter out all matching

        public bool ByPI { get; set; }
        public int PI_Min { get; set; }
        public int PI_Max { get; set; }
        private bool Match_PI(int pi) { return pi >= PI_Min && pi <= PI_Max; }

        public bool ByPICompetitive { get; set; }
        public bool IsCompetitive { get; set; }
        private bool Match_PIComp(int pi)
        {
            CarClass @class = pi.ClassFromPi();
            int maxPI = @class.GetMaxPI();
            if (IsCompetitive) return pi >= maxPI - 10;
            else return pi < maxPI - 5;
        }

        public bool ByClass { get; set; }
        public CarClass[] Class { get; set; }
        private bool Match_Class(CarClass @class) { return Class.Contains(@class); }

        public bool ByYear { get; set; }
        public int Year_Min { get; set; }
        public int Year_Max { get; set; }
        private bool Match_Year(int year) { return year >= Year_Min && year <= Year_Max; }

        public bool ByManf { get; set; }
        public Manufacturer[] Manfs { get; set; }
        private bool Match_Manf(Manufacturer manf) { return Manfs.Where(m => m.Equals(manf)).Count() > 0; }

        public bool ByFamily { get; set; }
        public string[] Family { get; set; }
        private bool Match_Family(Car car) { return car.Model.HasFamily && Family.Where(f => f == car.Model.ModelFamily).Count() > 0; }

        public bool ByType { get; set; }
        public CarType[] Type { get; set; }
        private bool Match_Type(Car car) { return car.Model.HasType && Type.Where(t => t.Equals(car.Model.Type)).Count() > 0; }

        public bool ByRarity { get; set; }
        public Rarity[] Rarity { get; set; }
        private bool Match_Rarity(Rarity rarity) { return Rarity.Contains(rarity); }

        public bool ByCountry { get; set; }
        public string[] CountryCodes { get; set; }
        private bool Match_Country(string code) { return CountryCodes.Contains(code); }

        public bool BySetup { get; set; }
        public Setup[] Setup { get; set; }
        private bool Match_Setup(Setup setup) { return Setup.Contains(setup); }

        public bool ByDrive { get; set; }
        public Drive[] Drive { get; set; }
        private bool Match_Drive(Drive drive) { return Drive.Contains(drive); }

        public bool ByCustomSpec { get; set; }
        public bool HasCustomSpec { get; set; }
        private bool Match_CustomSpec(Car car) { return car.HasCustomSpecs == HasCustomSpec; }

        public bool ByCustomLivery { get; set; }
        public bool HasCustomLivery { get; set; }
        private bool Match_CustomLivery(Car car) { return car.HasCustomLivery == HasCustomLivery; }

        public bool ByDriven { get; set; }
        public bool HasBeenDriven { get; set; }
        private bool Match_Driven(bool isDriven) { return HasBeenDriven == isDriven; }

        public bool BySearchText { get; set; }
        public bool BySearchText_Model { get; set; }
        public bool BySearchText_Manf { get; set; }
        public bool BySearchText_Type { get; set; }
        public bool BySearchText_Livery { get; set; }
        public bool BySearchText_Specs { get; set; }
        public bool BySearchText_Family { get; set; }
        public bool BySearchText_Year { get; set; }
        public string SearchText { get; set; }
        private bool Match_SearchText(Car car)
        {
            SearchText = SearchText.ToUpper();
            bool result = BySearchText_Model && car.Model.Name.ToUpper().Contains(SearchText);
            result |= BySearchText_Manf && car.Model.Manufacturer.Name.ToUpper().Contains(SearchText);
            result |= BySearchText_Type && car.Model.HasType && car.Model.Type.Name.ToUpper().Contains(SearchText);
            result |= BySearchText_Livery && car.HasCustomLivery && car.Livery.Name.ToUpper().Contains(SearchText);
            result |= BySearchText_Specs && car.HasCustomSpecs && car.SpecName.ToUpper().Contains(SearchText);
            result |= BySearchText_Family && car.Model.HasFamily && car.Model.ModelFamily.ToUpper().Contains(SearchText);

            int yearMaybe;
            if (BySearchText_Year && int.TryParse(SearchText, out yearMaybe)) result |= car.Model.Year == yearMaybe;

            return result;
        }

        public bool ByColor { get; set; }
        public bool ByColorAny { get; set; }
        public string[] Colors { get; set; }
        private bool Match_Color(CarLivery livery)
        {
            bool result = Colors.Contains(livery.PrimaryColor);
            if (ByColorAny) result |= Colors.Contains(livery.SecondaryColor) || Colors.Contains(livery.TernaryColor);
            return result;
        }

        public bool ByEngineSwap { get; set; }
        public bool ByEngineSwap_IsStock { get; set; }
        private bool Match_EngineSwap(EngineSwap engine)
        {
            if (ByEngineSwap_IsStock) return engine.IsStock;
            else return !engine.IsStock;
        }

        public Filter() { }

        public List<Car> Matches(List<Car> list)
        {
            int a, b;
            return Matches(list, out a, out b);
        }

        public List<Car> Matches(List<Car> list, out int count, out int outOf)
        {
            outOf = list.Count;
            var matches = list.Where(car => IsMatch(car)).ToList();
            count = matches.Count();
            return matches;
        }

        private bool IsMatch(Car car)
        {
            bool match = true;

            if (ByPI) match &= Match_PI(car.Stats.PI);
            if (ByPICompetitive) match &= Match_PIComp(car.Stats.PI);
            if (ByClass) match &= Match_Class(car.Stats.PI.ClassFromPi());
            if (BySetup) match &= Match_Setup(car.Setup);
            if (ByDrive) match &= Match_Drive(car.Drivetrain);

            if (ByYear) match &= Match_Year(car.Model.Year);
            if (ByManf) match &= Match_Manf(car.Model.Manufacturer);
            if (ByFamily) match &= Match_Family(car);
            if (ByType) match &= Match_Type(car);
            if (ByRarity) match &= Match_Rarity(car.Model.Rarity);
            if (ByCountry) match &= Match_Country(car.Model.Manufacturer.CountryCode);

            if (ByCustomSpec) match &= Match_CustomSpec(car);
            if (ByCustomLivery) match &= Match_CustomLivery(car);
            if (ByDriven) match &= Match_Driven(car.IsDriven);

            if (BySearchText) match &= Match_SearchText(car);

            if (ByColor) match &= Match_Color(car.Livery);

            if (ByEngineSwap) match &= Match_EngineSwap(car.Engine);

            return match;
        }
    }
}
