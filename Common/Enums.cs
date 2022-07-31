using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    //Induction:NA,T,TT,DSC,4T
    public enum InductionType
    {
        [Description("Naturally Aspired")]
        NA,
        [Description("Turbo")]
        T,
        [Description("Twin Turbo")]
        TT,
        [Description("Positive-Displacement Supercharger")]
        DSC,
        [Description("Quad Turbo")]
        T4,
    }

    //Cfg:I(nline),F(lat),V,VR,R(otary),W
    public enum EngineConfiguration
    {
        [Description("Inline")]
        I,
        [Description("Flat")]
        F,
        [Description("V")]
        V,
        [Description("VR")]
        VR,
        [Description("Rotary")]
        R,
        [Description("W")]
        W,
    }
}
