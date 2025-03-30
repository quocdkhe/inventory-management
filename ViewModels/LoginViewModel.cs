using System.Windows;
using System.Windows.Input;
using InventoryManagement.Models;

namespace InventoryManagement.ViewModel
{

    class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; } = false;

        private string _userName;
        private string _password;
        public User LoggedInUser { get; set; }
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }




        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (p == null) return;

                User account = InventoryManagementContext.INSTANCE
                .Users.FirstOrDefault(user =>
                user.UserName == UserName && user.Password == Password);

                if (account != null)
                {
                    IsLogin = true;
                    LoggedInUser = account;
                    p.Close();
                }
                else
                {
                    IsLogin = false;
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu");
                }
            });

            RegisterCommand = new RelayCommand<object>(p => { return true; }, p =>
            {
                RegisterWindow registerWindow = new RegisterWindow();
                registerWindow.ShowDialog();
            });
        }
    }
}
