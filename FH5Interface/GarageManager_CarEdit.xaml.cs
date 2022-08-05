using DataModel;
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
    /// Logique d'interaction pour GarageManager_CarEdit.xaml
    /// </summary>
    public partial class GarageManager_CarEdit : UserControl
    {
        private enum Modes
        {
            Select,
            Edit,
            New,
        }

        public GarageManager_CarEdit()
        {
            InitializeComponent();
            SelectedLivery = new CarLivery();

            Mode = Modes.Select;
            BoxManf.ItemsSource = Lists.Manufacturers();
            UpdateEngineBox();

            NbxSpd.Min = NbxAcc.Min = NbxBra.Min = NbxHnd.Min = NbxLau.Min = NbxOff.Min = 0D;
            NbxSpd.Max = NbxAcc.Max = NbxBra.Max = NbxHnd.Max = NbxLau.Max = NbxOff.Max = 10D;
            NbxPii.Min = 0L;
            NbxPii.Max = 999L;

            UpdateMode();
        }

        public void SetModel(Model model)
        {
            SelectedCar = new Car();
            SelectedCar.Model = model;
            BoxModl.SelectedItem = model;
            BoxManf.SelectedItem = model.Manufacturer;
            BtnNew_Click(null, null);
            UpdateInfo();
        }

        private Modes Mode { get; set; }
        private Car SelectedCar { get; set; }
        private CarLivery SelectedLivery { get; set; }

        public void SetCar(Car car)
        {
            if (car == null) { SelectedCar = null; return; }

            SelectedCar = car;
            BoxManf.SelectedItem = car.Model.Manufacturer;
            if (BoxModl.SelectedItem as Model == car.Model) BoxModl.SelectedItem = null;
            BoxModl.SelectedItem = car.Model;
            BoxEngine.SelectedItem = car.Engine;
        }

        private bool IsManufacturerSelected
        {
            get
            {
                return (BoxManf.SelectedItem != null) && (BoxManf.SelectedItem is Manufacturer);
            }
        }

        private bool IsModelSelected
        {
            get
            {
                return (BoxModl.SelectedItem != null) && (BoxModl.SelectedItem is Model);
            }
        }

        private bool ModelSortByYearFirst
        {
            get
            {
                return ModelSortGroup_Year.IsChecked == true;
            }
        }

        private void UpdateModelBoxes()
        {
            if (IsManufacturerSelected)
                BoxModl.ItemsSource = Lists.ModelsByManf((BoxManf.SelectedItem as FH5Data.Manufacturer), ModelSortByYearFirst);
            else
                BoxModl.ItemsSource = Lists.Models();
        }

        private bool EngineSortByForzaNameFirst { get { return EngineSortGroup_Forza.IsChecked == true; } }

        private void UpdateEngineBox()
        {
            BoxEngine.ItemsSource = Lists.Engines(EngineSortByForzaNameFirst);
        }

        private void UpdateMode()
        {
            switch (Mode)
            {
                case Modes.Select:
                    Box_Driven.IsEnabled = BoxModl.IsEnabled = BoxManf.IsEnabled = false;
                    ModelSortGroup_Year.IsEnabled = ModelSortGroup_Name.IsEnabled = false;
                    ContainerStats.IsEnabled = ContainerSetup.IsEnabled = ContainerLivery.IsEnabled = false;
                    BtnNew.IsEnabled = true; BtnNew.Background = Brushes.Gainsboro; BtnNew.Visibility = Visibility.Visible;
                    BtnEdit.IsEnabled = true; BtnEdit.Background = Brushes.Gainsboro; BtnEdit.Visibility = Visibility.Visible;
                    BtnCancel.IsEnabled = false; BtnCancel.Background = Brushes.Gainsboro; BtnCancel.Visibility = Visibility.Collapsed;
                    BtnPrev.IsEnabled = true; BtnPrev.Background = Brushes.Gainsboro; BtnPrev.Visibility = Visibility.Visible;
                    BtnNext.IsEnabled = true; BtnNext.Background = Brushes.Gainsboro; BtnNext.Visibility = Visibility.Visible;
                    BtnModl.IsEnabled = true; BtnModl.Background = Brushes.Gainsboro; BtnModl.Visibility = Visibility.Visible;
                    UpdateInfo();
                    break;
                case Modes.Edit:
                    Box_Driven.IsEnabled = BoxModl.IsEnabled = BoxManf.IsEnabled = true;
                    ModelSortGroup_Year.IsEnabled = ModelSortGroup_Name.IsEnabled = true;
                    ContainerStats.IsEnabled = ContainerSetup.IsEnabled = ContainerLivery.IsEnabled = true;
                    BtnNew.IsEnabled = false; BtnNew.Background = Brushes.Gainsboro; BtnNew.Visibility = Visibility.Collapsed;
                    BtnEdit.IsEnabled = true; BtnEdit.Background = Brushes.PaleGreen; BtnEdit.Visibility = Visibility.Visible;
                    BtnCancel.IsEnabled = true; BtnCancel.Background = Brushes.LightPink; BtnCancel.Visibility = Visibility.Visible;
                    BtnPrev.IsEnabled = false; BtnPrev.Background = Brushes.Gainsboro; BtnPrev.Visibility = Visibility.Collapsed;
                    BtnNext.IsEnabled = false; BtnNext.Background = Brushes.Gainsboro; BtnNext.Visibility = Visibility.Collapsed;
                    BtnModl.IsEnabled = false; BtnModl.Background = Brushes.Gainsboro; BtnModl.Visibility = Visibility.Collapsed;
                    break;
                case Modes.New:
                    Box_Driven.IsEnabled = BoxModl.IsEnabled = BoxManf.IsEnabled = true;
                    ModelSortGroup_Year.IsEnabled = ModelSortGroup_Name.IsEnabled = true;
                    BoxEngine.SelectedItem = EngineSwap.Stock;
                    ContainerStats.IsEnabled = ContainerSetup.IsEnabled = ContainerLivery.IsEnabled = true;
                    BtnNew.IsEnabled = true; BtnNew.Background = Brushes.PaleGreen; BtnNew.Visibility = Visibility.Visible;
                    BtnEdit.IsEnabled = false; BtnEdit.Background = Brushes.Gainsboro; BtnEdit.Visibility = Visibility.Collapsed;
                    BtnCancel.IsEnabled = true; BtnCancel.Background = Brushes.LightPink; BtnCancel.Visibility = Visibility.Visible;
                    BtnPrev.IsEnabled = false; BtnPrev.Background = Brushes.Gainsboro; BtnPrev.Visibility = Visibility.Collapsed;
                    BtnNext.IsEnabled = false; BtnNext.Background = Brushes.Gainsboro; BtnNext.Visibility = Visibility.Collapsed;
                    BtnModl.IsEnabled = false; BtnModl.Background = Brushes.Gainsboro; BtnModl.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void UpdateInfo()
        {
            if (SelectedCar == null && (BoxModl.SelectedItem == null || Mode == Modes.Select)) return;
            if (Mode == Modes.Select)
            {
                LblPii.Content = SelectedCar.Model.Stats.PI;
                LblSpd.Content = SelectedCar.Model.Stats.Speed;
                LblHnd.Content = SelectedCar.Model.Stats.Handling;
                LblAcc.Content = SelectedCar.Model.Stats.Acceleration;
                LblLau.Content = SelectedCar.Model.Stats.Launch;
                LblBra.Content = SelectedCar.Model.Stats.Braking;
                LblOff.Content = SelectedCar.Model.Stats.Offroad;

                NbxPii.Value = SelectedCar.Stats.PI;
                NbxSpd.Value = SelectedCar.Stats.Speed;
                NbxHnd.Value = SelectedCar.Stats.Handling;
                NbxAcc.Value = SelectedCar.Stats.Acceleration;
                NbxLau.Value = SelectedCar.Stats.Launch;
                NbxBra.Value = SelectedCar.Stats.Braking;
                NbxOff.Value = SelectedCar.Stats.Offroad;

                Box_Driven.IsChecked = SelectedCar.IsDriven;
                BoxEngine.SelectedItem = SelectedCar.Engine;

                switch (SelectedCar.Drivetrain)
                {
                    case Drive.RWD: Drivetrain_RWD.IsChecked = true; break;
                    case Drive.AWD: Drivetrain_AWD.IsChecked = true; break;
                    case Drive.FWD: Drivetrain_FWD.IsChecked = true; break;
                }
                switch (SelectedCar.Setup)
                {
                    case Setup.Road: Setup_Road.IsChecked = true; break;
                    case Setup.Offroad: Setup_Offr.IsChecked = true; break;
                    case Setup.Drift: Setup_Drif.IsChecked = true; break;
                    case Setup.Drag: Setup_Drag.IsChecked = true; break;
                }
                TbxSpec.Text = SelectedCar.SpecName;
                Box_Type.Content = SelectedCar.Model.Type + " - " + SelectedCar.Model.Rarity.GetName();
                ImportLivery(SelectedCar.Livery);
            }
            else if ((Mode == Modes.New || Mode == Modes.Select) && BoxModl.SelectedItem is Model)
            {
                Model mod = (BoxModl.SelectedItem as Model);
                LblPii.Content = mod.Stats.PI;
                LblSpd.Content = mod.Stats.Speed;
                LblHnd.Content = mod.Stats.Handling;
                LblAcc.Content = mod.Stats.Acceleration;
                LblLau.Content = mod.Stats.Launch;
                LblBra.Content = mod.Stats.Braking;
                LblOff.Content = mod.Stats.Offroad;

                NbxPii.Value = mod.Stats.PI;
                NbxSpd.Value = mod.Stats.Speed;
                NbxHnd.Value = mod.Stats.Handling;
                NbxAcc.Value = mod.Stats.Acceleration;
                NbxLau.Value = mod.Stats.Launch;
                NbxBra.Value = mod.Stats.Braking;
                NbxOff.Value = mod.Stats.Offroad;

                if (Mode == Modes.New) Box_Driven.IsChecked = false;
                else Box_Driven.IsChecked = SelectedCar.IsDriven;

                switch (mod.Drivetrain)
                {
                    default:
                    case Drive.RWD: Drivetrain_RWD.IsChecked = true; break;
                    case Drive.AWD: Drivetrain_AWD.IsChecked = true; break;
                    case Drive.FWD: Drivetrain_FWD.IsChecked = true; break;
                }

                Setup_Road.IsChecked = true;
                TbxSpec.Text = null;
                Box_Type.Content = mod.Type + " - " + mod.Rarity.GetName();
                ImportLivery(null);
            }
            else if (Mode == Modes.Edit && BoxModl.SelectedItem is Model)
            {
                Model mod = (BoxModl.SelectedItem as Model);
                LblPii.Content = mod.Stats.PI;
                LblSpd.Content = mod.Stats.Speed;
                LblHnd.Content = mod.Stats.Handling;
                LblAcc.Content = mod.Stats.Acceleration;
                LblLau.Content = mod.Stats.Launch;
                LblBra.Content = mod.Stats.Braking;
                LblOff.Content = mod.Stats.Offroad;
                Box_Type.Content = mod.Type + " - " + mod.Rarity.GetName();
            }
        }

        private void ImportLivery(CarLivery liv)
        {
            if (liv == null)
            {
                SelectedLivery.Name = null;
                SelectedLivery.PrimaryColor = null;
                SelectedLivery.SecondaryColor = null;
                SelectedLivery.TernaryColor = null;
                SelectedLivery.Type = LiveryType.Classic;
            }
            else
            {
                SelectedLivery.Name = liv.Name;
                SelectedLivery.PrimaryColor = liv.PrimaryColor;
                SelectedLivery.SecondaryColor = liv.SecondaryColor;
                SelectedLivery.TernaryColor = liv.TernaryColor;
                SelectedLivery.Type = liv.Type;
            }
            ContainerLivery.Set(SelectedLivery);
            ContainerLivery.Redraw(90);
        }

        private void ValidateEdit()
        {
            if (Drivetrain_RWD.IsChecked == true) SelectedCar.Drivetrain = Drive.RWD;
            else if (Drivetrain_AWD.IsChecked == true) SelectedCar.Drivetrain = Drive.AWD;
            else if (Drivetrain_FWD.IsChecked == true) SelectedCar.Drivetrain = Drive.FWD;

            SelectedCar.Livery.ImportLivery(SelectedLivery);
            SelectedCar.Model = BoxModl.SelectedItem as Model;

            if (Setup_Road.IsChecked == true) SelectedCar.Setup = Setup.Road;
            else if (Setup_Offr.IsChecked == true) SelectedCar.Setup = Setup.Offroad;
            else if (Setup_Drif.IsChecked == true) SelectedCar.Setup = Setup.Drift;
            else if (Setup_Drag.IsChecked == true) SelectedCar.Setup = Setup.Drag;

            SelectedCar.SpecName = TbxSpec.Text;
            SelectedCar.IsDriven = Box_Driven.IsChecked == true;

            SelectedCar.Stats.PI = (int)NbxPii.Value;
            SelectedCar.Stats.Speed = NbxSpd.Value;
            SelectedCar.Stats.Handling = NbxHnd.Value;
            SelectedCar.Stats.Acceleration = NbxAcc.Value;
            SelectedCar.Stats.Launch = NbxLau.Value;
            SelectedCar.Stats.Braking = NbxBra.Value;
            SelectedCar.Stats.Offroad = NbxOff.Value;

            SelectedCar.EngineID = (BoxEngine.SelectedItem as EngineSwap).EngineId;

            Mode = Modes.Select;
            ImportData.Quicksave();
        }
        private void ValidateNew()
        {
            Car CAR = new Car();

            CAR.Model = BoxModl.SelectedItem as Model;
            if (CAR.Model == null)
            {
                MessageBox.Show("No selected model", "Impossible");
                return;
            }

            if (Drivetrain_RWD.IsChecked == true) CAR.Drivetrain = Drive.RWD;
            else if (Drivetrain_AWD.IsChecked == true) CAR.Drivetrain = Drive.AWD;
            else if (Drivetrain_FWD.IsChecked == true) CAR.Drivetrain = Drive.FWD;

            CAR.Livery.ImportLivery(SelectedLivery);

            if (Setup_Road.IsChecked == true) CAR.Setup = Setup.Road;
            else if (Setup_Offr.IsChecked == true) CAR.Setup = Setup.Offroad;
            else if (Setup_Drif.IsChecked == true) CAR.Setup = Setup.Drift;
            else if (Setup_Drag.IsChecked == true) CAR.Setup = Setup.Drag;

            CAR.SpecName = TbxSpec.Text;
            CAR.IsDriven = Box_Driven.IsChecked == true;

            CAR.Stats.PI = (int)NbxPii.Value;
            CAR.Stats.Speed = NbxSpd.Value;
            CAR.Stats.Handling = NbxHnd.Value;
            CAR.Stats.Acceleration = NbxAcc.Value;
            CAR.Stats.Launch = NbxLau.Value;
            CAR.Stats.Braking = NbxBra.Value;
            CAR.Stats.Offroad = NbxOff.Value;

            CAR.EngineID = (BoxEngine.SelectedItem as EngineSwap).EngineId;

            Lists.NewCar(CAR);
            SelectedCar = CAR;
            Mode = Modes.Select;
            ImportData.Quicksave();
        }

        private void BoxManf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateModelBoxes();
        }

        private void ModelSortGroup_Checked(object sender, RoutedEventArgs e)
        {
            UpdateModelBoxes();
        }

        private void BoxModl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateInfo();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar == null)
            {
                MessageBox.Show("No selected car", "Impossible");
                return;
            }
            if (Mode == Modes.Select)
                Mode = Modes.Edit;
            else if (Mode == Modes.Edit)
                ValidateEdit();
            UpdateMode();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            if (Mode == Modes.Select)
                Mode = Modes.New;
            else if (Mode == Modes.New)
                ValidateNew();
            UpdateMode();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Mode = Modes.Select;
            UpdateMode();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar == null || Mode != Modes.Select) return;
            var list = Lists.Garage();
            var index = list.IndexOf(SelectedCar) - 1;
            if (index < 0) SetCar(list.Last());
            else SetCar(list[index]);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar == null || Mode != Modes.Select) return;
            var list = Lists.Garage();
            var index = list.IndexOf(SelectedCar) + 1;
            if (index >= list.Count) SetCar(list.First());
            else SetCar(list[index]);

        }

        private void BtnEditModel_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar == null || Mode != Modes.Select) return;
            var scd = new Window_StockCarData();
            scd.OpenModel(SelectedCar.Model);
            scd.ShowDialog();
            UpdateInfo();
        }

        private void Box_Driven_Checked(object sender, RoutedEventArgs e)
        {
            if (Box_Driven.IsChecked == true) Box_Driven.Content = "Driven";
            else Box_Driven.Content = "Not Driven";
        }

        private void EngineSortGroup_Checked(object sender, RoutedEventArgs e)
        {
            UpdateEngineBox();
        }
    }
}
