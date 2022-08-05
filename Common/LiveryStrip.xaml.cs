using Common;
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
    /// Logique d'interaction pour LiveryStrip.xaml
    /// </summary>
    public partial class LiveryStrip : UserControl
    {
        public LiveryStrip()
        {
            InitializeComponent();
            Box_LiveryStyle.ItemsSource = Enum.GetValues(typeof(LiveryType));
        }

        public CarLivery SelectedLivery { get; set; }

        public void Redraw(int size = 0)
        {
            Container.Children.Clear();

            LiveryWidget LW = new LiveryWidget();

            LW.Primary = Box1.SelectedColor;
            LW.Secondary = Box2.SelectedColor;
            LW.Ternary = Box3.SelectedColor;

            if (size == 0) size = (int)ActualHeight;
            switch (SelectedLivery.Type)
            {
                case LiveryType.Classic:
                default:
                    Container.Children.Add(LW.LiveryClassic(size));
                    break;
                case LiveryType.Racing:
                    Container.Children.Add(LW.LiveryRacing(size));
                    break;
                case LiveryType.Gradient:
                    Container.Children.Add(LW.LiveryGradient(size));
                    break;
            }
        }

        public void Set(CarLivery CL)
        {
            SelectedLivery = CL;
            Box1.Set(CL.Primary);
            Box2.Set(CL.Secondary);
            Box3.Set(CL.Ternary);
            Box_LiveryName.Text = CL.Name;
            Box_LiveryStyle.SelectedItem = CL.Type;
            Redraw();
        }

        private void Box_ColorPicked()
        {
            if (Box1.SelectedColor != null) SelectedLivery.PrimaryColor = Box1.SelectedColor.Name;
            else SelectedLivery.PrimaryColor = null;
            if (Box2.SelectedColor != null) SelectedLivery.SecondaryColor = Box2.SelectedColor.Name;
            else SelectedLivery.SecondaryColor = null;
            if (Box3.SelectedColor != null) SelectedLivery.TernaryColor = Box3.SelectedColor.Name;
            else SelectedLivery.TernaryColor = null;
            Redraw();
        }

        private void Box_LiveryStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedLivery.Type = (LiveryType)Box_LiveryStyle.SelectedItem;
            Redraw();
        }

        private void Box_LiveryName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectedLivery.Name = Box_LiveryName.Text;
        }
    }
}
