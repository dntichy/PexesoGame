using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
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
        public ObservableCollection<Player> Players { get; set; }

        public String UserName { get; set; }
        public String Opponent { get; set; }

        public IHubProxy HubProxy { get; set; }
        const string ServerUri = "http://localhost:8084/signalr";
        public HubConnection Connection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ConnectAsync();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Main = this;
        }


        private async void ConnectAsync()
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


            HubProxy.On("opponentDisconnected", (string opponentName) => { }
            );


            HubProxy.On("gotInvitation", (string opponentName) => { }
            );

            HubProxy.On("challengePlayerFailed", (string opponentName) => { }
            );


            HubProxy.On("move", () => { }
            );
            HubProxy.On("waitForMove", () => { }
            );
            HubProxy.On("gameOver", (string winnerName) => { }
            );

            HubProxy.On("gotMessage", (string message, string fromUser) => { }
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

        public void RegClient(string name)
        {
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
            Connection.Stop();
        }
    }
}