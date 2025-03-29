using System.Windows;
using InventoryManagement.ViewModel;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for ReceiptDetailWindow.xaml
    /// </summary>
    public partial class ReceiptDetailWindow : Window
    {
        private ReceiptDetailViewModel ViewModel;
        public ReceiptDetailWindow(int ReceiptId, int SupplierId, Boolean IsEditable)
        {
            InitializeComponent();
            ReceiptDetailViewModel ViewModel = new ReceiptDetailViewModel(ReceiptId, SupplierId, IsEditable);
            this.DataContext = ViewModel;
        }
    }
}
