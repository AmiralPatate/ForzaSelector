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
    /// Logique d'interaction pour ListManager_Family.xaml
    /// </summary>
    public partial class ListManager_Family : UserControl
    {
        private enum Modes
        {
            Select,
            EditRename,
            EditRemove,
            New,
        }

        public ListManager_Family()
        {
            InitializeComponent();
            FamContainer.ItemsSource = Lists.Families(true);

            Mode = Modes.Select;
        }

        public void SetLM(ListManager lm) { LM = lm; }
        private ListManager LM;
        private Modes Mode;

        public void UpdateLists()
        {
            FamContainer.ItemsSource = Lists.Families(true);
            if (SelectedFamily != null && Mode == Modes.Select)
            {
                FamContainer.SelectedItem = SelectedFamily;
                ListContainer.ItemsSource = Lists.ModelsByFam(SelectedFamily);
            }
        }

        public void SelectModel(Model mod)
        {
            if (Mode == Modes.Select && mod != null && mod.HasFamily)
            {
                FamContainer.SelectedItem = mod.ModelFamily;
                ListContainer.SelectedItem = mod;
            }
        }

        private string SelectedFamily
        {
            get
            {
                if (FamContainer.SelectedItem != null && (string)FamContainer.SelectedItem != string.Empty)
                    return FamContainer.SelectedItem as string;
                else
                    return null;
            }
        }

        private void UpdateMode()
        {
            switch (Mode)
            {
                case Modes.Select:
                    FamContainer.IsEnabled = true;
                    ListContainer.IsEnabled = true;
                    TbxName.IsEnabled = false;
                    BtnRemove.IsEnabled = true; BtnRemove.Background = Brushes.Gainsboro; BtnRemove.Visibility = Visibility.Visible;
                    BtnNew.IsEnabled = true; BtnNew.Background = Brushes.Gainsboro; BtnNew.Visibility = Visibility.Visible;
                    BtnEdit.IsEnabled = true; BtnEdit.Background = Brushes.Gainsboro; BtnEdit.Visibility = Visibility.Visible;
                    BtnCancel.IsEnabled = false; BtnCancel.Background = Brushes.Gainsboro; BtnCancel.Visibility = Visibility.Collapsed;
                    //UpdateInfo();
                    break;
                case Modes.EditRename:
                    FamContainer.IsEnabled = false;
                    ListContainer.IsEnabled = false;
                    TbxName.IsEnabled = true;
                    BtnRemove.IsEnabled = false; BtnRemove.Background = Brushes.Gainsboro; BtnRemove.Visibility = Visibility.Collapsed;
                    BtnNew.IsEnabled = false; BtnNew.Background = Brushes.Gainsboro; BtnNew.Visibility = Visibility.Collapsed;
                    BtnEdit.IsEnabled = true; BtnEdit.Background = Brushes.PaleGreen; BtnEdit.Visibility = Visibility.Visible;
                    BtnCancel.IsEnabled = true; BtnCancel.Background = Brushes.LightPink; BtnCancel.Visibility = Visibility.Visible;
                    break;
                case Modes.EditRemove:
                    FamContainer.IsEnabled = false;
                    ListContainer.IsEnabled = true;
                    TbxName.IsEnabled = false;
                    BtnRemove.IsEnabled = true; BtnRemove.Background = Brushes.PaleGreen; BtnRemove.Visibility = Visibility.Visible;
                    BtnNew.IsEnabled = false; BtnNew.Background = Brushes.Gainsboro; BtnNew.Visibility = Visibility.Collapsed;
                    BtnEdit.IsEnabled = false; BtnEdit.Background = Brushes.Gainsboro; BtnEdit.Visibility = Visibility.Collapsed;
                    BtnCancel.IsEnabled = true; BtnCancel.Background = Brushes.LightPink; BtnCancel.Visibility = Visibility.Visible;
                    break;
                case Modes.New:
                    FamContainer.IsEnabled = false;
                    ListContainer.IsEnabled = false;
                    TbxName.IsEnabled = true;
                    BtnRemove.IsEnabled = false; BtnRemove.Background = Brushes.Gainsboro; BtnRemove.Visibility = Visibility.Collapsed;
                    BtnNew.IsEnabled = true; BtnNew.Background = Brushes.PaleGreen; BtnNew.Visibility = Visibility.Visible;
                    BtnEdit.IsEnabled = false; BtnEdit.Background = Brushes.Gainsboro; BtnEdit.Visibility = Visibility.Collapsed;
                    BtnCancel.IsEnabled = true; BtnCancel.Background = Brushes.LightPink; BtnCancel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ValidateRename()
        {
            Lists.RenameFamily(SelectedFamily, TbxName.Text);

            FamContainer.ItemsSource = Lists.Families(true);
            FamContainer.SelectedItem = TbxName.Text;
            Mode = Modes.Select;
            LM.UpdateLists();
            ImportData.Quicksave();
        }

        private void ValidateRemove()
        {
            foreach (var item in ListContainer.SelectedItems)
            {
                if (item is Model)
                {
                    (item as Model).ModelFamily = string.Empty;
                }
            }

            ListContainer.ItemsSource = Lists.ModelsByFam(SelectedFamily);
            Mode = Modes.Select;
            LM.UpdateLists();
            ImportData.Quicksave();
        }

        private void ValidateNew()
        {
            Lists.NewFamily(TbxName.Text);

            FamContainer.ItemsSource = Lists.Families(true);
            FamContainer.SelectedItem = TbxName.Text;
            Mode = Modes.Select;
            LM.UpdateLists();
            ImportData.Quicksave();
        }

        private void FamContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListContainer.ItemsSource = Lists.ModelsByFam(SelectedFamily);
            TbxName.Text = SelectedFamily;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Mode == Modes.Select && SelectedFamily != null)
                Mode = Modes.EditRename;
            else if (Mode == Modes.EditRename)
                ValidateRename();
            UpdateMode();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (Mode == Modes.Select && SelectedFamily != null)
                Mode = Modes.EditRemove;
            else if (Mode == Modes.EditRemove)
                ValidateRemove();
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

        private void ListContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListContainer.SelectedItems.Count == 1)
                LM.SelectModel_FromFam(ListContainer.SelectedItems[0] as Model);
        }
    }
}
