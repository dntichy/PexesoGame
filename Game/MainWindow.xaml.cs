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

        public ObservableCollection<Player> Players { get; set; }

        public String UserName { get; set; }

        public IHubProxy HubProxy { get; set; }
        const string ServerUri = "http://localhost:8084/signalr";
        public HubConnection Connection { get; set; }
        public string TestProp { get; set; }

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

            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            //HubProxy.On<string, string>("AddMessage", (name, message) =>
            //    this.Dispatcher.Invoke(() =>
            //        RichTextBoxConsole.AppendText(String.Format("{0}: {1}\r", name, message))
            //    )
            //);


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

        public void RegClient()
        {
            HubProxy.Invoke("RegisterClient", UserName);
        }

        public void SearchPlayer(string searchName)
        {
            HubProxy.Invoke("SearchPlayer", searchName);
        }

        private void Window_OnClosed(object sender, EventArgs e)
        {
            Connection.Stop();
        }
    }
}