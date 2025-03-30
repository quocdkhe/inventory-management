using System.Windows;
using System.Windows.Input;
using InventoryManagement.Models;

namespace InventoryManagement.ViewModel
{
    public class ChangePassword : BaseViewModel
    {
        public Boolean isChanged { get; set; }
        private string _password;
        private string _confirmPassword;
        private int _userId;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; OnPropertyChanged(); }
        }
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(); }
        }

        public ICommand SubmitChangePassword { get; set; }

        public ChangePassword()
        {
            SubmitChangePassword = new RelayCommand<Window>((p) =>
            {
                if (Password == null || ConfirmPassword == null)
                {
                    return false;
                }
                return Password.Equals(ConfirmPassword);
            }, (p) =>
            {
                User? user = InventoryManagementContext.INSTANCE.Users.FirstOrDefault(user => user.Id == UserId);
                if (user != null)
                {
                    user.Password = Password;
                    InventoryManagementContext.INSTANCE.SaveChanges();
                    isChanged = true;
                    MessageBox.Show("Đổi mật khẩu thành công");
                    p.Close();
                }
                else
                {
                    isChanged = false;
                }
            });
        }
    }
}
