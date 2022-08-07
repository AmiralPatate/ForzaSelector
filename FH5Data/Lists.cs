using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FH5Data
{
    //[Flags]
    //internal enum EDITFLAGS { MODL = 1, MANF = 2, TYPE = 4 }

    public static class Lists
    {
        private static Random RDM = new Random();
        private static List<Model> ModelList { get; set; }
        private static List<Manufacturer> ManufacturerList { get; set; }
        private static List<CarType> TypeList { get; set; }
        private static List<Car> GarageList { get; set; }
        private static List<string> TemporaryFamilies { get; set; }
        private static List<EngineSwap> EngineList { get; set; }
        //internal static EDITFLAGS EDITFLAG = 0; 

        internal static string[] ExportModel()
        {
            return ModelList.OrderBy(m => m.Manufacturer.Name).ThenByDescending(m => m.Year).ThenBy(m => m.Name).Select(mod => mod.ToCSV()).ToArray();
        }

        internal static string[] ExportManufacturer()
        {
            return ManufacturerList.OrderBy(m => m.Name).Select(manf => manf.ToCSV()).ToArray();
        }

        internal static string[] ExportType()
        {
            return TypeList.OrderBy(t => t.Name).Select(typ => typ.ToCSV()).ToArray();
        }

        internal static string ExportGarage()
        {
            return JsonConvert.SerializeObject(GarageList.OrderBy(c=>c.Model.Manufacturer.Name).ThenByDescending(c => c.Model.Year).ThenBy(c => c.Model.Name).ThenByDescending(c=>c.Stats.PI).ThenBy(c=>c.CarNumber), Formatting.Indented);
        }

        internal static void Init()
        {
            ManufacturerList = new List<Manufacturer>();
            ModelList = new List<Model>();
            TypeList = new List<CarType>();
            GarageList = new List<Car>();
            TemporaryFamilies = new List<string>();
            EngineList = new List<EngineSwap>();
        }

        internal static void Add(CarType type)
        {
            if (TypeList.Where(t => t.Equals(type)).Count() == 0)
                TypeList.Add(type);
        }

        internal static void Add(Model model)
        {
            if (ModelList.Where(m => m.Equals(model)).Count() == 0)
                ModelList.Add(model);
        }

        internal static void Add(Manufacturer manf)
        {
            if (ManufacturerList.Where(m => m.Equals(manf)).Count() == 0)
            {
                ManufacturerList.Add(manf);
            }
        }

        private static int globalCarNumber = 0;
        internal static void Add(Car car)
        {
            ++globalCarNumber;
            if (car.CarNumber == -1) car.CarNumber = globalCarNumber;
            GarageList.Add(car);
        }

        internal static void Add(EngineSwap engine)
        {
            EngineList.Add(engine);
        }

        public static Car RandomCar(Filter filter = null)
        {
            List<Car> filteredList;
            if (filter == null) filteredList = GarageList;
            else filteredList = filter.Matches(GarageList);
            //GarageList.Where(car => filter.IsMatch(car)).ToList();

            if (filteredList.Count > 0)
            {
                int index = RDM.Next(0, filteredList.Count);
                return filteredList[index];
            }
            else return null;
        }

        public static void NewFamily(string fam)
        {
            if (fam != string.Empty && TemporaryFamilies.Where(f => f == fam).Count() == 0)
                TemporaryFamilies.Add(fam);
        }

        public static void NewCar(Car car)
        {
            Add(car);
        }

        public static void RenameFamily(string old, string @new)
        {
            var list = ModelsByFam(old);
            foreach (Model mod in list)
                mod.ModelFamily = @new;
        }

        public static void NewModel(Model mod)
        {
            Add(mod);
        }

        public static void NewManufacturer(Manufacturer manf)
        {
            Add(manf);
        }

        internal static Model FindModel(int year, string manf, string model)
        {
            var result = ModelList.Where(mod => mod.Year == year && mod.Manufacturer.Name == manf && mod.Name == model);
            if (result.Count() == 1) return result.First();
            else throw new Exception("ERRCODE1");
        }

        internal static Manufacturer ManfFromName(string name)
        {
            var result = ManufacturerList.Where(manf => manf.Name == name);
            if (result.Count() == 1) return result.First();
            else throw new Exception("ERRCODE1");
        }

        public static CarType TypeByName(string name)
        {
            var results = TypeList.Where(typ => typ.Name == name);
            if (results.Count() == 1) return results.First();
            else throw new Exception("ERRCODE1");
        }

        public static List<Manufacturer> Manufacturers()
        {
            return ManufacturerList.OrderBy(manf => manf.Name).ToList();
        }

        public static List<Model> Models()
        {
            return ModelList
                .OrderBy(mod => mod.Manufacturer.Name)
                .ThenByDescending(mod => mod.Year)
                .ThenBy(mod => mod.Name)
                .ToList();
        }

        public static List<CarType> Types()
        {
            return TypeList.OrderBy(type => type.Name).ToList();
        }

        public static List<Car> Garage()
        {
            return GarageList.OrderBy(car => car.Model.Manufacturer.Name)
                    .ThenByDescending(car => car.Model.Year)
                    .ThenBy(car => car.Model.Name)
                    .ToList();
        }

        public static List<EngineSwap> Engines(bool byForzaNameFirst)
        {
            if (byForzaNameFirst)
                return EngineList.OrderByDescending(eng => eng.EngineId == 0)
                    .ThenBy(eng => eng.ForzaName.Substring(0, 3))
                    .ThenBy(eng => eng.StockHP)
                    .ThenByDescending(eng => eng.MaxHP)
                    .ThenBy(eng => eng.EngineId)
                    .ToList();
            else
                return EngineList.OrderByDescending(eng => eng.EngineId == 0)
                    .ThenBy(eng => eng.Manufacturer)
                    .ThenBy(eng => eng.RealName)
                    .ThenBy(eng => eng.StockHP)
                    .ThenByDescending(eng => eng.MaxHP)
                    .ThenBy(eng => eng.EngineId)
                    .ToList();
        }

        public static List<string> Families(bool noEmpty = false)
        {
            if (noEmpty)
                return ModelList.Where(mod => mod.HasFamily).Select(mod => mod.ModelFamily).Distinct().Concat(TemporaryFamilies).OrderBy(fam => fam).ToList();
            else
                return ModelList.Select(mod => mod.ModelFamily).Distinct().Concat(TemporaryFamilies).OrderBy(fam => fam).ToList();
        }

        public static List<Model> ModelsByFam(string fam)
        {
            if (fam == null || fam == string.Empty) return null;
            return ModelList.Where(mod => mod.ModelFamily == fam)
                .OrderByDescending(mod => mod.Year)
                .ThenBy(mod => mod.Manufacturer.Name)
                .ThenBy(mod => mod.Name)
                .ToList();
        }

        public static List<Model> ModelsByManf(Manufacturer manf, bool sortByYearFirst = true)
        {
            if (sortByYearFirst)
                return ModelList.Where(mod => mod.Manufacturer.Name == manf.Name)
                    .OrderBy(mod => mod.Manufacturer.Name)
                    .ThenByDescending(mod => mod.Year)
                    .ThenBy(mod => mod.Name)
                    .ToList();
            else
                return ModelList.Where(mod => mod.Manufacturer.Name == manf.Name)
                    .OrderBy(mod => mod.Manufacturer.Name)
                    .ThenBy(mod => mod.Name)
                    .ThenByDescending(mod => mod.Year)
                    .ToList();
        }

        public static void RemoveCar(Car car)
        {
            GarageList.Remove(car);
            car = null;
        }

        public static EngineSwap FindEngine(int id)
        {
            if (id <= 0) return EngineSwap.Stock;
            var results = Lists.EngineList.Where(eng => eng.EngineId == id);
            return results.First();
        }
    }
}
