using Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI;

namespace FH5Interface
{
    /// <summary>
    /// Logique d'interaction pour Main_Showcase.xaml
    /// </summary>
    public partial class Main_Showcase : UserControl
    {
        private Car SelectedCar { get; set; }
        private Car ComparisonCar { get; set; }
        private Filter Filter { get; set; }

        private ShowcaseCompactCompanion SCC;
        private Window_Notetaker WNTK;
        private Window_ColorConverter WCCV;

        public Main_Showcase()
        {
            InitializeComponent();
            Box_Mini.SetSubfolder("fh5");
            FilterContainer.FilterUpdated += FilterContainer_FilterUpdated;
            ListContainer.ItemsSource = Lists.Garage();
            SetCar(Lists.RandomCar());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).KeyUp += UserControl_KeyUp;
            ListContainer.Focus();
            if (SelectedCar != null && SelectedCar.IsDriven && TopContainer.ActualWidth > Box_IsDriven.ActualWidth) ColCarName.MaxWidth = TopContainer.ActualWidth - (Box_IsDriven.ActualWidth + Box_IsDriven.Margin.Left + Box_IsDriven.Margin.Right);
            else ColCarName.MaxWidth = TopContainer.ActualWidth + Math.Max(0, -(Box_IsDriven.Margin.Left + Box_IsDriven.Margin.Right));
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (WNTK != null && WNTK.IsOpen) WNTK.Close();
            if (WCCV != null && WCCV.IsOpen) WCCV.Close();
            if (SCC != null && SCC.IsOpen) SCC.Close();
            WNTK = null;
            WCCV = null;
            SCC = null;
        }

        public void SetCar(Car car)
        {
            if (car == null) { UpdateNull(); return; }

            SelectedCar = car;
            if (Filter != null) ListContainer.ItemsSource = Filter.Matches(Lists.Garage());
            else ListContainer.ItemsSource = Lists.Garage();
            ListContainer.SelectedItem = car;
            ListContainer.ScrollIntoView(car);
            UpdateInfo();
            ListContainer.Focus();
            if (SCC != null && SCC.IsOpen) SCC.SetCar(SelectedCar);
        }

        private void UpdateNull()
        {
            Box_Manf.Text = "NO CAR TO SELECT";
            Box_Cols.Text = Box_Livn.Text = Box_Faml.Text = Box_Type.Text = Box_Rare.Text = Box_Modl.Text = Box_Year.Text = "";
            Box_TLog.Source = Box_Flag.Source = null;
            Box_IsDriven.Stroke = Brushes.Transparent;
            Box_SNam.Content = Box_CSpe.Content = Box_Setp.Content = Box_Driv.Content = Box_PCla.Content = Box_PInd.Content = "";
            Box_WhFL.Background = Box_WhFR.Background = Box_WhRL.Background = Box_WhRR.Background = Box_AxFr.Background = Box_AxRr.Background = Box_AxTr.Background = Brushes.Gray;
            BoxTitleR2.Visibility = BoxTitleR1.Visibility = DriveContainerCmp.Visibility = Box_DrivCmp.Visibility = Box_SetpCmp.Visibility = VSLabel.Visibility = Visibility.Collapsed;

            if (Filter != null) ListContainer.ItemsSource = Filter.Matches(Lists.Garage());
            else ListContainer.ItemsSource = Lists.Garage();
        }

        private void FilterContainer_FilterUpdated(Filter filter)
        {
            Filter = filter;
            ListContainer.ItemsSource = Filter.Matches(Lists.Garage());
        }

