using System.Collections.ObjectModel;
using System.Windows.Input;
using InventoryManagement.Models;
using Microsoft.Win32;

namespace InventoryManagement.ViewModel
{
    public class CustomerViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> _list;
        private Customer _selectedItem;
        private string _displayName;
        private string _phone;
        private string _address;
        private string _email;
        private string _moreInfo;
        private DateTime? _contractDate;
        public ObservableCollection<Customer> List
        {
            get => _list; set
            {
                _list = value; OnPropertyChanged();
            }
        }
        public string? DisplayName
        {
            get => _displayName; set { _displayName = value; OnPropertyChanged(); }
        }

        public string? Phone
        {
            get => _phone; set { _phone = value; OnPropertyChanged(); }
        }

        public string? Address
        {
            get => _address; set { _address = value; OnPropertyChanged(); }
        }

        public string? Email
        {
            get => _email; set { _email = value; OnPropertyChanged(); }
        }

        public string? MoreInfo
        {
            get => _moreInfo; set { _moreInfo = value; OnPropertyChanged(); }
        }

        public DateTime? ContractDate
        {
            get => _contractDate; set { _contractDate = value; OnPropertyChanged(); }
        }

        public Customer SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    Phone = SelectedItem.Phone;
                    Address = SelectedItem.Address;
                    Email = SelectedItem.Email;
                    ContractDate = SelectedItem.ContractDate;
                    MoreInfo = SelectedItem.MoreInfo;
                }
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ImportCommand { get; set; }

        private void LoadFromDatabase()
        {
            List = new ObservableCollection<Customer>(InventoryManagementContext.INSTANCE.Customers);
        }
        public CustomerViewModel()
        {
            LoadFromDatabase();
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (DisplayName == null)
                {
                    return false;
                }

                Boolean IsEmailExisted = InventoryManagementContext.INSTANCE.Customers.Any(c => c.Email == Email);
                Boolean IsPhoneExisted = InventoryManagementContext.INSTANCE.Customers.Any(c => c.Phone == Phone);

                if (IsEmailExisted || IsPhoneExisted)
                {
                    return false;
                }


                return true;

            }, (p) =>
            {
                Customer Customer = new Customer
                {
                    DisplayName = DisplayName,
                    Phone = Phone,
                    Address = Address,
                    Email = Email,
                    ContractDate = ContractDate,
                    MoreInfo = MoreInfo,
                };
                InventoryManagementContext.INSTANCE.Customers.Add(Customer);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }
                return true;
            }, (p) =>
            {
                var currentCustomer = InventoryManagementContext
                    .INSTANCE
                    .Customers
                    .FirstOrDefault(Customer => Customer.Id == SelectedItem.Id);
                currentCustomer.DisplayName = DisplayName;
                currentCustomer.Phone = Phone;
                currentCustomer.Address = Address;
                currentCustomer.Email = Email;
                currentCustomer.ContractDate = ContractDate;
                currentCustomer.MoreInfo = MoreInfo;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }
                Boolean customerActive = InventoryManagementContext.INSTANCE.Deliveries
                    .Any(d => d.CustomerId == SelectedItem.Id);
                if (customerActive)
                {
                    return false;
                }
                return true;

            }, (p) =>
            {
                var currentCustomer = InventoryManagementContext.INSTANCE
                    .Customers
                    .FirstOrDefault(Customer => Customer.Id == SelectedItem.Id);

                if (currentCustomer != null)
                {
                    InventoryManagementContext.INSTANCE.Customers.Remove(currentCustomer);
                    InventoryManagementContext.INSTANCE.SaveChanges();

                }
                LoadFromDatabase();
            });

            ImportCommand = new RelayCommand<object>(p => true, p =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    Title = "Nhập dữ liệu từ file JSON"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    Helpers.ImportFromJson.ImportJsonData<Customer>(
                        filePath,
                        (Customer customer) => InventoryManagementContext.INSTANCE.Customers.Add(customer),
                        () => InventoryManagementContext.INSTANCE.SaveChanges()
                    );
                }
                LoadFromDatabase();
            });
        }
    }
}
