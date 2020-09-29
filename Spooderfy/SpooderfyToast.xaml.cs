using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spooderfy
{
    /// <summary>
    /// Interaction logic for SpooderfyToast.xaml
    /// </summary>
    public partial class SpooderfyToast : Window
    {
        public Timer Timer = new Timer(750);
        public bool Exited { get; set; } = false;
        private bool _editingMode;
        public bool EditingMode
        {
            get
            {
                return _editingMode;
            }
            set
            {
                _editingMode = value;
                this.Cursor = (value == true) ? Cursors.Arrow : Cursors.Hand;
                this.ResizeMode = (value == true) ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;
                this.Background = (value == true) ? Brushes.Black : Brushes.Transparent;
            }
        }

        public SpooderfyToast()
        {
            InitializeComponent();
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
            AddHandler(FrameworkElement.MouseDownEvent, new MouseButtonEventHandler(Button_MouseDown), true);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (SpooderfyContent.Spooder.Opened && this.Visibility == Visibility.Visible)
            {
                var Data = SpooderfySongData.GetData();
                if (RequiresUpdate)
                {
                    RequiresUpdate = false;
                    this.Dispatcher.Invoke(new Action(() => { SpooderfyHelper.CreateImageForStreaming(this, (int)this.Width, (int)this.Height);  }));
                }
                if (lastTrackId != Data.Id)
                {
                    ArtistName.Dispatcher.Invoke(new Action(() => { ArtistName.Text = Data.Artists; }));
                    SongName.Dispatcher.Invoke(new Action(() => { SongName.Text = Data.Title; }));
                    AlbumImage.Dispatcher.Invoke(() => AlbumImage.Source = SpooderfyHelper.ImageFromUrl(Data.Image));
                    RequiresUpdate = true;
                    lastTrackId = Data.Id;
                }
            }
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SpooderfyNatives.ReleaseCapture();
                SpooderfyNatives.SendMessage(new WindowInteropHelper(this).Handle, SpooderfyNatives.WM_NCLBUTTONDOWN, SpooderfyNatives.HT_CAPTION, 0);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Timer.Stop();
            Exited = true;
        }

        private string LastTrackId
        {
            get
            {
                return lastTrackId;
            }
            set
            {
                lastTrackId = value;
            }
        }
        private string lastTrackId;
        private bool RequiresUpdate = false;
    }
}
