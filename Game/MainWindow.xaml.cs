using System;
using System.Collections.Generic;
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
using Microsoft.AspNet.SignalR.Client;

namespace PexGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow Main; //(1) Declare object as static
        public String UserName { get; set; }
        public IHubProxy HubProxy { get; set; }
        const string ServerUri = "http://localhost:8084/signalr";
        public HubConnection Connection { get; set; }
        public string TestProp { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            TestProp = "xxxx";
            ConnectAsync();

        }

        public void RegClient()
        {
            HubProxy.Invoke("RegisterClient", "MyName");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Main = this;
        }


        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerUri);
            //Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("GameHub");
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
                Console.WriteLine(
                    "Unable to connect to server: Start server before connecting clients.");
                //No connection: Don't enable Send button or show chat UI
                return;
            }
        }
    }
}