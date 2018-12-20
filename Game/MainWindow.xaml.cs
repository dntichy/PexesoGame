using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Game.Entities;
using Game.Pages;
using Microsoft.AspNet.SignalR.Client;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow Main; //(1) Declare object as static


        public RegisterPlayerPage RegisterPage;
        public ChoosePlayerPage ChoosePlayerPage;
        public GamePage GamePage;
        public WaitingCockpitPage WaitingCockpitPage;
        public ObservableCollection<Player> Players { get; set; }

        public String UserName { get; set; }
        public String Opponent { get; set; }
        public String PotentialOpponent { get; set; }

        public IHubProxy HubProxy { get; set; }
        const string ServerUri = "http://localhost:8084/signalr";
        public HubConnection Connection { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            GamePage = new GamePage();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Main = this;
        }


        private async Task ConnectAsync()
        {
            Connection = new HubConnection(ServerUri);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("GameHub");

            HubProxy.On("registerComplete", () =>
            {
                Console.WriteLine("Completed Registration");
                RegisterPage.NavigateToChoosePlayerPage();
            });
            HubProxy.On("registerCantCompleted",
                () =>
                {
                    Console.WriteLine("Registration Failed, user already exists");
                    MessageBox.Show("Zadané meno sa už používa, zvoľte prosím iné");
                });

            HubProxy.On("listOfPlayers", (List<Player> players) =>
                {
                    Console.WriteLine("List of players");

                    List<Player> playersReduced = players.FindAll(n => n.Name != UserName);
                    Players = new ObservableCollection<Player>(playersReduced);
                    ChoosePlayerPage.RereshGui();
                }
            );

            HubProxy.On("listOfSearchedPlayers", (List<Player> players) =>
                {
                    Console.WriteLine("List of searched players");
                    List<Player> playersReduced = players.FindAll(n => n.Name != UserName);
                    Players = new ObservableCollection<Player>(playersReduced);
                    ChoosePlayerPage.RereshGui();
                }
            );


            HubProxy.On("opponentDisconnected",
                (string opponentName) =>
                {
                    MessageBox.Show("Player " + opponentName + " has disconnected the party :(");
                    GamePage.NavigateToChoosePlayerPage();
                }
            );


            HubProxy.On("showChangesRedoCards",
                (int a, int b, int x, int y, Picture picture) => { GamePage.ShowChangesRedoCards(a, b, x, y, picture); }
            );

            HubProxy.On("showChangesScored",
                (int a, int b, int x, int y, Picture picture, Player theOneThatMoves) =>
                {
                    GamePage.ShowChangesScored(a, b, x, y, picture, theOneThatMoves);
                }
            );
            HubProxy.On("showChanges",
                (int a, int b, Picture picture) => { GamePage.ShowChanges(a, b, picture); }
            );

            HubProxy.On("gotInvitation", (string opponentName) =>
                {
                    if (MessageBox.Show("Accept invitation from" + opponentName + " ?",
                            "Invitation",
                            MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                    {
                        Opponent = opponentName;
                        AcceptInvitation(opponentName);
                        ChoosePlayerPage.NavigateToGamePage();
                    }
                    else
                    {
                        RejectInvitation(opponentName);
                        RequestPlayers();
                    }
                }
            );

            HubProxy.On("challengePlayerFailed", (string opponentName) => { Console.WriteLine("Failed"); }
            );
            HubProxy.On("challengeAccepted", () => { ChoosePlayerPage.NavigateToGamePage(); }
            );
            HubProxy.On("createGameScenario",
                (int a, int b) => { this.Dispatcher.Invoke(() => { Main.GamePage.CreateGameScenario(a, b); }); }
            );
            HubProxy.On("move", () => { GamePage.EnableAllControll(); }
            );
            HubProxy.On("waitForMove", () => { GamePage.DisableAllControll(); }
            );
            HubProxy.On("gameOver", (string winnerName) => { GamePage.GameOver(winnerName); }
            );

            HubProxy.On("gotMessage",
                (string message, string fromUser) => { GamePage.MessageRecieved(message, fromUser); }
            );

            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Unable to connect to server: Start server before connecting clients.");
                return;
            }
        }

        private void Connection_Closed()
        {
            Console.WriteLine("con closed");
            System.Windows.Application.Current.Shutdown();
        }

        public void RequestPlayers()
        {
            HubProxy.Invoke("RefreshPlayers");
        }

        public async Task RegClient(string name)
        {
            await ConnectAsync();
            UserName = name;
            HubProxy.Invoke("RegisterClient", name);
        }

        public void SearchPlayer(string searchName)
        {
            HubProxy.Invoke("SearchPlayer", searchName);
        }


        public void AcceptInvitation(string name)
        {
            HubProxy.Invoke("AcceptInvitation", name);
        }

        public void RejectInvitation(string name)
        {
            HubProxy.Invoke("RejectInvitation", name);
        }

        public void ChallengePlayer(string name, string gameType)
        {
            PotentialOpponent = name;
            ChoosePlayerPage.NavigateToWaitingCockpit();
            HubProxy.Invoke("ChallengePlayer", name, gameType);
        }

        public void Moved(int a, int b)
        {
            HubProxy.Invoke("Moved", a, b);
        }

        public void SendMessage(string name, string message)
        {
            HubProxy.Invoke("SendMessage", name, message);
        }

        private void Window_OnClosed(object sender, EventArgs e)
        {
            Connection?.Stop();
        }
    }
}