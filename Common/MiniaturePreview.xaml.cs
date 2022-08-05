using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Common
{
    /// <summary>
    /// Logique d'interaction pour MiniaturePreview.xaml
    /// </summary>
    public partial class MiniaturePreview : UserControl
    {
        public int CarNumber
        {
            set
            {
                ImagePaths = Directory.GetFiles(FOLDERPATH, value.ToString("0000") + "*");
                ImageIndex = 0;

                if (ImagePaths.Length > 1)
                {
                    PreviewImages = ImagePaths.Select(p => new BitmapImage(new Uri(p, UriKind.Absolute))).ToArray();
                    PreviewImage.Source = Miniature.Source = PreviewImages[0];
                    ToolTip = InternalTooltip;
                }
                else
                {
                    PreviewImage.Source = Miniature.Source = null;
                    ToolTip = null;
                }
            }
        }

        private static readonly string FOLDERBASE = Path.Combine(Data.APPPATH, "data");
        private string SUBFOLDER = string.Empty;
        private string FOLDERPATH = string.Empty;

        public static bool CheckMinis(string subfolder, int carno)
        {
            var path = Path.Combine(FOLDERBASE, subfolder, "minis");
            return Directory.GetFiles(path, carno.ToString("0000") + "*").Length > 0;
        }

        private string[] ImagePaths { get; set; }
        private int ImageIndex { get; set; }

        private Image PreviewImage { get; set; }
        private BitmapImage[] PreviewImages { get; set; }
        private ToolTip InternalTooltip;
        private DispatcherTimer InternalTimer = new DispatcherTimer();

        public MiniaturePreview()
        {
            SUBFOLDER = "fh5";
            FOLDERPATH = Path.Combine(FOLDERBASE, SUBFOLDER, "minis");

            InitializeComponent();

            PreviewImage = new Image();

            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            InternalTooltip = new ToolTip();
            InternalTooltip.Opened += Tooltip_Opened;
            InternalTooltip.Closed += Tooltip_Closed;
            InternalTooltip.Content = PreviewImage;
            InternalTimer.Tick += Timer_Tick;
            InternalTimer.Interval = new TimeSpan(0, 0, 5);
        }

        public void SetSubfolder(string subf)
        {
            SUBFOLDER = subf;
            FOLDERPATH = Path.Combine(FOLDERBASE, SUBFOLDER, "minis");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            PreviewImage.Source = PreviewImages[++ImageIndex % ImagePaths.Length];
        }

        void Tooltip_Closed(object sender, RoutedEventArgs e)
        {
            InternalTimer.Stop();
        }

        void Tooltip_Opened(object sender, RoutedEventArgs e)
        {
            if (PreviewImages.Length > 1)
            {
                ImageIndex = 0;
                InternalTimer.Start();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (PreviewImage == null) return;
            var window = Window.GetWindow(this);
            if (window == null) return;
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            PreviewImage.MaxHeight = .66 * screen.Bounds.Height;
            PreviewImage.MaxWidth = .66 * screen.Bounds.Width;
        }
    }
}
