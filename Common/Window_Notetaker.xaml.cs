using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Window_Notetaker.xaml
    /// </summary>
    public partial class Window_Notetaker : Window
    {
        private const string NEWLINE = "\r\n";
        private const string NEWSECTION = ">>";
        private const string SECTIONCLOSE = "<<";
        private const string FILEPATH = @"data\notes.txt";
        private bool DELMODE = false;

        public bool IsOpen { get; private set; }

        public Window_Notetaker()
        {
            InitializeComponent();

            if (File.Exists(FILEPATH))
                Load(File.ReadAllText(FILEPATH));
            else
                Load(string.Empty);

            Topmost = true;
            BtnPin.ToolTip = "Pinned on foreground\nClick to unpin";
        }

        public void Save()
        {
            File.WriteAllText(FILEPATH, Get());
            Title = "Notes";
        }

        private string Get()
        {
            List<string> filecontent = new List<string>();

            foreach (var child in Container.Children)
            {
                if (child is TextBlock)
                {
                    TextBlock ELEMENT = child as TextBlock;
                    //if (ELEMENT.Tag is bool && !((bool)ELEMENT.Tag))
                    //filecontent.Add(NEWSECTION + ELEMENT.Text + SECTIONCLOSE);
                    if (ELEMENT.Tag is UIElement && (ELEMENT.Tag as UIElement).Visibility == Visibility.Collapsed)
                        filecontent.Add(NEWSECTION + ELEMENT.Text + SECTIONCLOSE);
                    else
                        filecontent.Add(NEWSECTION + ELEMENT.Text);
                }
                else if (child is TextBox)
                {
                    TextBox ELEMENT = child as TextBox;
                    filecontent.Add(ELEMENT.Text);
                }
            }

            var result = string.Join(NEWLINE, filecontent);
            Load(result);
            return result;
        }

        public void Load(string raw)
        {
            Container.Children.Clear();

            if (string.IsNullOrEmpty(raw))
            {
                MakeTextBox(string.Empty, null);
                return;
            }

            TextBlock sectionHeader = null;

            string buffer = string.Empty;
            bool trigger = false;
            string[] filecontent = raw.Split(new string[] { NEWLINE }, StringSplitOptions.None);
            for (var i = 0; i < filecontent.Length; i++)
            {
                if (filecontent[i].StartsWith(NEWSECTION))
                {
                    if (trigger)
                    {
                        MakeTextBox(buffer, sectionHeader);
                        buffer = string.Empty;
                    }

                    TextBlock TXT = new TextBlock();

                    if (filecontent[i].EndsWith(SECTIONCLOSE))
                    {
                        TXT.Tag = false;
                        TXT.Text = filecontent[i].Substring(NEWSECTION.Length, filecontent[i].Length - NEWSECTION.Length - SECTIONCLOSE.Length);
                    }
                    else
                        TXT.Text = filecontent[i].Substring(NEWSECTION.Length);
                    TXT.Foreground = Brushes.White;
                    TXT.FontWeight = FontWeights.Bold;
                    TXT.Padding = new Thickness(5, 5, 0, 0);
                    TXT.MouseUp += TXT_MouseUp;
                    TXT.MouseEnter += TXT_MouseEnter;
                    TXT.MouseLeave += TXT_MouseLeave;
                    sectionHeader = TXT;
                    Container.Children.Add(TXT);
                }
                else
                {
                    if (!string.IsNullOrEmpty(buffer)) buffer += NEWLINE;
                    buffer += filecontent[i];
                }
                trigger = true;
            }
            if (!string.IsNullOrEmpty(buffer))
            {
                MakeTextBox(buffer, sectionHeader);
                buffer = string.Empty;
            }
        }

        void TXT_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Background = Brushes.Transparent;
        }

        void TXT_MouseEnter(object sender, MouseEventArgs e)
        {
            if (DELMODE)
                (sender as TextBlock).Background = Brushes.LightPink;
            else
                (sender as TextBlock).Background = new SolidColorBrush(Color.FromRgb(175, 175, 175));
        }

        void TXT_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DELMODE)
            {
                Container.Children.Remove(sender as UIElement);
            }
            else
            {
                var txt = sender as TextBlock;
                if (txt.Tag != null && txt.Tag is TextBox)
                {
                    var box = txt.Tag as TextBox;
                    box.Visibility = box.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        private void MakeTextBox(string content, TextBlock sectionHeader)
        {
            TextBox BOX = new TextBox();
            BOX.Text = content;
            BOX.TextWrapping = TextWrapping.Wrap;
            BOX.AcceptsReturn = true;
            BOX.TextChanged += BOX_TextChanged;
            BOX.PreviewMouseUp += BOX_MouseUp;
            BOX.MouseEnter += BOX_MouseEnter;
            BOX.MouseLeave += BOX_MouseLeave;
            Container.Children.Add(BOX);

            try { BOX.FontFamily = new FontFamily("Consolas"); }
            catch { }

            if (sectionHeader != null)
            {
                if (sectionHeader.Tag is bool && !((bool)sectionHeader.Tag))
                    BOX.Visibility = Visibility.Collapsed;
                sectionHeader.Tag = BOX;
            }
        }

        void BOX_MouseLeave(object sender, MouseEventArgs e)
        {
            if (DELMODE)
                (sender as TextBox).Background = Brushes.White;
        }

        void BOX_MouseEnter(object sender, MouseEventArgs e)
        {
            if (DELMODE)
                (sender as TextBox).Background = Brushes.LightPink;
        }

        void BOX_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (DELMODE)
                Container.Children.Remove(sender as UIElement);
        }

        void BOX_TextChanged(object sender, TextChangedEventArgs e)
        {
            Title = "Notes*";
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Key == Key.S)
                {
                    Save();
                }
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            DELMODE = !DELMODE;

            BtnSave.IsEnabled = BtnReload.IsEnabled = !DELMODE;
            if (DELMODE) { BtnDel.Background = Brushes.LightPink; }
            else
            {
                BtnDel.Background = SystemColors.ControlBrush;
                Get();
                //Load(Get());
            }
        }

        private void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            Get();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Title.Contains("*"))
            {
                var dr = MessageBox.Show("Do you want to save changes before closing?", "Wait a second!", MessageBoxButton.YesNo);
                if (dr == MessageBoxResult.Yes)
                    Save();
            }
            IsOpen = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
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
