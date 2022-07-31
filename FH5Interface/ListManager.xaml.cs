using FH5Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FH5Interface
{
    /// <summary>
    /// Logique d'interaction pour ListManager.xaml
    /// </summary>
    public partial class ListManager : UserControl
    {
        public ListManager()
        {
            InitializeComponent();

            LMManf.SetLM(this);
            LMFam.SetLM(this);
            LMMod.SetLM(this);
        }

        public void UpdateLists()
        {
            LMMod.UpdateLists();
            LMFam.UpdateLists();
        }

        public void SelectManufacturer(Manufacturer manf)
        {
            LMManf.SelectManufacturer(manf);
        }

        public void SelectModel(Model mod)
        {
            LMManf.SelectModel(mod);
            LMFam.SelectModel(mod);
        }

        public void SelectModel_FromFam(Model mod)
        {
            LMMod.SelectModel(mod);
            LMManf.SelectModel(mod);
        }

        public void SelectModel_FromOutside(Model mod)
        {
            LMMod.SelectModel(mod, true);
            LMManf.SelectModel(mod);
            LMFam.SelectModel(mod);
        }
    }
}
