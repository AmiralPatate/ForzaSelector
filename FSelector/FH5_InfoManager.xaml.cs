using FDataH5;
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

namespace FSelector
{
    /// <summary>
    /// Logique d'interaction pour FH5_InfoManager.xaml
    /// </summary>
    public partial class FH5_InfoManager : UserControl
    {
        public FH5_InfoManager()
        {
            InitializeComponent();

            TMP.ItemsSource = Lists.Garage();
        }
    }
}
