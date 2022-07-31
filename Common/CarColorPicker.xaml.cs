using Common;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logique d'interaction pour CarColorPicker.xaml
    /// </summary>
    public partial class CarColorPicker : UserControl
    {
        public CarColorPicker()
        {
            InitializeComponent();

            StackPanel SP = new StackPanel() { Margin = new Thickness(0, 1, 0, 1), Orientation = Orientation.Horizontal };
            int i = 0;
            foreach (ColorFamily family in Enum.GetValues(typeof(ColorFamily)))
            {
                foreach (CarColor color in CarColor.List.Where(col => col.Family == family).OrderBy(col => col.ColorIndex))
                {
                    Rectangle RE = new Rectangle() { Width = 50, Height = 25, Margin = new Thickness(1, 0, 1, 0) };
                    RE.Fill = color.Brush;
                    RE.ToolTip = color.Name;
                    RE.Tag = color;
                    RE.MouseUp += RE_MouseUp;
                    SP.Children.Add(RE);

                    i++;
                    if (i >= CarColor.CarColorIndexMax)
                    {
                        i = 0;
                        Container.Children.Add(SP);
                        SP = new StackPanel() { Margin = new Thickness(0, 1, 0, 1), Orientation = Orientation.Horizontal };
                    }
                }





                //for (var i = 0; i < CarColor.CarColorIndexMax; i++)
                //{

                //    CarColor color = CarColor.List.Where(col => col.Family == family && col.ColorIndex == i).FirstOrDefault();

                //    SP.Children.Add(RE);
                //    if (color == null) continue;


                //}

                //Container.Children.Add(SP);
            }
            Container.Children.Add(SP);
        }

        public CarColor SelectedColor { get; private set; }

        public event GenericEvent ColorPicked;

        private void RE_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle)
            {
                Rectangle RE = sender as Rectangle;
                SelectedColor = RE.Tag as CarColor;
                if (ColorPicked != null) ColorPicked();
            }
        }
    }
}
