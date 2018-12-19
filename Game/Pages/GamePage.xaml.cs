using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Game.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public readonly List<Button> ListOfButtonsToDelete;

        public GamePage()
        {
            this.ListOfButtonsToDelete = new List<Button>();
            InitializeComponent();
        }

        public void NavigateToChoosePlayerPage()
        {
            this.Dispatcher.Invoke(() =>
            {
                MainWindow.Main.MainFrame.NavigationService.Navigate(MainWindow.Main.ChoosePlayerPage);
            });
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            CreateGameScenario(2, 2);
        }

        public void CreateGameScenario(int m, int n)
        {
            for (int i = 0; i < m; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Auto
                });
            }

            for (int i = 0; i < n; i++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });
            }

            Button batak;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    batak = new Button()
                    {
                        Content = "Picture",
                        Height = 80,
                        Width = 80
                    };
                    ListOfButtonsToDelete.Add(batak);
                    batak.Click += this.ClickOnCardHandler;

                    GameGrid.Children.Add(batak);
                    Grid.SetColumn(batak, j);
                    Grid.SetRow(batak, i);
                }
            }
        }

        private void ClickOnCardHandler(object sender, EventArgs e)
        {
            var batak = sender as Button;
            int x = (int) batak.GetValue(Grid.RowProperty);
            int y = (int) batak.GetValue(Grid.ColumnProperty);
            Console.WriteLine("X:" + x + " Y: " + y);
        }

        public void RemoveCardAtXY(int x, int y)
        {
            GameGrid.Children.Remove(ListOfButtonsToDelete.Find(n =>
            {
                return ((int) n.GetValue(Grid.RowProperty) == x) &&
                       ((int) n.GetValue(Grid.ColumnProperty) == y);
            }));
        }

        public void DisableAllControll()
        {
            foreach (UIElement element in GameGrid.Children)
            {
                UIElement TheElement = element as UIElement;
                TheElement.IsEnabled = false;
            }
        }

        public void EnableAllControll()
        {
            foreach (UIElement element in GameGrid.Children)
            {
                UIElement TheElement = element as UIElement;
                TheElement.IsEnabled = true;
            }
        }


        private void SendMessage(object sender, RoutedEventArgs e)
        {
            //MainWindow.Main.SendMessage(MainWindow.Main.Opponent, Message.Text);
        }
    }
}