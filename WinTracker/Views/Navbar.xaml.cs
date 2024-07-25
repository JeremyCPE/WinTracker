using System.Windows;
using System.Windows.Controls;

namespace WinTracker.Views
{
    /// <summary>
    /// Logique d'interaction pour Navbar.xaml
    /// </summary>
    public partial class Navbar : UserControl
    {
        public Navbar()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Home());
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Dashboard());
        }
    }
}
