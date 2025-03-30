using System.Collections.ObjectModel;
using System.Windows.Input;
using InventoryManagement.Models;

namespace InventoryManagement.ViewModel
{
    public class DeliveryViewModel : BaseViewModel
    {
        private ObservableCollection<Delivery> _deliveries;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<User> _users;
        private DateTime? _date;
        private Customer _selectedCustomer;
        private User _selectedUser;
        private int? _totalPrice;
        private Delivery _selectedItem;
        private DateTime? _dateStart;
        private DateTime? _dateEnd;
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddDeliveryCommand { get; set; }
        public ICommand CompleteDelivery { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ResetFilterCommand { get; set; }
        public User LoggedInUser { get; set; }
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set { _customers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Delivery> Deliveries
        {
            get => _deliveries;
            set { _deliveries = value; OnPropertyChanged(); }
        }

        public DateTime? Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        public DateTime? DateStart
        {
            get => _dateStart;
            set { _dateStart = value; OnPropertyChanged(); }
        }

        public DateTime? DateEnd
        {
            get => _dateEnd;
            set { _dateEnd = value; OnPropertyChanged(); }
        }

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        public int? TotalPrice
        {
            get => _totalPrice;
            set { _totalPrice = value; OnPropertyChanged(); }
        }

        public Delivery SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectedCustomer = SelectedItem.Customer;
                    SelectedUser = SelectedItem.User;
                    Date = SelectedItem.Date;
                    TotalPrice = SelectedItem.TotalPrice;
                }
            }
        }

        public void LoadFromDatabase()
        {
            Deliveries = new ObservableCollection<Delivery>(InventoryManagementContext.INSTANCE.Deliveries);
            Customers = new ObservableCollection<Customer>(InventoryManagementContext.INSTANCE.Customers);
            Users = new ObservableCollection<User>(InventoryManagementContext.INSTANCE.Users);
        }
        public DeliveryViewModel(User LoggedInUser)
        {
            this.LoggedInUser = LoggedInUser;
            LoadFromDatabase();

            AddCommand = new RelayCommand<object>(p => true, p =>
            {
                Delivery delivery = new Delivery
                {
                    CustomerId = SelectedCustomer.Id,
                    UserId = LoggedInUser.Id,
                    Date = Date,
                };
                InventoryManagementContext.INSTANCE.Deliveries.Add(delivery);
                InventoryManagementContext.INSTANCE.SaveChanges();

                // Show Receipt Window Automatically
                DeliveryDetailWindow deliveryDetailWindow = new DeliveryDetailWindow(delivery.Id, delivery.CustomerId, true);
                deliveryDetailWindow.ShowDialog();
                LoadFromDatabase();
            });

            DeleteCommand = new RelayCommand<object>(p =>
            {
                return SelectedItem != null && TotalPrice == null;
            }
            , p =>
            {
                InventoryManagementContext.INSTANCE.Deliveries.Remove(SelectedItem);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            EditCommand = new RelayCommand<object>(p =>
            {
                return SelectedItem != null && TotalPrice == null;
            }, p =>
            {
                Delivery delivery = InventoryManagementContext.INSTANCE.Deliveries.FirstOrDefault(r => r.Id == SelectedItem.Id);
                delivery.CustomerId = SelectedCustomer.Id;
                delivery.Date = Date;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            AddDeliveryCommand = new RelayCommand<object>(p => SelectedItem != null, p =>
            {
                Boolean IsEditable = TotalPrice is null;
                DeliveryDetailWindow deliveryDetailWindow = new DeliveryDetailWindow(SelectedItem.Id, SelectedCustomer.Id, IsEditable);
                deliveryDetailWindow.ShowDialog();
                LoadFromDatabase();
            });

            FilterCommand = new RelayCommand<object>(p => DateStart != null && DateEnd != null, p =>
            {
                Deliveries = new ObservableCollection<Delivery>(InventoryManagementContext.INSTANCE.Deliveries
                    .Where(r => r.Date >= DateStart && r.Date <= DateEnd));
            });

            ResetFilterCommand = new RelayCommand<object>(p => true, p =>
            {
                LoadFromDatabase();
                DateStart = null;
                DateEnd = null;
            });
        }

    }
}
