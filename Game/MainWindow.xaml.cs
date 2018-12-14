using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
using Game;
using Game.Entities;
using Microsoft.AspNet.SignalR.Client;

namespace PexGame
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

        public void RegClient(string name)
        {
            HubProxy.Invoke("RegisterClient", name);
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
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            //HubProxy.On<string, string>("AddMessage", (name, message) =>
            //    this.Dispatcher.Invoke(() =>
            //        RichTextBoxConsole.AppendText(String.Format("{0}: {1}\r", name, message))
            //    )
            //);
            HubProxy.On("registerComplete", () => { Console.WriteLine("Completed Registration"); });
            HubProxy.On("listOfPlayers", (List<Player> players) =>
                {
                    Console.WriteLine("List of players");
                    ShowAllPlayers(players);
                }
            );

            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                Console.WriteLine(
                    "Unable to connect to server: Start server before connecting clients.");
                //No connection: Don't enable Send button or show chat UI
                return;
            }
        }

        private void Connection_Closed()
        {
            Console.WriteLine("con closed");
        }


        public void ShowAllPlayers(List<Player> t)
        {

            Players = new ObservableCollection<Player>(t);
            ChoosePlayerPage.RereshGui();

        }

        public void RequestPlayers()
        {
            HubProxy.Invoke("RefreshPlayers");
        }

        private void Window_OnClosed(object sender, EventArgs e)
        {
            Connection.Stop();
        }
    }
}