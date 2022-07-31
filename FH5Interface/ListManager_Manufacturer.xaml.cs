using Common;
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
    /// Logique d'interaction pour ListManager_Manufacturer.xaml
    /// </summary>
    public partial class ListManager_Manufacturer : UserControl
    {
        private enum Modes
        {
            Select,
            Edit,
            New,
        }

        public ListManager_Manufacturer()
        {
            InitializeComponent();

            BoxManf.ItemsSource = Lists.Manufacturers();

            TbxName.IsEnabled = false;
            TbxCode.IsEnabled = false;
            Mode = Modes.Select;
        }

        public void SetLM(ListManager lm) { LM = lm; }
        private ListManager LM;
        private Modes Mode { get; set; }

        public void SelectModel(Model mod)
        {
            if (Mode == Modes.Select && mod != null && mod.HasFamily)
            {
                BoxManf.SelectedItem = mod.Manufacturer;
            }
        }

        public void SelectManufacturer(Manufacturer manf)
        {
            if (Mode == Modes.Select && manf != null)
            {
                BoxManf.SelectedItem = manf;
            }
        }

        private bool IsManufacturerSelected
        {
            get
            {
                return (BoxManf.SelectedItem != null) && (BoxManf.SelectedItem is Manufacturer);
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

        private void UpdateMode()
        {
            switch (Mode)
            {
                case Modes.Select:
                    BoxManf.IsEnabled = true;
                    TbxName.IsEnabled = TbxCode.IsEnabled = false;
                    BtnNew.IsEnabled = true; BtnNew.Background = Brushes.Gainsboro; BtnNew.Visibility = Visibility.Visible;
                    BtnEdit.IsEnabled = true; BtnEdit.Background = Brushes.Gainsboro; BtnEdit.Visibility = Visibility.Visible;
                    BtnCancel.IsEnabled = false; BtnCancel.Background = Brushes.Gainsboro; BtnCancel.Visibility = Visibility.Collapsed;
                    UpdateInfo();
                    break;
                case Modes.Edit:
                    BoxManf.IsEnabled = false;
                    TbxName.IsEnabled = TbxCode.IsEnabled = true;
                    BtnNew.IsEnabled = false; BtnNew.Background = Brushes.Gainsboro; BtnNew.Visibility = Visibility.Collapsed;
                    BtnEdit.IsEnabled = true; BtnEdit.Background = Brushes.PaleGreen; BtnEdit.Visibility = Visibility.Visible;
                    BtnCancel.IsEnabled = true; BtnCancel.Background = Brushes.LightPink; BtnCancel.Visibility = Visibility.Visible;
                    break;
                case Modes.New:
                    BoxManf.IsEnabled = false;
                    TbxName.IsEnabled = TbxCode.IsEnabled = true;
                    BtnNew.IsEnabled = true; BtnNew.Background = Brushes.PaleGreen; BtnNew.Visibility = Visibility.Visible;
                    BtnEdit.IsEnabled = false; BtnEdit.Background = Brushes.Gainsboro; BtnEdit.Visibility = Visibility.Collapsed;
                    BtnCancel.IsEnabled = true; BtnCancel.Background = Brushes.LightPink; BtnCancel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void UpdateInfo()
        {
            TbxName.Text = TbxCode.Text = null;
            TbxCountryName.Content = null;
            ImgFlag.Source = ImgLogo.Source = null;

            if (IsManufacturerSelected)
            {
                TbxName.Text = SelectedManufacturer.Name;
                TbxCode.Text = SelectedManufacturer.CountryCode.ToUpper();
                TbxCountryName.Content = Data.GetCountryName(SelectedManufacturer.CountryCode);
                ImgFlag.Source = Data.GetCountryFlagBitmap(SelectedManufacturer.CountryCode);
                ImgLogo.Source = SelectedManufacturer.GetManfLogo();
                ImgLogoPath.Content = System.IO.Path.GetFileName(SelectedManufacturer.GetManfLogoPath());
            }
        }

        private void ValidateEdit()
        {
            SelectedManufacturer.Name = TbxName.Text;
            SelectedManufacturer.CountryCode = TbxCode.Text.ToLower();

            BoxManf.ItemsSource = Lists.Manufacturers();
            Mode = Modes.Select;
            LM.UpdateLists();
            ImportData.Quicksave();
        }

        private void ValidateNew()
        {
            Manufacturer MANF = new Manufacturer();
            MANF.Name = TbxName.Text;
            MANF.CountryCode = TbxCode.Text.ToLower();
            Lists.NewManufacturer(MANF);

            BoxManf.ItemsSource = Lists.Manufacturers();
            BoxManf.SelectedItem = MANF;
            Mode = Modes.Select;
            LM.UpdateLists();
            ImportData.Quicksave();
        }

        private void BoxManf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateInfo();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Mode == Modes.Select && IsManufacturerSelected)
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
    }
}
