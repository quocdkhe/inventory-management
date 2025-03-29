using System.Windows;
using System.Windows.Controls;
using InventoryManagement.ViewModel;

namespace InventoryManagement
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private ChangePassword chgPwdVM;
        public ChangePasswordWindow(int UserId)
        {
            InitializeComponent();
            chgPwdVM = new ChangePassword();
            this.DataContext = chgPwdVM;
            chgPwdVM.UserId = UserId;
        }

        private void OnChangeNewPassword(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                chgPwdVM.Password = passwordBox.Password;
            }
        }

        private void OnChangeConfirmPassword(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                chgPwdVM.ConfirmPassword = passwordBox.Password;
            }
        }
    }
}
