using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Game.Entities;

namespace Game.Pages
{
    /// <summary>
    /// Interaction logic for ChoosePlayerPage.xaml
    /// </summary>
    public partial class ChoosePlayerPage : Page
    {
        private string _gameType;

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
            var item = (PlayerWrapper) McDataGrid.SelectedItem;
            if (item == null)
            {
                MessageBox.Show("Vybrete prosím hráča");
            }
            else if (item.InGame == true)
            {
                MessageBox.Show("Vybrete prosím hráča, ktorý momenatálne nie je v hre");
            }
            else if (_gameType == null)
            {
                MessageBox.Show("Vybrete prosím typ hry");
            }
            else
            {
                MainWindow.Main.ChallengePlayer(item.Name, _gameType);
            }

            //NavigationService.Navigate(new GamePage());
        }

        public void RereshGui()
        {
            //this.Dispatcher.Invoke(() => { DataGrid.ItemsSource = MainWindow.Main.Players; });
            this.Dispatcher.Invoke(() =>
            {
                var playersReduced = MainWindow.Main.Players
                    .Select(n => new PlayerWrapper()
                        {Name = n.Name, InGame = n.IsPlaying || n.HasInvitation,}).ToList();
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

        private void OnCheckRadioButton(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null) _gameType = radioButton.Content.ToString();
        }

        public void NavigateToWaitingCockpit()
        {
            this.Dispatcher.Invoke(() =>
            {
                WaitingCockpitPage wc = new WaitingCockpitPage();
                NavigationService.Navigate(wc);
            });
        }

        public void NavigateToGamePage()
        {
            this.Dispatcher.Invoke(() =>
            {
                MainWindow.Main.MainFrame.NavigationService.Navigate(MainWindow.Main.GamePage);
            });
        }
    }
}