using System.Linq;
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
            McDataGrid.ItemsSource = MainWindow.Main.Players;
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.ChoosePlayerPage = this;
            MainWindow.Main.RequestPlayers();
            NameLabel.Content = "Player: " + MainWindow.Main.UserName;
        }

        private void OnPlayGameClicked(object sender, RoutedEventArgs e)
        {
            
            var item =  McDataGrid.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Vybrete prosím hráča");
            }
            else if (item.InGame == true)
            {
                MessageBox.Show("Vybrete prosím hráča, ktorý momenatálne nie je v hre");
            }
            else
            {
                
            }

            //NavigationService.Navigate(new GamePage());
        }

        public void RereshGui()
        {
            //this.Dispatcher.Invoke(() => { DataGrid.ItemsSource = MainWindow.Main.Players; });
            this.Dispatcher.Invoke(() =>
            {
                var playersReduced = MainWindow.Main.Players
                    .Select(n => new {n.Name, InGame = n.IsPlaying || n.HasInvitation,}).ToList();
                McDataGrid.ItemsSource = playersReduced;
            });
        }

        private void OnSearchPlayerClicked(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.SearchPlayer(SearchNameTextBox.Text);
        }

        private void OnPlayGameRandomPlayerClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}