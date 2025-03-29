using System.Windows;
using InventoryManagement.Models;
using InventoryManagement.ViewModel;

namespace InventoryManagement
{
    public partial class ReceiptWindow : Window
    {
        public ReceiptWindow(User LoggedInUser)
        {
            InitializeComponent();
            ReceiptViewModel ViewModel = new ReceiptViewModel();
            this.DataContext = ViewModel;
            ViewModel.LoggedInUser = LoggedInUser;
        }
    }
}
