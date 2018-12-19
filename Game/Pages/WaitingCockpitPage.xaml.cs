using System.Windows;
using System.Windows.Controls;

namespace Game.Pages
{
    /// <summary>
    /// Interaction logic for WaitingCockpitPage.xaml
    /// </summary>
    public partial class WaitingCockpitPage : Page
    {
        public WaitingCockpitPage()
        {
            InitializeComponent();
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.WaitingCockpitPage = this;
        }

        private void CancelInvitation(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.RejectInvitation(MainWindow.Main.Opponent);
        }
    }
}