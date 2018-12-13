using System;
using System.Collections.Generic;
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
using PexGame;

namespace Game
{
    /// <summary>
    /// Interaction logic for RegisterPlayerPage.xaml
    /// </summary>
    public partial class RegisterPlayerPage : Page
    {
        public RegisterPlayerPage()
        {
            InitializeComponent();
        }

        private void OnRegisterUserClicked(object sender, RoutedEventArgs e)
        {
            var x = NickNameTextBox.Text;


            NavigationService.Navigate(new ChoosePlayerPage());
            //MainWindow.Main.ConnectAsync(NickNameTextBox.Text);
            Console.WriteLine(MainWindow.Main.TestProp);

        }


    }
}
