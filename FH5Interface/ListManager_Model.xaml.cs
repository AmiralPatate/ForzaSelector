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
    /// Logique d'interaction pour ListManager_Model.xaml
    /// </summary>
    public partial class ListManager_Model : UserControl
    {
        private enum Modes
        {
            Select,
            EditModel,
            NewModel,
        }

        public ListManager_Model()
        {
            Mode = Modes.Select;
            InitializeComponent();

            BoxManf.ItemsSource = Lists.Manufacturers();
            BoxModl.ItemsSource = Lists.Models();

            TbxYear.Value = (long)DateTime.Now.Year;
            TbxYear.Min = 1000L;
            TbxYear.Max = 9999L;

            NbxSpd.Min = NbxAcc.Min = NbxBra.Min = NbxHnd.Min = NbxLau.Min = NbxOff.Min = 0D;
            NbxSpd.Max = NbxAcc.Max = NbxBra.Max = NbxHnd.Max = NbxLau.Max = NbxOff.Max = 10D;
            NbxPii.Min = 0L;
            NbxPii.Max = 999L;
            
            Drivetrain_RWD.IsEnabled = Drivetrain_AWD.IsEnabled = Drivetrain_FWD.IsEnabled = false;

            TbxManf.ItemsSource = Lists.Manufacturers();
            TbxType.ItemsSource = Lists.Types();
            TbxFaml.ItemsSource = Lists.Families();
            TbxRare.ItemsSource = Enum.GetValues(typeof(Rarity));
        }

        public void SetLM(ListManager lm) { LM = lm; }
        private ListManager LM;
        private Modes Mode;

        public void UpdateLists()
        {
            BoxManf.ItemsSource = Lists.Manufacturers();
            TbxManf.ItemsSource = Lists.Manufacturers();
            TbxFaml.ItemsSource = Lists.Families();
            TbxType.ItemsSource = Lists.Types();
        }

        public void SelectModel(Model mod, bool force = false)
        {
            if (Mode == Modes.Select)
            {
                if (IsManufacturerSelected || force) BoxManf.SelectedItem = mod.Manufacturer;
                BoxModl.SelectedItem = mod;
            }
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

        private Manufacturer SelectedManufacturer
        {
            get
            {
                if (IsManufacturerSelected)
                    return BoxManf.SelectedItem as Manufacturer;
                else
                    return null;
            }
        }

        private Model SelectedModel
        {
            get
            {
                if (IsModelSelected)
                    return BoxModl.SelectedItem as Model;
                else
                    return null;
            }
        }

        private void UpdateModelBoxes()
        {
            if (IsManufacturerSelected)
                BoxModl.ItemsSource = Lists.ModelsByManf((BoxManf.SelectedItem as FH5Data.Manufacturer), ModelSortByYearFirst);
            else
                BoxModl.ItemsSource = Lists.Models();
        }

        private void UpdateModelInfo()
        {
            TbxFaml.ItemsSource = Lists.Families();
            TbxYear.Value = (long)DateTime.Now.Year;
            TbxManf.SelectedItem = null;
            TbxName.Text = "";
            TbxFaml.SelectedItem = null;
            TbxType.SelectedItem = null;
            NbxSpd.Value = NbxAcc.Value = NbxBra.Value = NbxHnd.Value = NbxLau.Value = NbxOff.Value = 0D;
            NbxPii.Value = 0L;

            if (IsModelSelected)
            {
                TbxRare.SelectedItem = SelectedModel.Rarity;
                TbxYear.Value = SelectedModel.Year;
                TbxName.Text = SelectedModel.Name;
                TbxManf.SelectedItem = SelectedModel.Manufacturer;

                if (SelectedModel.HasFamily)
                    TbxFaml.SelectedItem = SelectedModel.ModelFamily;

                if (SelectedModel.HasType)
                    TbxType.SelectedItem = SelectedModel.Type;

                if (SelectedModel.Stats != null)
                {
                    NbxSpd.Value = SelectedModel.Stats.Speed;
                    NbxAcc.Value = SelectedModel.Stats.Acceleration;
                    NbxBra.Value = SelectedModel.Stats.Braking;
                    NbxHnd.Value = SelectedModel.Stats.Handling;
                    NbxLau.Value = SelectedModel.Stats.Launch;
                    NbxOff.Value = SelectedModel.Stats.Offroad;
                    NbxPii.Value = SelectedModel.Stats.PI;
                }

                switch (SelectedModel.Drivetrain)
                {
                    default:
                    case Drive.RWD: Drivetrain_RWD.IsChecked = true; break;
                    case Drive.AWD: Drivetrain_AWD.IsChecked = true; break;
                    case Drive.FWD: Drivetrain_FWD.IsChecked = true; break;
                }
            }
        }

        private void BoxManf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateModelBoxes();
            LM.SelectManufacturer(SelectedManufacturer);
        }

        private void ModelSortGroup_Checked(object sender, RoutedEventArgs e)
        {
            UpdateModelBoxes();
        }

        private void BoxModl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateModelInfo();
            if (SelectedModel != null)
            {
                LM.SelectModel(SelectedModel);
                LM.SelectManufacturer(SelectedModel.Manufacturer);
            }
        }

        private void UpdateMode()
        {
            switch (Mode)
            {
                case Modes.Select:
                    BoxManf.IsEnabled = BoxModl.IsEnabled = ModelSortGroup_Year.IsEnabled = ModelSortGroup_Name.IsEnabled = true;
                    TbxYear.IsEnabled = TbxManf.IsEnabled = TbxName.IsEnabled = TbxFaml.IsEnabled = TbxType.IsEnabled = TbxRare.IsEnabled = false;
                    BtnEdit.Visibility = Visibility.Visible; BtnEdit.IsEnabled = true; BtnEdit.Background = Brushes.Gainsboro;
                    BtnModl.Visibility = Visibility.Visible; BtnModl.IsEnabled = true; BtnModl.Background = Brushes.Gainsboro;
                    BtnCanc.Visibility = Visibility.Collapsed; BtnCanc.IsEnabled = false; BtnCanc.Background = Brushes.LightPink;
                    NbxSpd.IsEnabled = NbxAcc.IsEnabled = NbxBra.IsEnabled = NbxHnd.IsEnabled = NbxLau.IsEnabled = NbxOff.IsEnabled = NbxPii.IsEnabled = false;
                    Drivetrain_RWD.IsEnabled = Drivetrain_AWD.IsEnabled = Drivetrain_FWD.IsEnabled = false;
                    UpdateModelInfo();
                    break;
                case Modes.EditModel:
                    BoxManf.IsEnabled = BoxModl.IsEnabled = ModelSortGroup_Year.IsEnabled = ModelSortGroup_Name.IsEnabled = false;
                    TbxYear.IsEnabled = TbxManf.IsEnabled = TbxName.IsEnabled = TbxFaml.IsEnabled = TbxType.IsEnabled = TbxRare.IsEnabled = true;
                    BtnEdit.Visibility = Visibility.Visible; BtnEdit.IsEnabled = true; BtnEdit.Background = Brushes.PaleGreen;
                    BtnModl.Visibility = Visibility.Collapsed; BtnModl.IsEnabled = false; BtnModl.Background = Brushes.Gainsboro;
                    BtnCanc.Visibility = Visibility.Visible; BtnCanc.IsEnabled = true; BtnCanc.Background = Brushes.LightPink;
                    NbxSpd.IsEnabled = NbxAcc.IsEnabled = NbxBra.IsEnabled = NbxHnd.IsEnabled = NbxLau.IsEnabled = NbxOff.IsEnabled = NbxPii.IsEnabled = true;
                    Drivetrain_RWD.IsEnabled = Drivetrain_AWD.IsEnabled = Drivetrain_FWD.IsEnabled = true;
                    break;
                case Modes.NewModel:
                    BoxManf.IsEnabled = BoxModl.IsEnabled = ModelSortGroup_Year.IsEnabled = ModelSortGroup_Name.IsEnabled = false;
                    TbxYear.IsEnabled = TbxManf.IsEnabled = TbxName.IsEnabled = TbxFaml.IsEnabled = TbxType.IsEnabled = TbxRare.IsEnabled = true;
                    BtnEdit.Visibility = Visibility.Collapsed; BtnEdit.IsEnabled = false; BtnEdit.Background = Brushes.Gainsboro;
                    BtnModl.Visibility = Visibility.Visible; BtnModl.IsEnabled = true; BtnModl.Background = Brushes.PaleGreen;
                    BtnCanc.Visibility = Visibility.Visible; BtnCanc.IsEnabled = true; BtnCanc.Background = Brushes.LightPink;
                    NbxSpd.IsEnabled = NbxAcc.IsEnabled = NbxBra.IsEnabled = NbxHnd.IsEnabled = NbxLau.IsEnabled = NbxOff.IsEnabled = NbxPii.IsEnabled = true;
                    Drivetrain_RWD.IsEnabled = Drivetrain_AWD.IsEnabled = Drivetrain_FWD.IsEnabled = true;
                    break;
            }
        }

        private void ValidateEdit()
        {
            SelectedModel.Year = (int)TbxYear.Value;
            SelectedModel.Name = TbxName.Text.Trim();
            SelectedModel.ModelFamily = TbxFaml.SelectedItem as string;
            SelectedModel.Manufacturer = TbxManf.SelectedItem as Manufacturer;
            SelectedModel.Type = TbxType.SelectedItem as CarType;
            SelectedModel.Rarity = (Rarity)TbxRare.SelectedItem;

            if (SelectedModel.Stats == null) SelectedModel.Stats = new CarStats();
            SelectedModel.Stats.Speed = (double)NbxSpd.Value;
            SelectedModel.Stats.Acceleration = (double)NbxAcc.Value;
            SelectedModel.Stats.Braking = (double)NbxBra.Value;
            SelectedModel.Stats.Handling = (double)NbxHnd.Value;
            SelectedModel.Stats.Launch = (double)NbxLau.Value;
            SelectedModel.Stats.Offroad = (double)NbxOff.Value;
            SelectedModel.Stats.PI = (int)NbxPii.Value;

            if (Drivetrain_RWD.IsChecked == true) SelectedModel.Drivetrain = Drive.RWD;
            else if (Drivetrain_AWD.IsChecked == true) SelectedModel.Drivetrain = Drive.AWD;
            else if (Drivetrain_FWD.IsChecked == true) SelectedModel.Drivetrain = Drive.FWD;

            UpdateModelBoxes();
            Mode = Modes.Select;
            LM.UpdateLists();
            ImportData.Quicksave();
        }

        private void ValidateCreation()
        {
            Model MDL = new Model();

            MDL.Year = (int)TbxYear.Value;
            MDL.Name = TbxName.Text.Trim();
            MDL.ModelFamily = TbxFaml.SelectedItem as string;
            MDL.Manufacturer = TbxManf.SelectedItem as Manufacturer;
            MDL.Type = TbxType.SelectedItem as CarType;
            MDL.Rarity = (Rarity)TbxRare.SelectedItem;

            CarStats STA = new CarStats();
            MDL.Stats = STA;
            MDL.Stats.Speed = (double)NbxSpd.Value;
            MDL.Stats.Acceleration = (double)NbxAcc.Value;
            MDL.Stats.Braking = (double)NbxBra.Value;
            MDL.Stats.Handling = (double)NbxHnd.Value;
            MDL.Stats.Launch = (double)NbxLau.Value;
            MDL.Stats.Offroad = (double)NbxOff.Value;
            MDL.Stats.PI = (int)NbxPii.Value;

            if (Drivetrain_RWD.IsChecked == true) MDL.Drivetrain = Drive.RWD;
            else if (Drivetrain_AWD.IsChecked == true) MDL.Drivetrain = Drive.AWD;
            else if (Drivetrain_FWD.IsChecked == true) MDL.Drivetrain = Drive.FWD;

            Lists.NewModel(MDL);
            UpdateModelBoxes();
            BoxModl.SelectedItem = MDL;
            Mode = Modes.Select;
            LM.UpdateLists();
            ImportData.Quicksave();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Mode == Modes.Select && SelectedModel != null)
                Mode = Modes.EditModel;
            else if (Mode == Modes.EditModel)
                ValidateEdit();
            UpdateMode();
        }

        private void BtnModl_Click(object sender, RoutedEventArgs e)
        {
            if (Mode == Modes.Select)
                Mode = Modes.NewModel;
            else if (Mode == Modes.NewModel)
                ValidateCreation();
            UpdateMode();
        }

        private void BtnCanc_Click(object sender, RoutedEventArgs e)
        {
            Mode = Modes.Select;
            UpdateMode();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedModel == null || Mode != Modes.Select) return;
            var list = Lists.Models();
            var index = list.IndexOf(SelectedModel) + 1;
            if (index >= list.Count) SelectModel(list.First(), true);
            else SelectModel(list[index], true);
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedModel == null || Mode != Modes.Select) return;
            var list = Lists.Models();
            var index = list.IndexOf(SelectedModel) - 1;
            if (index < 0) SelectModel(list.Last(), true);
            else SelectModel(list[index], true);
        }

        private void BtnNcar_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedModel == null) return;
            var ce = new Window_CarEdit(SelectedModel);
            ce.ShowDialog();
        }
    }
}
