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

namespace Common
{
    /// <summary>
    /// Logique d'interaction pour Window_ColorConverter.xaml
    /// </summary>
    public partial class Window_ColorConverter : Window
    {
        public bool IsOpen { get; private set; }

        public Window_ColorConverter()
        {
            InitializeComponent();
            ForzaColor.Load();
            ListColors();

            Topmost = true;
            BtnPin.ToolTip = "Pinned on foreground\nClick to unpin";
            IsOpen = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ForzaColor.Save();
            IsOpen = false;
        }

        private ForzaColor CurrentColor;
        private bool DELMODE = false;

        private void Box_Hexa_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = Box_Hexa.Text.Replace("#", "");
            byte r = 0, g = 0, b = 0;
            bool success = text.Length == 6;
            if (success)
            {
                success &= byte.TryParse(text.Substring(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out r);
                success &= byte.TryParse(text.Substring(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out g);
                success &= byte.TryParse(text.Substring(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out b);
            }

            if (!success)
            {
                Box_ResultH.Content = "Error";
                Box_ResultS.Content = "Invalid";
                Box_ResultV.Content = "Input";
                ColorPreview.Background = Brushes.Transparent;
                CurrentColor = null;
            }
            else
            {
                ForzaColor fc = new ForzaColor(r, g, b);
                LoadColor(fc, false);
            }
        }

        private string Format(double d)
        {
            if (double.IsNaN(d)) return "Any";
            string str = d.ToString("F3");
            str = str.Substring(0, str.Length - 1) + "(" + str.Last() + ")";
            return str;
        }

        private void Box_RGB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] text = Box_RGB.Text.Split(',');
            bool success = text.Length == 3 && text[2].Length > 0;
            byte r = 0, g = 0, b = 0;
            if (success)
            {
                success &= byte.TryParse(text[0], out r);
                success &= byte.TryParse(text[1], out g);
                success &= byte.TryParse(text[2], out b);
            }

            if (!success)
            {
                Box_ResultH.Content = "Error";
                Box_ResultS.Content = "Invalid";
                Box_ResultV.Content = "Input";
                ColorPreview.Background = Brushes.Transparent;
                CurrentColor = null;
            }
            else
            {
                ForzaColor fc = new ForzaColor(r, g, b);
                LoadColor(fc, false);
            }
        }

        private void LoadColor(ForzaColor fc, bool loadName)
        {
            Box_ResultH.Content = Format(fc.Hue);
            Box_ResultS.Content = Format(fc.Saturation);
            Box_ResultV.Content = Format(fc.Value);
            if (loadName) BoxName.Text = fc.Name;
            ColorPreview.Background = fc.GetBrush();
            CurrentColor = fc;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            DELMODE = !DELMODE;
            Box_Hexa.IsEnabled = Box_RGB.IsEnabled = BoxName.IsEnabled = BtnSave.IsEnabled = !DELMODE;
            BtnDel.Background = DELMODE ? Brushes.LightPink : SystemColors.ControlBrush;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentColor == null) return;

            CurrentColor.Name = BoxName.Text;
            ForzaColor.Add(CurrentColor);

            ListColors();
        }

        private void ListColors()
        {
            ColorListContainer.Children.Clear();
            foreach (var color in ForzaColor.GetAll())
            {
                Brush BRUSH = color.GetBrush();

                Button BUTT = new Button();
                BUTT.Height = 30;
                BUTT.Width = 50;
                BUTT.Margin = new Thickness(1);
                BUTT.Background = BRUSH;
                BUTT.Tag = color;
                BUTT.ToolTip = color.Name;

                TextBlock TXT = new TextBlock();
                TXT.Padding = new Thickness(1, 0, 1, 0);
                TXT.Text = color.Name;

                var L = color.RelativeLuminance;
                if ((L + 0.05) / (0.0 + 0.05) > (1.0 + 0.05) / (L + 0.05)) TXT.Foreground = Brushes.Black;
                else TXT.Foreground = Brushes.White;

                TXT.FontSize = 10;

                BUTT.VerticalContentAlignment = VerticalAlignment.Bottom;
                BUTT.HorizontalContentAlignment = HorizontalAlignment.Center;
                BUTT.Content = TXT;

                BUTT.Click += BUTT_Click;

                ColorListContainer.Children.Add(BUTT);
            }
        }

        void BUTT_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var color = button.Tag as ForzaColor;
            if (DELMODE)
            {
                ForzaColor.Remove(color);
                ListColors();
            }
            else
                LoadColor(color, true);
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
