using System.Windows;
using System.Windows.Input;
using InventoryManagement.Models;

namespace InventoryManagement.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;
        private string _confirmPassword;
        private string _displayName;

        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged(); } }
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }
        public string ConfirmPassword { get { return _confirmPassword; } set { _confirmPassword = value; OnPropertyChanged(); } }
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand<Window>(p =>
            {
                if (Password == null || ConfirmPassword == null
                || UserName == null || DisplayName == null)
                {
                    return false;
                }
                if (!Password.Equals(ConfirmPassword))
                {
                    return false;
                }
                return true;
            }, p =>
            {
                InventoryManagementContext.INSTANCE.Add(new User()
                {
                    DisplayName = DisplayName,
                    Password = Password,
                    UserName = UserName,
                    RoleId = 2
                });
                InventoryManagementContext.INSTANCE.SaveChanges();
                MessageBox.Show("Đăng kí tài khoản thành công !");
                p.Close();
            });
        }
    }
}