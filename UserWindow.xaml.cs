using System.Windows;
using System.Windows.Controls;
using InventoryManagement.ViewModel;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private UserViewModel userViewModel;
        public UserWindow()
        {
            InitializeComponent();
            userViewModel = new UserViewModel();
            this.DataContext = userViewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                userViewModel.Password = passwordBox.Password;
            }
        }
    }
}
