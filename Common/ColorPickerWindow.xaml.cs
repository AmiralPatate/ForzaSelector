using DataModel;
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
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Logique d'interaction pour ColorPickerWindow.xaml
    /// </summary>
    public partial class ColorPickerWindow : Window
    {
        private ColorPickerWindow()
        {
            InitializeComponent();
            Picker.ColorPicked += Picker_ColorPicked;
        }

        public static CarColor PopUp()
        {
            ColorPickerWindow CPW = new ColorPickerWindow();
            CPW.ShowDialog();
            return CPW.Picker.SelectedColor;
        }

        private void Picker_ColorPicked()
        {
            Close();
        }
    }
}
