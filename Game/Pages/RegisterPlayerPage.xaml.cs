using System.Windows;
using System.Windows.Controls;
using PexGame;

namespace Game.Pages
{
    /// <summary>
    /// Interaction logic for RegisterPlayerPage.xaml
    /// </summary>
    public partial class RegisterPlayerPage : Page
    {
        public RegisterPlayerPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.RegisterPage = this;
        }

        private void OnRegisterUserClicked(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.UserName = NickNameTextBox.Text;
            MainWindow.Main.RegClient();
        }


        public void NavigateToChoosePlayerPage()
        {
            this.Dispatcher.Invoke(() =>
            {
                ChoosePlayerPage cp = new ChoosePlayerPage();
                NavigationService.Navigate(cp);
            });
        }
    }
}