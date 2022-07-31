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
using FH5Data;
using FH5Interface;
using System.Windows.Markup;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Interop;
using Common;

namespace FSelector
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void InitData()
        {
            ImportData.Import(@"data\fh5");
        }

        private void FinaliseData()
        {
            ImportData.Export(@"data\fh5");
        }

        public MainWindow()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
               typeof(FrameworkElement),
               new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            InitData();

            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FinaliseData();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(this);
            Screen screen = Screen.FromHandle(windowInteropHelper.Handle);
            MaxHeight = screen.Bounds.Height + 16;
            MaxWidth = screen.Bounds.Width + 16;
        }
    }
}
