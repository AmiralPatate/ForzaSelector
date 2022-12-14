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
using System.Windows.Shapes;

namespace FH5Interface
{
    /// <summary>
    /// Logique d'interaction pour Window_StockCarData.xaml
    /// </summary>
    public partial class Window_StockCarData : Window
    {
        public Window_StockCarData()
        {
            InitializeComponent();
        }

        public void OpenModel(Model model)
        {
            if (model != null)
                Container.SelectModel_FromOutside(model);
        }
    }
}