        private void UpdateInfo()
        {
            Box_Year.Text = SelectedCar.Model.Year.ToString("0000");
            Box_Manf.Text = SelectedCar.Model.Manufacturer.Name;
            Box_Flag.Source = Data.GetCountryFlagBitmap(SelectedCar.Model.Manufacturer.CountryCode);
            Box_Modl.Text = SelectedCar.Model.Name;
            Box_Logo.Source = SelectedCar.Model.Manufacturer.GetManfLogo();
            if (SelectedCar.Model.Type != null) Box_TLog.Source = SelectedCar.Model.Type.Logo;

            Box_CarNo.Text = "ID#" + SelectedCar.CarNumber.ToString("0000");
            Box_Mini.CarNumber = SelectedCar.CarNumber;
            ContainerCarName.ToolTip = SelectedCar.Model.ToString();

            Box_IsDriven.Visibility = SelectedCar.IsDriven ? Visibility.Visible : Visibility.Collapsed;

            RedrawLivery();
            Box_Rare.Text = SelectedCar.Model.Rarity.GetName().ToUpper();
            Box_Rare.Background = SelectedCar.Model.Rarity.Color();
            Box_Livn.Text = SelectedCar.Livery.Name;
            Box_Type.Text = SelectedCar.Model.HasType ? SelectedCar.Model.Type.Name : "";
            Box_Faml.Text = SelectedCar.Model.HasFamily ? SelectedCar.Model.ModelFamily + " family" : "";

            Box_Cols.Text = "";
            if (SelectedCar.Livery.Primary != null) Box_Cols.Text += SelectedCar.Livery.Primary.Name;
            if (SelectedCar.Livery.Secondary != null) Box_Cols.Text += "\n" + SelectedCar.Livery.Secondary.Name;
            if (SelectedCar.Livery.Ternary != null) Box_Cols.Text += "\n" + SelectedCar.Livery.Ternary.Name;

            Box_PCla.Content = SelectedCar.Stats.PI.ClassFromPi();
            Box_PInd.Content = SelectedCar.Stats.PI.ToString("000");
            Box_PCla.Background = Box_PInd.BorderBrush = SelectedCar.Stats.PI.ClassFromPi().Color();

            Brush FrontBrush = (SelectedCar.Drivetrain == Drive.RWD) ? Brushes.Gray : Brushes.White;
            Brush RearBrush = (SelectedCar.Drivetrain == Drive.FWD) ? Brushes.Gray : Brushes.White;
            Brush TransversalBrush = (SelectedCar.Drivetrain == Drive.AWD) ? Brushes.White : Brushes.Gray;

            if (SelectedCar.HasDriveSwap)
            {
                Brush RMbrush = new SolidColorBrush(Color.FromRgb(150, 110, 110));
                Brush ADbrush = new SolidColorBrush(Color.FromRgb(220, 255, 220));
                Brush NObrush = Brushes.Gray;
                Brush YSbrush = Brushes.White;

                //Drive wheels added
                if (SelectedCar.Drivetrain == Drive.AWD)
                {
                    FrontBrush = (SelectedCar.Model.Drivetrain == Drive.FWD) ? YSbrush : ADbrush;
                    RearBrush = (SelectedCar.Model.Drivetrain == Drive.RWD) ? YSbrush : ADbrush;
                    TransversalBrush = ADbrush;
                }
                //Drive wheel removed
                else if (SelectedCar.Model.Drivetrain == Drive.AWD)
                {
                    FrontBrush = (SelectedCar.Drivetrain == Drive.RWD) ? RMbrush : YSbrush;
                    RearBrush = (SelectedCar.Drivetrain == Drive.FWD) ? RMbrush : YSbrush;
                    TransversalBrush = RMbrush;
                }
                //Drive wheel moved
                else
                {
                    FrontBrush = (SelectedCar.Drivetrain == Drive.RWD) ? RMbrush : YSbrush;
                    RearBrush = (SelectedCar.Drivetrain == Drive.FWD) ? RMbrush : YSbrush;
                    TransversalBrush = (SelectedCar.Drivetrain == Drive.AWD) ? YSbrush : NObrush;
                }
            }

            Box_WhFL.Background = Box_WhFR.Background = Box_AxFr.Background = FrontBrush;
            Box_WhRL.Background = Box_WhRR.Background = Box_AxRr.Background = RearBrush;
            Box_AxTr.Background = TransversalBrush;

            Box_Driv.Content = SelectedCar.Drivetrain.ToString();
            Box_Setp.Content = SelectedCar.Setup.ToString();

            Box_CSpd.Content = SelectedCar.Stats.Speed.ToString("0.0", CultureInfo.InvariantCulture);
            Box_CHnd.Content = SelectedCar.Stats.Handling.ToString("0.0", CultureInfo.InvariantCulture);
            Box_CAcc.Content = SelectedCar.Stats.Acceleration.ToString("0.0", CultureInfo.InvariantCulture);
            Box_CLau.Content = SelectedCar.Stats.Launch.ToString("0.0", CultureInfo.InvariantCulture);
            Box_CBra.Content = SelectedCar.Stats.Braking.ToString("0.0", CultureInfo.InvariantCulture);
            Box_COff.Content = SelectedCar.Stats.Offroad.ToString("0.0", CultureInfo.InvariantCulture);

            string specName = (SelectedCar.HasDriveSwap ? SelectedCar.Drivetrain.ToString() : "");
            specName += (SelectedCar.Engine.IsStock ? "" : (string.IsNullOrEmpty(specName) ? "" : " - ") + SelectedCar.Engine.ShortName);
            specName += (SelectedCar.HasCustomSpecs ? (string.IsNullOrEmpty(specName) ? "" : " - ") + SelectedCar.SpecName : "");
            Box_CSpe.Content = specName;

            Box_PClaCmp.Visibility = Box_PIndCmp.Visibility = DriveContainerCmp.Visibility = Box_DrivCmp.Visibility = Box_SetpCmp.Visibility =
                VSLabel.Visibility = BoxTitleR1.Visibility = BoxTitleR2.Visibility =
                ComparisonCar != null ? Visibility.Visible : Visibility.Collapsed;

            if (ComparisonCar != null)
            {
                Box_CSpe.Content = SelectedCar.Model.ToString() + (string.IsNullOrEmpty(specName) ? "" : " - ") + specName;

                Box_SSpd.Content = ComparisonCar.Stats.Speed.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SHnd.Content = ComparisonCar.Stats.Handling.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SAcc.Content = ComparisonCar.Stats.Acceleration.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SLau.Content = ComparisonCar.Stats.Launch.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SBra.Content = ComparisonCar.Stats.Braking.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SOff.Content = ComparisonCar.Stats.Offroad.ToString("0.0", CultureInfo.InvariantCulture);

                string compSpecName = (ComparisonCar.HasDriveSwap ? ComparisonCar.Drivetrain.ToString() : "");
                compSpecName += (ComparisonCar.Engine.IsStock ? "" : (string.IsNullOrEmpty(compSpecName) ? "" : " - ") + ComparisonCar.Engine.ShortName);
                compSpecName += (ComparisonCar.HasCustomSpecs ? (string.IsNullOrEmpty(compSpecName) ? "" : " - ") + ComparisonCar.SpecName : "");
                Box_SNam.Content = ComparisonCar.Model.ToString() + (string.IsNullOrEmpty(compSpecName) ? "" : " - ") + compSpecName;

                Box_SNam.Foreground = Brushes.White;

                BoxTitleR1.Text = ComparisonCar.Model.Year + " " + ComparisonCar.Model.Manufacturer.Name;
                BoxTitleR2.Text = ComparisonCar.Model.Name;

                Box_PClaCmp.Content = ComparisonCar.Stats.PI.ClassFromPi();
                Box_PIndCmp.Content = ComparisonCar.Stats.PI.ToString("000");
                Box_PClaCmp.Background = Box_PIndCmp.BorderBrush = ComparisonCar.Stats.PI.ClassFromPi().Color();

                FrontBrush = (ComparisonCar.Drivetrain == Drive.RWD) ? Brushes.Gray : Brushes.White;
                RearBrush = (ComparisonCar.Drivetrain == Drive.FWD) ? Brushes.Gray : Brushes.White;
                TransversalBrush = (ComparisonCar.Drivetrain == Drive.AWD) ? Brushes.White : Brushes.Gray;

                if (ComparisonCar.HasDriveSwap)
                {
                    Brush RMbrush = new SolidColorBrush(Color.FromRgb(150, 110, 110));
                    Brush ADbrush = new SolidColorBrush(Color.FromRgb(220, 255, 220));
                    Brush NObrush = Brushes.Gray;
                    Brush YSbrush = Brushes.White;

                    //Drive wheels added
                    if (ComparisonCar.Drivetrain == Drive.AWD)
                    {
                        FrontBrush = (ComparisonCar.Model.Drivetrain == Drive.FWD) ? YSbrush : ADbrush;
                        RearBrush = (ComparisonCar.Model.Drivetrain == Drive.RWD) ? YSbrush : ADbrush;
                        TransversalBrush = ADbrush;
                    }
                    //Drive wheel removed
                    else if (ComparisonCar.Model.Drivetrain == Drive.AWD)
                    {
                        FrontBrush = (ComparisonCar.Drivetrain == Drive.RWD) ? RMbrush : YSbrush;
                        RearBrush = (ComparisonCar.Drivetrain == Drive.FWD) ? RMbrush : YSbrush;
                        TransversalBrush = RMbrush;
                    }
                    //Drive wheel moved
                    else
                    {
                        FrontBrush = (ComparisonCar.Drivetrain == Drive.RWD) ? RMbrush : YSbrush;
                        RearBrush = (ComparisonCar.Drivetrain == Drive.FWD) ? RMbrush : YSbrush;
                        TransversalBrush = (ComparisonCar.Drivetrain == Drive.AWD) ? YSbrush : NObrush;
                    }
                }

                Box_WhFLCmp.Background = Box_WhFRCmp.Background = Box_AxFrCmp.Background = FrontBrush;
                Box_WhRLCmp.Background = Box_WhRRCmp.Background = Box_AxRrCmp.Background = RearBrush;
                Box_AxTrCmp.Background = TransversalBrush;

                Box_DrivCmp.Content = ComparisonCar.Drivetrain.ToString();
                Box_SetpCmp.Content = ComparisonCar.Setup.ToString();

                if (ComparisonCar.Drivetrain != SelectedCar.Drivetrain) Box_DrivCmp.Foreground = Box_Driv.Foreground = Brushes.Orange;
                else Box_DrivCmp.Foreground = Box_Driv.Foreground = Brushes.White;

                if (ComparisonCar.Setup != SelectedCar.Setup) Box_SetpCmp.Foreground = Box_Setp.Foreground = Brushes.Orange;
                else Box_SetpCmp.Foreground = Box_Setp.Foreground = Brushes.White;

                if (ComparisonCar.Stats.PI > SelectedCar.Stats.PI + 5) { Box_PInd.Foreground = Brushes.Red; Box_PIndCmp.Foreground = Brushes.Green; }
                else if (ComparisonCar.Stats.PI + 5 < SelectedCar.Stats.PI) { Box_PIndCmp.Foreground = Brushes.Red; Box_PInd.Foreground = Brushes.Green; }
                else { Box_PInd.Foreground = Box_PIndCmp.Foreground = Brushes.Black; }

                if (SelectedCar.Stats.Speed > ComparisonCar.Stats.Speed) { Box_CSpd.Foreground = Brushes.Lime; Box_SSpd.Foreground = Brushes.Red; }
                else if (SelectedCar.Stats.Speed < ComparisonCar.Stats.Speed) { Box_CSpd.Foreground = Brushes.Red; Box_SSpd.Foreground = Brushes.Lime; }
                else { Box_CSpd.Foreground = Brushes.White; Box_SSpd.Foreground = Brushes.White; }

                if (SelectedCar.Stats.Handling > ComparisonCar.Stats.Handling) { Box_CHnd.Foreground = Brushes.Lime; Box_SHnd.Foreground = Brushes.Red; }
                else if (SelectedCar.Stats.Handling < ComparisonCar.Stats.Handling) { Box_CHnd.Foreground = Brushes.Red; Box_SHnd.Foreground = Brushes.Lime; }
                else { Box_CHnd.Foreground = Brushes.White; Box_SHnd.Foreground = Brushes.White; }

                if (SelectedCar.Stats.Acceleration > ComparisonCar.Stats.Acceleration) { Box_CAcc.Foreground = Brushes.Lime; Box_SAcc.Foreground = Brushes.Red; }
                else if (SelectedCar.Stats.Acceleration < ComparisonCar.Stats.Acceleration) { Box_CAcc.Foreground = Brushes.Red; Box_SAcc.Foreground = Brushes.Lime; }
                else { Box_CAcc.Foreground = Brushes.White; Box_SAcc.Foreground = Brushes.White; }

                if (SelectedCar.Stats.Launch > ComparisonCar.Stats.Launch) { Box_CLau.Foreground = Brushes.Lime; Box_SLau.Foreground = Brushes.Red; }
                else if (SelectedCar.Stats.Launch < ComparisonCar.Stats.Launch) { Box_CLau.Foreground = Brushes.Red; Box_SLau.Foreground = Brushes.Lime; }
                else { Box_CLau.Foreground = Brushes.White; Box_SLau.Foreground = Brushes.White; }

                if (SelectedCar.Stats.Braking > ComparisonCar.Stats.Braking) { Box_CBra.Foreground = Brushes.Lime; Box_SBra.Foreground = Brushes.Red; }
                else if (SelectedCar.Stats.Braking < ComparisonCar.Stats.Braking) { Box_CBra.Foreground = Brushes.Red; Box_SBra.Foreground = Brushes.Lime; }
                else { Box_CBra.Foreground = Brushes.White; Box_SBra.Foreground = Brushes.White; }

                if (SelectedCar.Stats.Offroad > ComparisonCar.Stats.Offroad) { Box_COff.Foreground = Brushes.Lime; Box_SOff.Foreground = Brushes.Red; }
                else if (SelectedCar.Stats.Offroad < ComparisonCar.Stats.Offroad) { Box_COff.Foreground = Brushes.Red; Box_SOff.Foreground = Brushes.Lime; }
                else { Box_COff.Foreground = Brushes.White; Box_SOff.Foreground = Brushes.White; }
            }
            else
            {
                Box_SNam.Content = (SelectedCar.HasDriveSwap ? SelectedCar.Model.Drivetrain.ToString() + " - " : "") + "Stock";
                Box_SNam.Foreground = Box_SSpd.Foreground = Box_SHnd.Foreground = Box_SAcc.Foreground = Box_SLau.Foreground = Box_SBra.Foreground = Box_SOff.Foreground = Brushes.DarkGray;

                Box_Driv.Foreground = Brushes.White;
                Box_Setp.Foreground = Brushes.White;
                Box_PInd.Foreground = Brushes.Black;

                if (SelectedCar.Stats.Speed > SelectedCar.Model.Stats.Speed) Box_CSpd.Foreground = Brushes.Lime;
                else if (SelectedCar.Stats.Speed < SelectedCar.Model.Stats.Speed) Box_CSpd.Foreground = Brushes.Red;
                else Box_CSpd.Foreground = Brushes.White;

                if (SelectedCar.Stats.Handling > SelectedCar.Model.Stats.Handling) Box_CHnd.Foreground = Brushes.Lime;
                else if (SelectedCar.Stats.Handling < SelectedCar.Model.Stats.Handling) Box_CHnd.Foreground = Brushes.Red;
                else Box_CHnd.Foreground = Brushes.White;

                if (SelectedCar.Stats.Acceleration > SelectedCar.Model.Stats.Acceleration) Box_CAcc.Foreground = Brushes.Lime;
                else if (SelectedCar.Stats.Acceleration < SelectedCar.Model.Stats.Acceleration) Box_CAcc.Foreground = Brushes.Red;
                else Box_CAcc.Foreground = Brushes.White;

                if (SelectedCar.Stats.Launch > SelectedCar.Model.Stats.Launch) Box_CLau.Foreground = Brushes.Lime;
                else if (SelectedCar.Stats.Launch < SelectedCar.Model.Stats.Launch) Box_CLau.Foreground = Brushes.Red;
                else Box_CLau.Foreground = Brushes.White;

                if (SelectedCar.Stats.Braking > SelectedCar.Model.Stats.Braking) Box_CBra.Foreground = Brushes.Lime;
                else if (SelectedCar.Stats.Braking < SelectedCar.Model.Stats.Braking) Box_CSpd.Foreground = Brushes.Red;
                else Box_CBra.Foreground = Brushes.White;

                if (SelectedCar.Stats.Offroad > SelectedCar.Model.Stats.Offroad) Box_COff.Foreground = Brushes.Lime;
                else if (SelectedCar.Stats.Offroad < SelectedCar.Model.Stats.Offroad) Box_COff.Foreground = Brushes.Red;
                else Box_COff.Foreground = Brushes.White;

                Box_SSpd.Content = SelectedCar.Model.Stats.Speed.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SHnd.Content = SelectedCar.Model.Stats.Handling.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SAcc.Content = SelectedCar.Model.Stats.Acceleration.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SLau.Content = SelectedCar.Model.Stats.Launch.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SBra.Content = SelectedCar.Model.Stats.Braking.ToString("0.0", CultureInfo.InvariantCulture);
                Box_SOff.Content = SelectedCar.Model.Stats.Offroad.ToString("0.0", CultureInfo.InvariantCulture);
            }
            Box_CSpe.ToolTip = Box_CSpe.Content;
            Box_SNam.ToolTip = Box_SNam.Content;

            if (SelectedCar != null && SelectedCar.IsDriven && TopContainer.ActualWidth > Box_IsDriven.ActualWidth) ColCarName.MaxWidth = TopContainer.ActualWidth - (Box_IsDriven.ActualWidth + Box_IsDriven.Margin.Left + Box_IsDriven.Margin.Right);
            else ColCarName.MaxWidth = TopContainer.ActualWidth + Math.Max(0, -(Box_IsDriven.Margin.Left + Box_IsDriven.Margin.Right));

            FilterContainer.UpdateCount(Filter);
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
                    Box_Livr.Children.Add(LW.LiveryClassic(90));
                    break;
                case LiveryType.Racing:
                    Box_Livr.Children.Add(LW.LiveryRacing(90));
                    break;
                case LiveryType.Gradient:
                    Box_Livr.Children.Add(LW.LiveryGradient(90));
                    break;
            }
        }

        private void Garage_Click(object sender, RoutedEventArgs e)
        {
            var gl = new Window_GarageList();
            gl.SelectCar(SelectedCar);
            gl.ShowDialog();
            ComparisonCar = gl.ReturnValueComp;
            if (gl.ReturnValue == null) SetCar(SelectedCar);
            else SetCar(gl.ReturnValue);
        }

        private void Note_Click(object sender, RoutedEventArgs e)
        {
            if (WNTK == null || !WNTK.IsOpen)
            {
                WNTK = new Window_Notetaker();
                WNTK.Show();
            }
        }

        private void Companion_Click(object sender, RoutedEventArgs e)
        {
            if (SCC == null || !SCC.IsOpen)
            {
                SCC = new ShowcaseCompactCompanion(SelectedCar, UserControl_KeyUp);
                SCC.Show();
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            if (WCCV == null || !WCCV.IsOpen)
            {
                WCCV = new Window_ColorConverter();
                WCCV.Show();
            }
        }

        private void Random_Click(object sender, RoutedEventArgs e)
        {
            SetCar(Lists.RandomCar(Filter));
        }

        private void EditCar_Click(object sender, RoutedEventArgs e)
        {
            var ce = new Window_CarEdit(SelectedCar);
            ce.ShowDialog();
            SetCar(SelectedCar);
        }

        private void EditModel_Click(object sender, RoutedEventArgs e)
        {
            var scd = new Window_StockCarData();
            if (SelectedCar != null)
                scd.OpenModel(SelectedCar.Model);
            else
                scd.OpenModel(null);
            scd.ShowDialog();
            SetCar(SelectedCar);
        }

        private void Driven_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCar == null) return;
            SelectedCar.IsDriven = true;
            ImportData.Quicksave();
            SetCar(SelectedCar);
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Length == 1 && char.IsLetter(e.Key.ToString().First()))
            {
                ListContainer_KeyUp(sender, e);
                e.Handled = true;
            }
            else
            {
                switch (e.Key)
                {
                    case Key.F1: EditCar_Click(null, null); break;
                    case Key.F2: EditModel_Click(null, null); break;
                    case Key.F3: Note_Click(null, null); break;
                    case Key.F4: Color_Click(null, null); break;
                    case Key.F5: Random_Click(null, null); break;
                    case Key.F6: Driven_Click(null, null); break;
                    case Key.F7: Companion_Click(null, null); break;
                }
                e.Handled = true;
            }
        }

        private void ListContainer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListContainer.SelectedItem as Car == null) { MessageBox.Show("No selection"); return; }
            SetCar(ListContainer.SelectedItem as Car);
        }

        private void LCSelect_Click(object sender, RoutedEventArgs e)
        {
            if (ListContainer.SelectedItem as Car == null) { MessageBox.Show("No selection"); return; }
            SetCar(ListContainer.SelectedItem as Car);
        }

        private void LCEditCar_Click(object sender, RoutedEventArgs e)
        {
            if (ListContainer.SelectedItem as Car == null) { MessageBox.Show("No selection"); return; }
            var ce = new Window_CarEdit(ListContainer.SelectedItem as Car);
            ce.ShowDialog();
            SetCar(SelectedCar);
        }

        private void LCEditModel_Click(object sender, RoutedEventArgs e)
        {
            if (ListContainer.SelectedItem as Car == null) { MessageBox.Show("No selection"); return; }
            var scd = new Window_StockCarData();
            scd.OpenModel((ListContainer.SelectedItem as Car).Model);
            scd.ShowDialog();
            SetCar(SelectedCar);
        }

        private void LCCompare_Click(object sender, RoutedEventArgs e)
        {
            if (ListContainer.SelectedItem as Car == null) { MessageBox.Show("No selection"); return; }
            ComparisonCar = ListContainer.SelectedItem as Car;
            UpdateInfo();
        }

        private void LCUnCompare_Click(object sender, RoutedEventArgs e)
        {
            ComparisonCar = null;
            UpdateInfo();
        }

        private void LCDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListContainer.SelectedItem as Car == null) { MessageBox.Show("No selection"); return; }
            var mbr = MessageBox.Show("This action will delete this car. This action cannot be undone. Confirmation is required to execute.\n\nDo you wish to delete this " + (ListContainer.SelectedItem as Car).ToString() + "?", (ListContainer.SelectedItem as Car).ToString() + " deletion prompt", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (MessageBoxResult.Yes == mbr)
            {
                var car = ListContainer.SelectedItem as Car;
                if (SelectedCar == car) SetCar(Lists.RandomCar(Filter));
                Lists.RemoveCar(car);
                if (Filter == null)
                    ListContainer.ItemsSource = Lists.Garage();
                else
                    ListContainer.ItemsSource = Filter.Matches(Lists.Garage());
            }
        }

        private string Entry = "";
        private DateTime LastEntry = DateTime.Now.AddYears(-1);
        private void ListContainer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString().Length != 1) { Entry = ""; return; }

            List<Car> list;
            if (Filter == null) list = Lists.Garage();
            else list = Filter.Matches(Lists.Garage());
            var search = e.Key.ToString().First();

            if (DateTime.Now.Subtract(LastEntry).TotalMilliseconds > 500) Entry = search.ToString();
            else Entry += search;

            System.Diagnostics.Debug.WriteLine(Entry);

            var matches = list.Where(c => c.Model.Manufacturer.Name.ToUpper().StartsWith(Entry)).ToList();
            if (matches.Count() > 0)
            {
                if (ListContainer.SelectedItem == null)
                    SetCar(matches.First());
                else
                {
                    Car tmp = ListContainer.SelectedItem as Car;
                    if (Entry.Length == 1 && tmp.Model.Manufacturer.Name.ToUpper().StartsWith(Entry))
                    {
                        int index = matches.IndexOf(tmp);
                        index = (index + 1) % matches.Count();
                        SetCar(matches[index]);
                    }
                    else
                    {
                        SetCar(matches.First());
                    }
                }
            }
            LastEntry = DateTime.Now;
        }
    }
}
