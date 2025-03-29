using System.Windows;
using InventoryManagement.Models;
using InventoryManagement.ViewModel;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for DeliveryWindow.xaml
    /// </summary>
    public partial class DeliveryWindow : Window
    {
        public DeliveryWindow(User LoggedInUser)
        {
            InitializeComponent();
            DeliveryViewModel ViewModel = new DeliveryViewModel(LoggedInUser);
            this.DataContext = ViewModel;
        }
    }
}
