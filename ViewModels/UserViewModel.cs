using System.Collections.ObjectModel;
using System.Windows.Input;
using InventoryManagement.Models;

namespace InventoryManagement.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        private ObservableCollection<User> _list;
        private ObservableCollection<Role> _roles;
        private User _selectedItem;
        private string _displayName;
        private string _userName;
        private string _password;
        private Role _selectedRole;

        public ObservableCollection<User> List
        {
            get => _list; set
            {
                _list = value; OnPropertyChanged();
            }
        }
        public ObservableCollection<Role> Roles
        {
            get => _roles; set
            {
                _roles = value; OnPropertyChanged();
            }
        }

        public User SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    UserName = SelectedItem.UserName;
                    SelectedRole = SelectedItem.Role;
                    Password = "";
                }
            }
        }
        public Role SelectedRole
        {
            get => _selectedRole;
            set { _selectedRole = value; OnPropertyChanged(); }
        }
        public string? DisplayName
        {
            get => _displayName;
            set { _displayName = value; OnPropertyChanged(); }
        }
        public string? UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }
        public string? Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }

        private void LoadFromDatabase()
        {
            List = new ObservableCollection<User>(InventoryManagementContext.INSTANCE.Users);
            Roles = new ObservableCollection<Role>(InventoryManagementContext.INSTANCE.Roles);
        }
        public UserViewModel()
        {
            LoadFromDatabase();
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName)
                || string.IsNullOrEmpty(UserName)
                || string.IsNullOrEmpty(Password)
                || SelectedRole == null
                )
                {
                    return false;
                }

                bool isExisted = InventoryManagementContext.INSTANCE.Users
                    .Where(user => user.UserName == UserName).Count() != 0;

                return !isExisted;
            }, (p) =>
            {
                User user = new User
                {
                    DisplayName = DisplayName,
                    UserName = UserName,
                    Password = Password,
                    RoleId = SelectedRole.Id,
                };
                InventoryManagementContext.INSTANCE.Users.Add(user);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            EditCommand = new RelayCommand<object>((p) => SelectedItem != null
            , (p) =>
            {
                var currentUser = InventoryManagementContext
                    .INSTANCE
                    .Users
                    .FirstOrDefault(user => user.Id == SelectedItem.Id);
                currentUser.DisplayName = DisplayName;
                currentUser.UserName = UserName;
                currentUser.RoleId = SelectedRole.Id;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            DeleteCommand = new RelayCommand<object>((p) => SelectedItem != null
            , (p) =>
            {
                var currentUser = InventoryManagementContext.INSTANCE
                    .Users
                    .FirstOrDefault(user => user.Id == SelectedItem.Id);
                if (currentUser != null)
                {
                    InventoryManagementContext.INSTANCE.Users.Remove(currentUser);
                    InventoryManagementContext.INSTANCE.SaveChanges();
                }
                LoadFromDatabase();
            });

            ChangePasswordCommand = new RelayCommand<object>((canExecute) => SelectedItem != null
            , (execute) =>
            {
                ChangePasswordWindow changePwdCmd = new ChangePasswordWindow(SelectedItem.Id);
                changePwdCmd.ShowDialog();
            });
        }
    }
}
