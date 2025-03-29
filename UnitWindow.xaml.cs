using System.Windows;
using InventoryManagement.ViewModel;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for UnitWindow.xaml
    /// </summary>
    public partial class UnitWindow : Window
    {
        public UnitWindow()
        {
            InitializeComponent();
            this.DataContext = new UnitViewModel();
        }
    }
}
