using System.Windows;
using System.Windows.Controls;
using InventoryManagement.ViewModel;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private RegisterViewModel _viewModel;
        public RegisterWindow()
        {
            InitializeComponent();
            _viewModel = new RegisterViewModel();
            this.DataContext = _viewModel;
        }

        private void OnChangePassword(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.Password = passwordBox.Password;
            }
        }

        private void OnChangeConfirmPassword(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.ConfirmPassword = passwordBox.Password;
            }
        }
    }
}
