using System.Windows;
using System.Windows.Controls;

namespace Game.Pages
{
    /// <summary>
    /// Interaction logic for ChoosePlayerPage.xaml
    /// </summary>
    public partial class ChoosePlayerPage : Page
    {
        public ChoosePlayerPage()
        {
            InitializeComponent();

            DataContext = MainWindow.Main;
            //DataContext = this;
            DataGrid.ItemsSource = MainWindow.Main.Players;
            //DataGrid.ItemsSource = Players;
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.ChoosePlayerPage = this;
            MainWindow.Main.RequestPlayers();
            NameLabel.Content = "Player: " + MainWindow.Main.UserName;
        }

        private void OnPlayGameClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GamePage());
        }

        public void RereshGui()
        {
            this.Dispatcher.Invoke(() => { DataGrid.ItemsSource = MainWindow.Main.Players; });
        }

        private void OnSearchPlayerClicked(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.SearchPlayer(SearchNameTextBox.Text);
        }
    }
}