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
using System.Timers;

namespace Spooderfy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Timer Timer = new Timer(1);
        public SpooderfyToast Toast = new SpooderfyToast();


        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            debug.Visibility = Visibility.Visible;
#endif
        }

        private void ShowSecret_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(swclientSecretBox.Visibility == Visibility.Visible)
            {
                swclientSecretBox.Visibility = Visibility.Hidden;
                hdclientSecretBox.Visibility = Visibility.Visible;
            }
            else
            {
                swclientSecretBox.Visibility = Visibility.Visible;
                hdclientSecretBox.Visibility = Visibility.Hidden;
            }
        }

        private void hdclientSecretBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (hdclientSecretBox.Visibility == Visibility.Hidden)
                return;
            else
                swclientSecretBox.Text = hdclientSecretBox.Password;
            SpooderfyContent.Credentials.ClientSecret = hdclientSecretBox.Password;
        }

        private void swclientSecretBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (swclientSecretBox.Visibility == Visibility.Hidden)
                return;
            else
                hdclientSecretBox.Password = swclientSecretBox.Text;
            SpooderfyContent.Credentials.ClientSecret = swclientSecretBox.Text;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SpooderfyContent.Credentials = SpooderfyCredentials.Load();
            SpooderfyContent.Spooder = Spooder.Load(Spooder_OpenStateChanged);
            redirectUriBox.Text = SpooderfyContent.Credentials.RedirectUrl;
            clientIdBox.Text = SpooderfyContent.Credentials.ClientId;
            hdclientSecretBox.Password = SpooderfyContent.Credentials.ClientSecret;

            SpooderfyContent.Spooder.OpenStateChanged += Spooder_OpenStateChanged;
        }

        public void Spooder_OpenStateChanged(bool State)
        {
            try
            {
                StateInfo.Dispatcher.Invoke(new Action(() => { StateInfo.Content = $"State: {(State == true ? "Open" : "Closed")}"; }));
                clientIdBox.Dispatcher.Invoke(new Action(() => { clientIdBox.IsEnabled = !State; }));
                hdclientSecretBox.Dispatcher.Invoke(new Action(() => { hdclientSecretBox.IsEnabled = !State; }));
                swclientSecretBox.Dispatcher.Invoke(new Action(() => { swclientSecretBox.IsEnabled = !State; }));
                redirectUriBox.Dispatcher.Invoke(new Action(() => { redirectUriBox.IsEnabled = false; }));
                openBtn.Dispatcher.Invoke(new Action(() => { openBtn.IsEnabled = !State; }));
            }
            catch { }
        }

        private void clientIdBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SpooderfyContent.Credentials.ClientId = clientIdBox.Text;
        }

        private void redirectUriBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SpooderfyContent.Credentials.RedirectUrl = redirectUriBox.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SpooderfyContent.Spooder.Open(SpooderfyContent.Credentials);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Toast.Close();
            Timer.Stop();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (Toast.Exited)
                Toast = new SpooderfyToast();
            Toast.Show();
            Toast.Width = toastwidth.Value;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Toast.Exited)
                Toast = new SpooderfyToast();
            Toast.Hide();
            Toast.Width = toastwidth.Value;
        }

        private void DebugBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(SpooderfyContent.Spooder.Token.RefreshToken);
            SpooderfyContent.Spooder.Refresh();

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Toast.Width = toastwidth.Value;
            }
            catch
            {

            }
        }

        private void editingMode_Checked(object sender, RoutedEventArgs e)
        {
            if (Toast.Exited)
                Toast = new SpooderfyToast();
            Toast.EditingMode = editingMode.IsChecked.Value;
        }

        private void editingMode_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Toast.Exited)
                Toast = new SpooderfyToast();
            Toast.EditingMode = editingMode.IsChecked.Value;
        }
    }
}
