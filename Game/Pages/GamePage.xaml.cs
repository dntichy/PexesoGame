using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Game.Entities;

namespace Game.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private DispatcherTimer _timer;

        public GamePage()
        {
            InitializeComponent();
            _timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ProgressBar.Value++;

            if (ProgressBar.Value.Equals(ProgressBar.Maximum))
            {
                _timer.Tick -= Timer_Tick;
                _timer.Stop();
                MessageBox.Show("You were disconnected from the party :)");
                MainWindow.Main.DisconnectFromParty();
                MainWindow.Main.MainFrame.NavigationService.Navigate(MainWindow.Main.ChoosePlayerPage);
            }
        }

        private void InitializeTimer()
        {
            ProgressBar.Maximum = 60;
            ProgressBar.Value = 0;
            _timer.Stop();
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
                        Background = Brushes.White,
                        Content = "",
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Height = 80,
                        Width = 80
                    };
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
            MainWindow.Main.Moved(x, y);
        }

        public void RemoveCardAtXy(int x, int y)
        {
            for (int i = 0; i < GameGrid.Children.Count; i++)
            {
                UIElement e = GameGrid.Children[i];
                if (Grid.GetRow(e) == x && Grid.GetColumn(e) == y)
                {
                    GameGrid.Children.Remove(e);
                }
            }
        }

        public void RemovePicturesFromCardsAtXy(int x, int y)
        {
            UIElement btn = null;
            for (int i = 0; i < GameGrid.Children.Count; i++)
            {
                UIElement e = GameGrid.Children[i];
                if (Grid.GetRow(e) == x && Grid.GetColumn(e) == y)
                {
                    btn = e;
                }
            }

            var batak = btn as Button;

            batak.Content = null;
        }

        public void DisableAllControll()
        {
            Dispatcher.Invoke(() =>
            {
                foreach (UIElement element in GameGrid.Children)
                {
                    UIElement TheElement = element as UIElement;
                    TheElement.IsEnabled = false;
                    TurnSwitcherText.Content = "IT'S OPPONENT'S TURN";
                }

                InitializeTimer();
            });
        }

        public void EnableAllControll()
        {
            Dispatcher.Invoke(() =>
            {
                foreach (UIElement element in GameGrid.Children)
                {
                    UIElement TheElement = element as UIElement;
                    TheElement.IsEnabled = true;
                    TurnSwitcherText.Content = "IT'S YOUR TURN";
                }

                InitializeTimer();
                _timer.Start();
            });
        }


        private void AddPictureToImage(int a, int b, Picture p)
        {
            UIElement btn = null;
            for (int i = 0; i < GameGrid.Children.Count; i++)
            {
                UIElement e = GameGrid.Children[i];
                if (Grid.GetRow(e) == a && Grid.GetColumn(e) == b)
                {
                    btn = e;
                }
            }

            var batak = btn as Button;

            Image img = new Image()
            {
                Source = ImageFromBuffer(p.Image)
            };
            batak.Content = img;
        }

        private BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(bytes))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }

            image.Freeze();
            return image;
        }

        public void ShowChangesRedoCards(int i, int i1, int arg3, int arg4, Picture picture)
        {
            Dispatcher.Invoke(async () =>
            {
                InitializeTimer();
                AddPictureToImage(i, i1, picture);

                await Task.Delay(1500);
                RemovePicturesFromCardsAtXy(i, i1);
                RemovePicturesFromCardsAtXy(arg3, arg4);
            });
        }

        public void ShowChangesScored(int i, int i1, int arg3, int arg4, Picture picture, Player theOneThatMoves)
        {
            Dispatcher.Invoke(async () =>
            {
                InitializeTimer();

                AddPictureToImage(i, i1, picture);

                //add score to theOneThatMoves
                SetScore(theOneThatMoves);

                //after 1,5 second => 
                await Task.Delay(1500);
                RemoveCardAtXy(i, i1);
                RemoveCardAtXy(arg3, arg4);
            });
        }

        private void SetScore(Player theOneThatMoves)
        {
            if (MainWindow.Main.UserName == theOneThatMoves.Name)
            {
                ScoreTrackMe.Content = "You: " + theOneThatMoves.Points;
                ScoreTrackOpp.Content = "Opponent: " + theOneThatMoves.Opponent.Points;
            }
            else
            {
                ScoreTrackOpp.Content = "Opponent: " + theOneThatMoves.Opponent.Points;
                ScoreTrackMe.Content = "You: " + theOneThatMoves.Points;
            }
        }

        public void ShowChanges(int i, int i1, Picture picture)
        {
            Dispatcher.Invoke(() => { AddPictureToImage(i, i1, picture); });
        }

        public void MessageRecieved(string message, string fromUser)
        {
            Dispatcher.Invoke(() => { MessageConsole.Items.Add(fromUser + ": " + message); });
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.SendMessage(MainWindow.Main.Opponent, Message.Text);
            Dispatcher.Invoke(() => { MessageConsole.Items.Add(MainWindow.Main.UserName + ": " + Message.Text); });
        }

        public void GameOver(string winnerName)
        {
            Dispatcher.Invoke(() =>
            {
                InitializeTimer();
                MessageBox.Show(winnerName == MainWindow.Main.UserName ? "CONGRATULATIONS YOU WON :)" : "YOU LOSE :(");
                MainWindow.Main.MainFrame.NavigationService.Navigate(MainWindow.Main.ChoosePlayerPage);
            });
        }
    }
}