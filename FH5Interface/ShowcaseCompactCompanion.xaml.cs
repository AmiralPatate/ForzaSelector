using DataModel;
using FH5Data;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using UI;

namespace FH5Interface
{
    /// <summary>
    /// Logique d'interaction pour ShowcaseCompactCompanion.xaml
    /// </summary>
    public partial class ShowcaseCompactCompanion : Window
    {
        private Car SelectedCar { get; set; }
        public bool IsOpen { get; private set; }

        private Action<object, KeyEventArgs> OwnerKeyUp;

        public ShowcaseCompactCompanion(Car car, Action<object, KeyEventArgs> keyup)
        {
            OwnerKeyUp = keyup;
            InitializeComponent();
            SetCar(car);
            Topmost = true;
            BtnPin.ToolTip = "Pinned on foreground\nClick to unpin";
            IsOpen = true;
            KeyUp += ShowcaseCompactCompanion_KeyUp;
        }

        void ShowcaseCompactCompanion_KeyUp(object sender, KeyEventArgs e)
        {
            OwnerKeyUp(sender, e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsOpen = false;
        }

        public void SetCar(Car car)
        {
            SelectedCar = car;
            if (car == null)
            {
                Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                Visibility = System.Windows.Visibility.Visible;
                LoadInfo();
            }
        }

        private void LoadInfo()
        {
            Box_Year.Text = SelectedCar.Model.Year.ToString("0000");
            Box_Manf.Text = SelectedCar.Model.Manufacturer.Name;
            Box_Modl.Text = SelectedCar.Model.Name;
            Box_IsDriven.Stroke = SelectedCar.IsDriven ? Brushes.White : Brushes.Transparent;
            ModelContainer.ToolTip = SelectedCar.Model.ToString();

            Box_Rare.Content = SelectedCar.Model.Rarity == Rarity.ForzaEdition ? "Forza Ed." : SelectedCar.Model.Rarity.ToString();
            Box_Rare.Background = SelectedCar.Model.Rarity.Color();
            Box_Type.Content = SelectedCar.Model.HasType ? SelectedCar.Model.Type.Name : "";
            Box_PCla.Content = SelectedCar.Stats.PI.ClassFromPi();
            Box_PCla.Background = SelectedCar.Stats.PI.ClassFromPi().Color();
            Box_PInd.Content = SelectedCar.Stats.PI.ToString("000");
            Box_PInd.BorderBrush = SelectedCar.Stats.PI.ClassFromPi().Color();

            Box_Driv.Content = SelectedCar.Drivetrain.ToString();
            Box_Setp.Content = SelectedCar.Setup.ToString();

            RedrawLivery();
            Box_Livn.Text = SelectedCar.Livery.Name;

            Box_CSpd.Content = SelectedCar.Model.Stats.Speed.ToString("0.0", CultureInfo.InvariantCulture);
            Box_CHnd.Content = SelectedCar.Model.Stats.Handling.ToString("0.0", CultureInfo.InvariantCulture);
            Box_CAcc.Content = SelectedCar.Model.Stats.Acceleration.ToString("0.0", CultureInfo.InvariantCulture);
            Box_CLau.Content = SelectedCar.Model.Stats.Launch.ToString("0.0", CultureInfo.InvariantCulture);
            Box_CBra.Content = SelectedCar.Model.Stats.Braking.ToString("0.0", CultureInfo.InvariantCulture);
            Box_COff.Content = SelectedCar.Model.Stats.Offroad.ToString("0.0", CultureInfo.InvariantCulture);
            Box_CEng.Content = SelectedCar.Engine.IsStock ? "" : SelectedCar.Engine.ShortName;
            Box_CSpe.Content = SelectedCar.HasCustomSpecs ? SelectedCar.SpecName : "";
        }

        private void RedrawLivery()
        {
            Box_Livr.Children.Clear();

            LiveryWidget LW = new LiveryWidget();
            LW.Primary = SelectedCar.Livery.Primary;
            LW.Secondary = SelectedCar.Livery.Secondary;
            LW.Ternary = SelectedCar.Livery.Ternary;
            switch (SelectedCar.Livery.Type)
            {
                case LiveryType.Classic:
                default:
                    Box_Livr.Children.Add(LW.LiveryClassic(50));
                    break;
                case LiveryType.Racing:
                    Box_Livr.Children.Add(LW.LiveryRacing(50));
                    break;
                case LiveryType.Gradient:
                    Box_Livr.Children.Add(LW.LiveryGradient(50));
                    break;
            }
        }

        private void BtnPin_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            if (Topmost)
            {
                BtnPin.ToolTip = "Pinned on foreground\nClick to unpin";
                PinDraw.Fill = Brushes.LimeGreen;
            }
            else
            {
                BtnPin.ToolTip = "Click to pin on foreground";
                PinDraw.Fill = Brushes.Transparent;
            }
        }
    }
}
