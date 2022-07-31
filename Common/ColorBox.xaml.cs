using Common;
using DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Logique d'interaction pour ColorBox.xaml
    /// </summary>
    public partial class ColorBox : UserControl
    {
        public ColorBox()
        {
            InitializeComponent();
            Container.Background = Brushes.Transparent;
        }

        public void Set(CarColor col)
        {
            SelectedColor = col;
            if (SelectedColor != null) Container.Background = SelectedColor.Brush;
            else Container.Background = Brushes.Transparent;
        }

        public CarColor SelectedColor { get; private set; }

        public event GenericEvent ColorPicked;

        private void Container_Click(object sender, RoutedEventArgs e)
        {
            SelectedColor = ColorPickerWindow.PopUp();
            if (SelectedColor != null) Container.Background = SelectedColor.Brush;
            else Container.Background = Brushes.Transparent;
            if (ColorPicked != null) ColorPicked();
        }
    }
}
