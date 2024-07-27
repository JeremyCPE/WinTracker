using System.Windows.Controls;
using WinTracker.ViewModels;

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
            DataContext = new NavbarViewModel();
        }
    }
}
