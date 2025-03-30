using System.Windows;
using System.Windows.Input;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private int? _totalCosts;
        private int? _totalIncome;
        private int? _accountNumber;
        private List<Stock> _stock;
        public Window MainWindow { get; private set; }
        public bool IsLoaded { get; set; } = false;
        public ICommand UnitCommand { get; set; }
        public ICommand SupplierComand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ObjectCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand ReceiptCommand { get; set; }
        public ICommand DeliveryCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public User LoggedInUser { get; set; }

        public int? TotalCosts
        {
            get => _totalCosts; set { _totalCosts = value; OnPropertyChanged(); }
        }

        public int? TotalIncome
        {
            get => _totalIncome; set { _totalIncome = value; OnPropertyChanged(); }
        }

        public int? AccountNumber
        {
            get => _accountNumber; set { _accountNumber = value; OnPropertyChanged(); }
        }

        public List<Stock> Stock
        {
            get => _stock;
            set { _stock = value; OnPropertyChanged(); }
        }

        public void HandleLogin(Window p)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            // Process: the login is succeeded or not
            var loginVM = loginWindow.DataContext as LoginViewModel;
            if (loginVM == null)
            {
                return;
            }
            // If is not logged in, close main window
            if (!loginVM.IsLogin)
            {
                p.Close();
            }
            // If logged in 
            LoggedInUser = loginVM.LoggedInUser;
        }

        public Boolean IsAuthorized()
        {
            if (LoggedInUser == null)
            {
                return false;
            }
            return LoggedInUser.RoleId == 1;
        }

        public void LoadFromDatabase()
        {
            Stock = InventoryManagementContext.INSTANCE.Stocks.
                Include(s => s.Object.Unit).
                ToList();
            AccountNumber = InventoryManagementContext.INSTANCE.Users.Count();
            TotalCosts = InventoryManagementContext.INSTANCE.CashFlows.FirstOrDefault(x => x.Id == 1).TotalCosts;
            TotalIncome = InventoryManagementContext.INSTANCE.CashFlows.FirstOrDefault(x => x.Id == 1).TotalIncome;
        }


        public MainViewModel()
        {
            MainWindow = Application.Current.MainWindow;

            if (!IsLoaded)
            {
                IsLoaded = true;
                HandleLogin(MainWindow);
                LoadFromDatabase();
            }

            LogoutCommand = new RelayCommand<Window>((p) => true, p =>
            {
                MessageBoxResult result = MessageBox.Show(
                "Bạn có muốn đăng xuất ?",
                "Đăng xuất",
                MessageBoxButton.YesNo,   // Show Yes and No buttons
                MessageBoxImage.Question  // Show a question mark icon
                );

                // Check the user's choice
                if (result == MessageBoxResult.Yes)
                {
                    LoggedInUser = null;
                    p.Hide();
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();
                    var loginVM = loginWindow.DataContext as LoginViewModel;
                    if (loginVM == null || !loginVM.IsLogin)
                    {
                        p.Close();
                    }
                    else
                    {
                        LoggedInUser = loginVM.LoggedInUser;
                        p.Show();
                        loginWindow.Close();
                    }
                }
            });

            UnitCommand = new RelayCommand<object>((p) => IsAuthorized(), (p) =>
            {
                UnitWindow unitWindow = new UnitWindow();
                unitWindow.ShowDialog();
            });

            SupplierComand = new RelayCommand<object>((p) => IsAuthorized(), (p) =>
            {
                SupplierWindow supplierWindow = new SupplierWindow();
                supplierWindow.ShowDialog();
            });

            CustomerCommand = new RelayCommand<object>((p) => IsAuthorized(), (p) =>
            {
                CustomerWindow customerWindow = new CustomerWindow();
                customerWindow.ShowDialog();
            });

            ObjectCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                ObjectWindow objectWindow = new ObjectWindow();
                objectWindow.ShowDialog();
            });

            UserCommand = new RelayCommand<object>((p) => IsAuthorized(), (p) =>
            {
                UserWindow userWindow = new UserWindow();
                userWindow.ShowDialog();
            });

            ReceiptCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                ReceiptWindow receiptWindow = new ReceiptWindow(LoggedInUser);
                receiptWindow.ShowDialog();
                LoadFromDatabase();
            });

            DeliveryCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                DeliveryWindow deliveryWindow = new DeliveryWindow(LoggedInUser);
                deliveryWindow.ShowDialog();
                LoadFromDatabase();
            });
        }
    }
}
