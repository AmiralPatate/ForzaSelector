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
    /// Logique d'interaction pour Window_GarageList.xaml
    /// </summary>
    public partial class Window_GarageList : Window
    {
        public Car ReturnValue { get { return Container.ReturnValue; } }
        public Car ReturnValueComp { get { return Container.ReturnValueComp; } }
        public Window_GarageList()
        {
            InitializeComponent();
        }

        public void SelectCar(Car car)
        {
            Container.SelectCar(car);
        }
    }
}
