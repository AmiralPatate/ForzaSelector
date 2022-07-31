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
    /// Logique d'interaction pour GarageManager_List.xaml
    /// </summary>
    public partial class GarageManager_List : UserControl
    {
        public Car ReturnValue { get; private set; }
        public Car ReturnValueComp { get; private set; }

        public GarageManager_List()
        {
            InitializeComponent();

            Container.ItemsSource = Lists.Garage();
            FilterContainer.FilterUpdated += FilterContainer_FilterUpdated;
        }

        public void SelectCar(Car car)
        {
            //Container.UpdateLayout();
            Container.SelectedItem = car;
            Container.ScrollIntoView(car);
            Container.Focus();
        }

        void FilterContainer_FilterUpdated(Filter filter)
        {
            Container.ItemsSource = filter.Matches(Lists.Garage());
        }

        private void OpenForEdit()
        {
            if (Container.SelectedItem == null) return;
            var ce = new Window_CarEdit(Container.SelectedItem as Car);
            ce.ShowDialog();
        }

        private void OpenModelForEdit()
        {
            if (Container.SelectedItem == null) return;
            var scd = new Window_StockCarData();
            scd.OpenModel((Container.SelectedItem as Car).Model);
            scd.ShowDialog();
        }

        private void Select()
        {
            if (Container.SelectedItem == null) return;
            ReturnValue = Container.SelectedItem as Car;
            Window.GetWindow(this).Close();
        }

        private void Container_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Select();
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            Select();
        }

        private void EditCar_Click(object sender, RoutedEventArgs e)
        {
            OpenForEdit();
            Container.ItemsSource = Lists.Garage();
        }

        private void EditModel_Click(object sender, RoutedEventArgs e)
        {
            OpenModelForEdit();
        }

        private void Compare_Click(object sender, RoutedEventArgs e)
        {
            if (Container.SelectedItem == null) return;
            ReturnValueComp = Container.SelectedItem as Car;
        }

        private void UnCompare_Click(object sender, RoutedEventArgs e)
        {
            ReturnValueComp = null;
        }

        private string Entry = "";
        private DateTime LastEntry = DateTime.Now.AddYears(-1);
        private void Container_KeyUp(object sender, KeyEventArgs e)
        {
            var list = Container.ItemsSource.OfType<Car>().ToList();
            var search = e.Key.ToString().First();

            if (DateTime.Now.Subtract(LastEntry).TotalMilliseconds > 500) Entry = search.ToString();
            else Entry += search;

            var matches = list.Where(c => c.Model.Manufacturer.Name.ToUpper().StartsWith(Entry)).ToList();
            if (matches.Count() > 0)
            {
                if (Container.SelectedItem == null)
                    SelectCar(matches.First());
                else
                {
                    if (Entry.Length == 1 && (Container.SelectedItem as Car).Model.Manufacturer.Name.ToUpper().StartsWith(Entry))
                    {
                        int index = matches.IndexOf(Container.SelectedItem as Car);
                        index = (index + 1) % matches.Count();
                        SelectCar(matches[index]);
                    }
                    else
                    {
                        SelectCar(matches.First());
                    }
                }
            }
            LastEntry = DateTime.Now;
        }
    }
}
