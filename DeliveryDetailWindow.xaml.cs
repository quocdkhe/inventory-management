using System.Windows;
using InventoryManagement.ViewModel;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for DelieveryDetailWindow.xaml
    /// </summary>
    public partial class DeliveryDetailWindow : Window
    {
        public DeliveryDetailWindow(int DeliveryId, int CustomerId, Boolean IsEditable)
        {
            InitializeComponent();
            DeliveryDetailViewModel ViewModel = new DeliveryDetailViewModel(DeliveryId, CustomerId, IsEditable);
            this.DataContext = ViewModel;
        }
    }
}
