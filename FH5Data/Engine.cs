using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH5Data
{
    //ForzaName,StockHP,MaxHP,Conf,Cylinders,Induction,Manf,RealName
    public class EngineSwap
    {
        public static readonly EngineSwap Stock = new EngineSwap() { EngineId = 0, ForzaName = "Stock" };

        public int EngineId { get; set; }
        public string ForzaName { get; set; }
        public int StockHP { get; set; }
        public int MaxHP { get; set; }
        public EngineConfiguration Configuration { get; set; }
        public int Cylinders { get; set; }
        public InductionType Induction { get; set; }
        public string Manufacturer { get; set; }
        public string RealName { get; set; }
        public bool IsStock { get { return this == Stock || EngineId <= 0; } }

        public string ShortName
        {
            get
            {
                if (EngineId <= 0) return ForzaName;

                string name = "";

                if (string.IsNullOrEmpty(Manufacturer)) name += ForzaName;
                else
                {
                    name += Manufacturer;
                    if (!string.IsNullOrEmpty(RealName)) name += " " + RealName;
                    name += " (" + Configuration.ToString() + Cylinders;
                    if (Induction != InductionType.NA) name += Induction.ToString();
                    name += ")";
                }

                return name;
            }
        }

        public override string ToString()
        {
            if (this == Stock || EngineId <= 0) return "Stock";

            string name = ForzaName;
            if (!string.IsNullOrEmpty(Manufacturer)) name += " = " + Manufacturer;
            if (!string.IsNullOrEmpty(RealName)) name += " " + RealName;
            name += " (" + Configuration.ToString() + Cylinders;
            if (Induction != InductionType.NA) name += "-" + Induction.ToString();
            name += " : " + StockHP + " - " + MaxHP + " HP)";
            return name;
        }
    }
}
