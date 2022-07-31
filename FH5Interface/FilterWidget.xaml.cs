using Common;
using DataModel;
using FH5Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FH5Interface
{
    /// <summary>
    /// Logique d'interaction pour FilterWidget.xaml
    /// </summary>
    public partial class FilterWidget : UserControl
    {
        public delegate void FilterUpdateHandler(Filter filter);
        public event FilterUpdateHandler FilterUpdated;

        private readonly bool INIT = false;
        private bool SUPPRESS = false;
        public FilterWidget()
        {
            InitializeComponent();

            NameScope.SetNameScope(ContainerClass, new NameScope());
            NameScope.SetNameScope(ContainerRarity, new NameScope());
            NameScope.SetNameScope(ContainerManf, new NameScope());
            NameScope.SetNameScope(ContainerType, new NameScope());
            NameScope.SetNameScope(ContainerFamily, new NameScope());
            NameScope.SetNameScope(ContainerCountry, new NameScope());
            NameScope.SetNameScope(ContainerColors, new NameScope());

            Chk_ByPI.Checked += UpdateFilterToggle;
            Chk_ByDrive.Checked += UpdateFilterToggle;
            Chk_BySetup.Checked += UpdateFilterToggle;
            Chk_ByClass.Checked += UpdateFilterToggle;
            Chk_ByYear.Checked += UpdateFilterToggle;
            Chk_ByRarity.Checked += UpdateFilterToggle;
            Chk_ByManf.Checked += UpdateFilterToggle;
            Chk_ByType.Checked += UpdateFilterToggle;
            Chk_ByFamily.Checked += UpdateFilterToggle;
            Chk_ByCountry.Checked += UpdateFilterToggle;
            Chk_ByHasLivery.Checked += UpdateFilterToggle;
            Chk_ByHasSpecs.Checked += UpdateFilterToggle;
            Chk_ByIsDriven.Checked += UpdateFilterToggle;
            Chk_ByPIComp.Checked += UpdateFilterToggle;
            Chk_ByColor.Checked += UpdateFilterToggle;
            Chk_BySwap.Checked += UpdateFilterToggle;

            Chk_ByPI.Unchecked += UpdateFilterToggle;
            Chk_ByDrive.Unchecked += UpdateFilterToggle;
            Chk_BySetup.Unchecked += UpdateFilterToggle;
            Chk_ByClass.Unchecked += UpdateFilterToggle;
            Chk_ByYear.Unchecked += UpdateFilterToggle;
            Chk_ByRarity.Unchecked += UpdateFilterToggle;
            Chk_ByManf.Unchecked += UpdateFilterToggle;
            Chk_ByType.Unchecked += UpdateFilterToggle;
            Chk_ByFamily.Unchecked += UpdateFilterToggle;
            Chk_ByCountry.Unchecked += UpdateFilterToggle;
            Chk_ByHasLivery.Unchecked += UpdateFilterToggle;
            Chk_ByHasSpecs.Unchecked += UpdateFilterToggle;
            Chk_ByIsDriven.Unchecked += UpdateFilterToggle;
            Chk_ByPIComp.Unchecked += UpdateFilterToggle;
            Chk_ByColor.Unchecked += UpdateFilterToggle;
            Chk_BySwap.Unchecked += UpdateFilterToggle;

            Box_PIMax.Min = Box_PIMin.Min = 0L;
            Box_PIMax.Max = Box_PIMin.Max = 999L;
            Box_PIMax.Value = 999L;
            Box_PIMin.Value = 100L;

            Box_YearMax.Min = Box_YearMin.Min = 1800L;
            Box_YearMax.Max = Box_YearMin.Max = 3000L;
            try
            {
                Box_YearMax.Value = (long)Lists.Models().Select(x => x.Year).Max();
                Box_YearMin.Value = (long)Lists.Models().Select(x => x.Year).Min();
            }
            catch
            {
                Box_YearMax.Value = 3000L;
                Box_YearMin.Value = 1800L;
            }

            Box_FilX.Content = Box_FilY.Content = Lists.Garage().Count();

            FillClass();
            FillRarity();
            FillManf();
            FillType();
            FillFamily();
            FillCountry();
            FillColor();

            INIT = true;
        }

        private string Namify(string str)
        {
            return str.Replace(" ", "").Replace("-", "").Replace("&", "").Replace("'", "");
        }

        #region Class
        private void FillClass()
        {
            for (var i = 0; i < ContainerClass.Children.Count;)
            {
                UIElement element = ContainerClass.Children[i];
                if (element is Control)
                {
                    Control child = element as Control;
                    if (child.Tag is string && ((string)(child.Tag)).ToLower() == "true")
                        i++;
                    else
                    {
                        ContainerClass.UnregisterName(child.Name);
                        ContainerClass.Children.Remove(child);
                    }
                }
            }

            foreach (CarClass cl in (CarClass[])Enum.GetValues(typeof(CarClass)))
            {
                CheckBox chk = new CheckBox();
                chk.VerticalContentAlignment = chk.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                chk.Content = cl.ToString();
                chk.Margin = new Thickness(0, 0, 10, 0);
                chk.Name = "Chk_Class_" + cl.ToString();
                ContainerClass.RegisterName(chk.Name, chk);
                chk.IsChecked = true;
                chk.Checked += UpdateFilterEvent;
                chk.Unchecked += UpdateFilterEvent;
                chk.Unchecked += Chk_Class_Unchecked;
                chk.IsEnabled = Chk_ByClass.IsChecked == true;
                chk.Visibility = Visibility.Collapsed;
                chk.Tag = (int?)cl;
                chk.Foreground = cl.Color();
                ContainerClass.Children.Add(chk);
            }
        }

        private void Chk_Class_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS) return;
            SUPPRESS = true;
            Chk_ByClass_All.IsChecked = false;
            SUPPRESS = false;
        }

        private void Chk_ByClass_All_Checked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS || !INIT) return;
            SUPPRESS = true;
            foreach (CarClass cl in (CarClass[])Enum.GetValues(typeof(CarClass)))
            {
                CheckBox chk = (ContainerClass.FindName("Chk_Class_" + cl.ToString()) as CheckBox);
                chk.IsChecked = Chk_ByClass_All.IsChecked == true;
            }
            SUPPRESS = false;
            UpdateFilter();
        }
        #endregion

        #region Rarity
        private void FillRarity()
        {
            for (var i = 0; i < ContainerRarity.Children.Count;)
            {
                UIElement element = ContainerRarity.Children[i];
                if (element is Control)
                {
                    Control child = element as Control;
                    if (child.Tag is string && ((string)(child.Tag)).ToLower() == "true")
                        i++;
                    else
                    {
                        ContainerRarity.UnregisterName(child.Name);
                        ContainerRarity.Children.Remove(child);
                    }
                }
            }

            foreach (Rarity ra in (Rarity[])Enum.GetValues(typeof(Rarity)))
            {
                CheckBox chk = new CheckBox();
                chk.VerticalContentAlignment = chk.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                chk.Content = ra.GetName();
                chk.Margin = new Thickness(0, 0, 10, 0);
                chk.Name = "Chk_Rarity_" + ra.ToString();
                ContainerRarity.RegisterName(chk.Name, chk);
                chk.IsChecked = true;
                chk.Checked += UpdateFilterEvent;
                chk.Unchecked += UpdateFilterEvent;
                chk.IsEnabled = Chk_ByRarity.IsChecked == true;
                chk.Tag = (int?)ra;
                chk.Visibility = Visibility.Collapsed;
                chk.Foreground = ra.Color();
                ContainerRarity.Children.Add(chk);
            }
        }
        #endregion

        #region Manf
        private void FillManf()
        {
            for (var i = 0; i < ContainerManf.Children.Count;)
            {
                UIElement element = ContainerManf.Children[i];
                if (element is Control)
                {
                    Control child = element as Control;
                    if (child.Tag is string && ((string)(child.Tag)).ToLower() == "true")
                        i++;
                    else
                    {
                        ContainerManf.UnregisterName(child.Name);
                        ContainerManf.Children.Remove(child);
                    }
                }
            }

            foreach (Manufacturer ma in Lists.Manufacturers())
            {
                CheckBox chk = new CheckBox();
                chk.VerticalContentAlignment = chk.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                chk.Content = ma.Name;
                chk.Name = "Chk_Manf_" + Namify(ma.Name);
                ContainerManf.RegisterName(chk.Name, chk);
                chk.IsChecked = true;
                chk.Checked += UpdateFilterEvent;
                chk.Unchecked += UpdateFilterEvent;
                chk.Unchecked += Chk_Manf_Unchecked;
                chk.IsEnabled = Chk_ByManf.IsChecked == true;
                chk.Visibility = Chk_ByManf.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                chk.Tag = ma;
                chk.Width = 150;
                ContainerManf.Children.Add(chk);
            }
        }

        private void Chk_Manf_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS) return;
            SUPPRESS = true;
            Chk_ByManf_All.IsChecked = false;
            SUPPRESS = false;
        }

        private void Chk_ByManf_All_Checked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS || !INIT) return;
            SUPPRESS = true;
            foreach (Manufacturer ma in Lists.Manufacturers())
            {
                CheckBox chk = (ContainerManf.FindName("Chk_Manf_" + Namify(ma.Name)) as CheckBox);
                chk.IsChecked = Chk_ByManf_All.IsChecked == true;
            }
            SUPPRESS = false;
            UpdateFilter();
        }
        #endregion

        #region Type
        private void FillType()
        {
            for (var i = 0; i < ContainerType.Children.Count;)
            {
                UIElement element = ContainerType.Children[i];
                if (element is Control)
                {
                    Control child = element as Control;
                    if (child.Tag is string && ((string)(child.Tag)).ToLower() == "true")
                        i++;
                    else
                    {
                        ContainerType.UnregisterName(child.Name);
                        ContainerType.Children.Remove(child);
                    }
                }
            }

            foreach (CarType ty in Lists.Types())
            {
                CheckBox chk = new CheckBox();
                chk.VerticalContentAlignment = chk.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                chk.Content = ty.Name;
                chk.Name = "Chk_Type_" + Namify(ty.Name);
                ContainerType.RegisterName(chk.Name, chk);
                chk.IsChecked = true;
                chk.Checked += UpdateFilterEvent;
                chk.Unchecked += UpdateFilterEvent;
                chk.Unchecked += Chk_Type_Unchecked;
                chk.IsEnabled = Chk_ByType.IsChecked == true;
                chk.Visibility = Chk_ByType.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                chk.Tag = ty;
                chk.Width = 150;
                ContainerType.Children.Add(chk);
            }
        }

        private void Chk_Type_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS) return;
            SUPPRESS = true;
            Chk_ByType_All.IsChecked = false;
            SUPPRESS = false;
        }

        private void Chk_ByType_All_Checked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS || !INIT) return;
            SUPPRESS = true;
            foreach (CarType ty in Lists.Types())
            {
                CheckBox chk = (ContainerType.FindName("Chk_Type_" + Namify(ty.Name)) as CheckBox);
                chk.IsChecked = Chk_ByType_All.IsChecked == true;
            }
            SUPPRESS = false;
            UpdateFilter();
        }
        #endregion

        #region Family
        private void FillFamily()
        {
            for (var i = 0; i < ContainerFamily.Children.Count;)
            {
                UIElement element = ContainerFamily.Children[i];
                if (element is Control)
                {
                    Control child = element as Control;
                    if (child.Tag is string && ((string)(child.Tag)).ToLower() == "true")
                        i++;
                    else
                    {
                        ContainerFamily.UnregisterName(child.Name);
                        ContainerFamily.Children.Remove(child);
                    }
                }
            }

            foreach (string fa in Lists.Families(true))
            {
                CheckBox chk = new CheckBox();
                chk.VerticalContentAlignment = chk.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                chk.Content = fa;
                chk.Name = "Chk_Family_" + Namify(fa);
                ContainerFamily.RegisterName(chk.Name, chk);
                chk.IsChecked = true;
                chk.Checked += UpdateFilterEvent;
                chk.Unchecked += UpdateFilterEvent;
                chk.Unchecked += Chk_Family_Unchecked;
                chk.IsEnabled = Chk_ByFamily.IsChecked == true;
                chk.Visibility = Chk_ByFamily.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                chk.Tag = fa;
                chk.Width = 150;
                ContainerFamily.Children.Add(chk);
            }
        }

        private void Chk_Family_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS) return;
            SUPPRESS = true;
            Chk_ByFamily_All.IsChecked = false;
            SUPPRESS = false;
        }

        private void Chk_ByFamily_All_Checked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS || !INIT) return;
            SUPPRESS = true;
            foreach (string fa in Lists.Families(true))
            {
                CheckBox chk = (ContainerFamily.FindName("Chk_Family_" + Namify(fa)) as CheckBox);
                chk.IsChecked = Chk_ByFamily_All.IsChecked == true;
            }
            SUPPRESS = false;
            UpdateFilter();
        }
        #endregion

        #region Country
        private void FillCountry()
        {
            for (var i = 0; i < ContainerCountry.Children.Count;)
            {
                UIElement element = ContainerCountry.Children[i];
                if (element is Control)
                {
                    Control child = element as Control;
                    if (child.Tag is string && ((string)(child.Tag)).ToLower() == "true")
                        i++;
                    else
                    {
                        ContainerCountry.UnregisterName(child.Name);
                        ContainerCountry.Children.Remove(child);
                    }
                }
            }

            foreach (string cc in Data.CountryCodeList)
            {
                CheckBox chk = new CheckBox();
                chk.VerticalContentAlignment = chk.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                chk.Content = cc.ToUpper();
                chk.Name = "Chk_Country_" + cc;
                ContainerCountry.RegisterName(chk.Name, chk);
                chk.IsChecked = true;
                chk.Checked += UpdateFilterEvent;
                chk.Unchecked += UpdateFilterEvent;
                chk.Unchecked += Chk_Country_Unchecked;
                chk.IsEnabled = Chk_ByCountry.IsChecked == true;
                chk.Visibility = Chk_ByCountry.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                chk.Tag = cc;
                chk.Width = 50;
                ContainerCountry.Children.Add(chk);
            }
        }

        private void Chk_Country_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS) return;
            SUPPRESS = true;
            Chk_ByCountry_All.IsChecked = false;
            SUPPRESS = false;
        }

        private void Chk_ByCountry_All_Checked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS || !INIT) return;
            SUPPRESS = true;
            foreach (string cc in Data.CountryCodeList)
            {
                CheckBox chk = (ContainerCountry.FindName("Chk_Country_" + cc) as CheckBox);
                chk.IsChecked = Chk_ByCountry_All.IsChecked == true;
            }
            SUPPRESS = false;
            UpdateFilter();
        }
        #endregion

        #region Color
        private void FillColor()
        {
            for (var i = 0; i < ContainerColors.Children.Count;)
            {
                UIElement element = ContainerColors.Children[i];
                if (element is Control)
                {
                    Control child = element as Control;
                    if (child.Tag is string && ((string)(child.Tag)).ToLower() == "true")
                        i++;
                    else
                    {
                        ContainerColors.UnregisterName(child.Name);
                        ContainerColors.Children.Remove(child);
                    }
                }
            }

            foreach (CarColor co in CarColor.List)
            {
                CheckBox chk = new CheckBox();
                chk.VerticalContentAlignment = chk.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                chk.Content = co.Name;
                chk.Name = "Chk_Color_" + Namify(co.Name);
                ContainerColors.RegisterName(chk.Name, chk);
                chk.IsChecked = true;
                chk.Checked += UpdateFilterEvent;
                chk.Unchecked += UpdateFilterEvent;
                chk.Unchecked += Chk_Color_Unchecked;
                chk.IsEnabled = Chk_ByColor.IsChecked == true;
                chk.Visibility = Chk_ByColor.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                chk.Tag = co;
                chk.Width = 150;
                chk.Foreground = co.Brush;
                ContainerColors.Children.Add(chk);
            }
        }

        private void Chk_Color_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS) return;
            SUPPRESS = true;
            Chk_ByColor_All.IsChecked = false;
            SUPPRESS = false;
        }

        private void Chk_ByColor_All_Checked(object sender, RoutedEventArgs e)
        {
            if (SUPPRESS || !INIT) return;
            SUPPRESS = true;
            foreach (CarColor co in CarColor.List)
            {
                CheckBox chk = (ContainerColors.FindName("Chk_Color_" + Namify(co.Name)) as CheckBox);
                chk.IsChecked = Chk_ByColor_All.IsChecked == true;
            }
            SUPPRESS = false;
            UpdateFilter();
        }
        #endregion

        private void UpdateFilter()
        {
            //return;
            if (!INIT || SUPPRESS) return;
            Filter Filter = new Filter();

            Filter.ByPI = Chk_ByPI.IsChecked == true;
            if (Filter.ByPI)
            {
                Filter.PI_Min = (int)Math.Min(Box_PIMin.Value, Box_PIMax.Value);
                Filter.PI_Max = (int)Math.Max(Box_PIMin.Value, Box_PIMax.Value);
            }

            Filter.ByPICompetitive = Chk_ByPIComp.IsChecked == true;
            if (Filter.ByPICompetitive)
            {
                Filter.IsCompetitive = Chk_PICompY.IsChecked == true;
            }

            Filter.ByDrive = Chk_ByDrive.IsChecked == true;
            if (Filter.ByDrive)
            {
                List<Drive> tmpDrv = new List<Drive>();
                if (Chk_DriveR.IsChecked == true) tmpDrv.Add(Drive.RWD);
                if (Chk_DriveA.IsChecked == true) tmpDrv.Add(Drive.AWD);
                if (Chk_DriveF.IsChecked == true) tmpDrv.Add(Drive.FWD);
                Filter.Drive = tmpDrv.ToArray();
            }

            Filter.ByClass = Chk_ByClass.IsChecked == true;
            if (Filter.ByClass)
            {
                List<CarClass> tmpCls = new List<CarClass>();
                foreach (UIElement element in ContainerClass.Children)
                {
                    if (element is CheckBox)
                    {
                        CheckBox child = element as CheckBox;
                        if (child.Tag is int && child.IsChecked == true)
                            tmpCls.Add((CarClass)(child.Tag as int?));
                    }
                }
                Filter.Class = tmpCls.ToArray();
            }

            Filter.BySetup = Chk_BySetup.IsChecked == true;
            if (Filter.BySetup)
            {
                List<Setup> tmpStp = new List<Setup>();
                if (Chk_SetupR.IsChecked == true) tmpStp.Add(Setup.Road);
                if (Chk_SetupO.IsChecked == true) tmpStp.Add(Setup.Offroad);
                if (Chk_SetupF.IsChecked == true) tmpStp.Add(Setup.Drift);
                if (Chk_SetupG.IsChecked == true) tmpStp.Add(Setup.Drag);
                Filter.Setup = tmpStp.ToArray();
            }

            Filter.ByYear = Chk_ByYear.IsChecked == true;
            if (Filter.ByYear)
            {
                Filter.Year_Min = (int)Math.Min(Box_YearMin.Value, Box_YearMax.Value);
                Filter.Year_Max = (int)Math.Max(Box_YearMin.Value, Box_YearMax.Value);
            }

            Filter.ByRarity = Chk_ByRarity.IsChecked == true;
            if (Filter.ByRarity)
            {
                List<Rarity> tmpRar = new List<Rarity>();
                foreach (UIElement element in ContainerRarity.Children)
                {
                    if (element is CheckBox)
                    {
                        CheckBox child = element as CheckBox;
                        if (child.Tag is int && child.IsChecked == true)
                            tmpRar.Add((Rarity)(child.Tag as int?));
                    }
                }
                Filter.Rarity = tmpRar.ToArray();
            }

            Filter.ByManf = Chk_ByManf.IsChecked == true;
            if (Filter.ByManf)
            {
                List<Manufacturer> tmpMnf = new List<Manufacturer>();
                foreach (UIElement element in ContainerManf.Children)
                {
                    if (element is CheckBox)
                    {
                        CheckBox child = element as CheckBox;
                        if (child.Tag is Manufacturer && child.IsChecked == true)
                            tmpMnf.Add(child.Tag as Manufacturer);
                    }
                }
                Filter.Manfs = tmpMnf.ToArray();
            }

            Filter.ByType = Chk_ByType.IsChecked == true;
            if (Filter.ByType)
            {
                List<CarType> tmpTyp = new List<CarType>();
                foreach (UIElement element in ContainerType.Children)
                {
                    if (element is CheckBox)
                    {
                        CheckBox child = element as CheckBox;
                        if (child.Tag is CarType && child.IsChecked == true)
                            tmpTyp.Add(child.Tag as CarType);
                    }
                }
                Filter.Type = tmpTyp.ToArray();
            }

            Filter.ByFamily = Chk_ByFamily.IsChecked == true;
            if (Filter.ByFamily)
            {
                List<string> tmpFam = new List<string>();
                foreach (UIElement element in ContainerFamily.Children)
                {
                    if (element is CheckBox)
                    {
                        CheckBox child = element as CheckBox;
                        if (child.Tag is string && child.IsChecked == true)
                            tmpFam.Add(child.Tag as string);
                    }
                }
                Filter.Family = tmpFam.ToArray();
            }

            Filter.ByCountry = Chk_ByCountry.IsChecked == true;
            if (Filter.ByCountry)
            {
                List<string> tmpCcd = new List<string>();
                foreach (UIElement element in ContainerCountry.Children)
                {
                    if (element is CheckBox)
                    {
                        CheckBox child = element as CheckBox;
                        if (child.Tag is string && child.IsChecked == true)
                            tmpCcd.Add(child.Tag as string);
                    }
                }
                Filter.CountryCodes = tmpCcd.ToArray();
            }

            Filter.ByCustomLivery = Chk_ByHasLivery.IsChecked == true;
            if (Filter.ByCustomLivery)
            {
                Filter.HasCustomLivery = Chk_LiveryY.IsChecked == true;
            }

            Filter.ByCustomSpec = Chk_ByHasSpecs.IsChecked == true;
            if (Filter.ByCustomSpec)
            {
                Filter.HasCustomSpec = Chk_SpecsY.IsChecked == true;
            }

            Filter.ByDriven = Chk_ByIsDriven.IsChecked == true;
            if (Filter.ByDriven)
            {
                Filter.HasBeenDriven = Chk_DrivenY.IsChecked == true;
            }

            Filter.BySearchText = !string.IsNullOrEmpty(SearchBar.Text);
            if (Filter.BySearchText)
            {
                Filter.BySearchText_Model = Box_TextBy_Modl.IsChecked == true;
                Filter.BySearchText_Manf = Box_TextBy_Manf.IsChecked == true;
                Filter.BySearchText_Type = Box_TextBy_Type.IsChecked == true;
                Filter.BySearchText_Livery = Box_TextBy_Livr.IsChecked == true;
                Filter.BySearchText_Specs = Box_TextBy_Spec.IsChecked == true;
                Filter.BySearchText_Family = Box_TextBy_Faml.IsChecked == true;
                Filter.BySearchText_Year = Box_TextBy_Year.IsChecked == true;

                Filter.SearchText = SearchBar.Text;
            }

            Filter.ByColor = Chk_ByColor.IsChecked == true;
            if (Filter.ByColor)
            {
                List<string> tmpCol = new List<string>();
                foreach (UIElement element in ContainerColors.Children)
                {
                    if (element is CheckBox)
                    {
                        CheckBox child = element as CheckBox;
                        if (child.Tag is CarColor && child.IsChecked == true)
                            tmpCol.Add((child.Tag as CarColor).Name);
                    }
                }
                Filter.Colors = tmpCol.ToArray();

                Filter.ByColorAny = Chk_ColorA.IsChecked == true;
            }

            Filter.ByEngineSwap = Chk_BySwap.IsChecked == true;
            if (Filter.ByEngineSwap)
            {
                Filter.ByEngineSwap_IsStock = Chk_SwapN.IsChecked == true;
            }

            int x, y;
            Filter.Matches(Lists.Garage(), out x, out y);
            Box_FilX.Content = x;
            Box_FilY.Content = y;


            if (FilterUpdated != null) FilterUpdated(Filter);
        }

        public void UpdateCount(Filter filter)
        {
            int x, y;
            if (filter == null) x = y = Lists.Garage().Count;
            else filter.Matches(Lists.Garage(), out x, out y);
            Box_FilX.Content = x;
            Box_FilY.Content = y;
        }

        private void UpdateFilterEvent(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void UpdateFilterToggle(object sender, RoutedEventArgs e)
        {
            if (sender == Chk_ByPI)
            {
                Box_PIMin.IsEnabled = Box_PIMax.IsEnabled = Chk_ByPI.IsChecked == true;
                PIDash.Visibility = Box_PIMin.Visibility = Box_PIMax.Visibility = Chk_ByPI.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (sender == Chk_ByPIComp)
            {
                Chk_PICompY.IsEnabled = Chk_PICompN.IsEnabled = Chk_ByPIComp.IsChecked == true;
                Chk_PICompY.Visibility = Chk_PICompN.Visibility = Chk_ByPIComp.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (sender == Chk_ByDrive)
            {
                Chk_DriveR.IsEnabled = Chk_DriveA.IsEnabled = Chk_DriveF.IsEnabled = Chk_ByDrive.IsChecked == true;
                Chk_DriveR.Visibility = Chk_DriveA.Visibility = Chk_DriveF.Visibility = Chk_ByDrive.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (sender == Chk_BySetup)
            {
                Chk_SetupR.IsEnabled = Chk_SetupO.IsEnabled = Chk_SetupF.IsEnabled = Chk_SetupG.IsEnabled = Chk_BySetup.IsChecked == true;
                Chk_SetupR.Visibility = Chk_SetupO.Visibility = Chk_SetupF.Visibility = Chk_SetupG.Visibility = Chk_BySetup.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (sender == Chk_ByClass)
            {
                Chk_ByClass_All.IsEnabled = Chk_ByClass.IsChecked == true;
                Chk_ByClass_All.Visibility = Chk_ByClass.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                foreach (CarClass cl in (CarClass[])Enum.GetValues(typeof(CarClass)))
                {
                    CheckBox chk = (ContainerClass.FindName("Chk_Class_" + cl.ToString()) as CheckBox);
                    chk.IsEnabled = Chk_ByClass.IsChecked == true;
                    chk.Visibility = Chk_ByClass.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else if (sender == Chk_ByYear)
            {
                Box_YearMin.IsEnabled = Box_YearMax.IsEnabled = Chk_ByYear.IsChecked == true;
                YearDash.Visibility = Box_YearMin.Visibility = Box_YearMax.Visibility = Chk_ByYear.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (sender == Chk_ByRarity)
            {
                foreach (Rarity ra in (Rarity[])Enum.GetValues(typeof(Rarity)))
                {
                    CheckBox chk = (ContainerRarity.FindName("Chk_Rarity_" + ra.ToString()) as CheckBox);
                    chk.IsEnabled = Chk_ByRarity.IsChecked == true;
                    chk.Visibility = Chk_ByRarity.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else if (sender == Chk_ByManf)
            {
                Chk_ByManf_All.IsEnabled = Chk_ByManf.IsChecked == true;
                Chk_ByManf_All.Visibility = ContainerManf.Visibility = Chk_ByManf.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                foreach (Manufacturer ma in Lists.Manufacturers())
                {
                    CheckBox chk = (ContainerManf.FindName("Chk_Manf_" + Namify(ma.Name)) as CheckBox);
                    chk.IsEnabled = Chk_ByManf.IsChecked == true;
                    chk.Visibility = Chk_ByManf.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else if (sender == Chk_ByType)
            {
                Chk_ByType_All.IsEnabled = Chk_ByType.IsChecked == true;
                Chk_ByType_All.Visibility = ContainerType.Visibility = Chk_ByType.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                foreach (CarType ty in Lists.Types())
                {
                    CheckBox chk = (ContainerType.FindName("Chk_Type_" + Namify(ty.Name)) as CheckBox);
                    chk.IsEnabled = Chk_ByType.IsChecked == true;
                    chk.Visibility = Chk_ByType.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else if (sender == Chk_ByFamily)
            {
                Chk_ByFamily_All.IsEnabled = Chk_ByFamily.IsChecked == true;
                Chk_ByFamily_All.Visibility = ContainerFamily.Visibility = Chk_ByFamily.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                foreach (string fa in Lists.Families(true))
                {
                    CheckBox chk = (ContainerFamily.FindName("Chk_Family_" + Namify(fa)) as CheckBox);
                    chk.IsEnabled = Chk_ByFamily.IsChecked == true;
                    chk.Visibility = Chk_ByFamily.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else if (sender == Chk_ByCountry)
            {
                Chk_ByCountry_All.IsEnabled = Chk_ByCountry.IsChecked == true;
                Chk_ByCountry_All.Visibility = ContainerCountry.Visibility = Chk_ByCountry.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                foreach (string cc in Data.CountryCodeList)
                {
                    CheckBox chk = (ContainerCountry.FindName("Chk_Country_" + cc) as CheckBox);
                    chk.IsEnabled = Chk_ByCountry.IsChecked == true;
                    chk.Visibility = Chk_ByCountry.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else if (sender == Chk_ByIsDriven)
            {
                Chk_DrivenY.IsEnabled = Chk_DrivenN.IsEnabled = Chk_ByIsDriven.IsChecked == true;
                Chk_DrivenY.Visibility = Chk_DrivenN.Visibility = Chk_ByIsDriven.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (sender == Chk_ByHasLivery)
            {
                Chk_LiveryY.IsEnabled = Chk_LiveryN.IsEnabled = Chk_ByHasLivery.IsChecked == true;
                Chk_LiveryN.Visibility = Chk_LiveryY.Visibility = Chk_ByHasLivery.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (sender == Chk_ByHasSpecs)
            {
                Chk_SpecsY.IsEnabled = Chk_SpecsN.IsEnabled = Chk_ByHasSpecs.IsChecked == true;
                Chk_SpecsN.Visibility = Chk_SpecsY.Visibility = Chk_ByHasSpecs.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (sender == Chk_ByColor)
            {
                Chk_ByColor_All.IsEnabled = Chk_ColorP.IsEnabled = Chk_ColorA.IsEnabled = Chk_ByColor.IsChecked == true;
                Chk_ByColor_All.Visibility = Chk_ColorP.Visibility = Chk_ColorA.Visibility = ContainerColors.Visibility = Chk_ByColor.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                foreach (CarColor co in CarColor.List)
                {
                    CheckBox chk = (ContainerColors.FindName("Chk_Color_" + Namify(co.Name)) as CheckBox);
                    chk.IsEnabled = Chk_ByColor.IsChecked == true;
                    chk.Visibility = Chk_ByColor.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else if (sender == Chk_BySwap)
            {
                Chk_SwapY.IsEnabled = Chk_SwapN.IsEnabled = Chk_BySwap.IsChecked == true;
                Chk_SwapN.Visibility = Chk_SwapY.Visibility = Chk_BySwap.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void UpdateFilterEvent(object sender, RoutedEventArgs e)
        {
            UpdateFilter();
        }

        private void ExecuteSearchBar()
        {
            UpdateFilter();
        }

        private void SearchBar_KeyUp(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) ExecuteSearchBar(); e.Handled = true; }
    }
}
