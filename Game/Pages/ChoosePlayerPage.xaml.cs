using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Game.Entities;
using Game.Pages;

namespace PexGame
{
    /// <summary>
    /// Interaction logic for ChoosePlayerPage.xaml
    /// </summary>
    public partial class ChoosePlayerPage : Page
    {
        public ChoosePlayerPage()
        {
            InitializeComponent();
            


            //Players = new ObservableCollection<Player>();
            //Players.Add(new Player()
            //{
            //    Name = "jolo"
            //});

            DataContext = MainWindow.Main;
            //DataContext = this;
            DataGrid.ItemsSource = MainWindow.Main.Players;
            //DataGrid.ItemsSource = Players;
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.ChoosePlayerPage = this;
            MainWindow.Main.RequestPlayers();
        }

        private void OnPlayGameClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GamePage());
        }

        public void RereshGui()
        {
            this.Dispatcher.Invoke(() =>
            {
                DataGrid.ItemsSource = MainWindow.Main.Players;
            }); 


        }
    }
}
