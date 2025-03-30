using System.Collections.ObjectModel;
using System.Windows.Input;
using InventoryManagement.Models;

namespace InventoryManagement.ViewModel
{
    public class ReceiptViewModel : BaseViewModel
    {
        private ObservableCollection<Receipt> receipts;
        private ObservableCollection<Supplier> _suppliers;
        private ObservableCollection<User> _users;
        private DateTime? _date;
        private Supplier _selectedSupplier;
        private User _selectedUser;
        private int? _totalPrice;
        private Receipt _selectedItem;
        private DateTime? _dateStart;
        private DateTime? _dateEnd;
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddReceiptDetail { get; set; }
        public ICommand CompleteReceipt { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ResetFilterCommand { get; set; }
        public User LoggedInUser { get; set; }

        public DateTime? DateStart { get => _dateStart; set { _dateStart = value; OnPropertyChanged(); } }
        public DateTime? DateEnd { get => _dateEnd; set { _dateEnd = value; OnPropertyChanged(); } }
        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set { _suppliers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Receipt> Receipts
        {
            get => receipts;
            set { receipts = value; OnPropertyChanged(); }
        }

        public DateTime? Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }
        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set { _selectedSupplier = value; OnPropertyChanged(); }
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

        public Receipt SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectedSupplier = SelectedItem.Supplier;
                    SelectedUser = SelectedItem.User;
                    Date = SelectedItem.Date;
                    TotalPrice = SelectedItem.TotalPrice;
                }
            }
        }

        public void LoadFromDatabase()
        {
            Receipts = new ObservableCollection<Receipt>(InventoryManagementContext.INSTANCE.Receipts);
            Suppliers = new ObservableCollection<Supplier>(InventoryManagementContext.INSTANCE.Suppliers);
            Users = new ObservableCollection<User>(InventoryManagementContext.INSTANCE.Users);
        }
        public ReceiptViewModel()
        {
            LoadFromDatabase();

            AddCommand = new RelayCommand<object>(p =>
            {
                return SelectedSupplier != null && Date != null;
            }, p =>
            {
                Receipt receipt = new Receipt
                {
                    SupplierId = SelectedSupplier.Id,
                    UserId = LoggedInUser.Id,
                    Date = Date,
                };
                InventoryManagementContext.INSTANCE.Receipts.Add(receipt);
                InventoryManagementContext.INSTANCE.SaveChanges();

                // Show Receipt Window Automatically
                ReceiptDetailWindow receiptDetailWindow = new ReceiptDetailWindow(receipt.Id, receipt.SupplierId, true);
                receiptDetailWindow.ShowDialog();
                LoadFromDatabase();
            });

            DeleteCommand = new RelayCommand<object>(p =>
            {
                return SelectedItem != null && TotalPrice == null;
            }
            , p =>
            {
                InventoryManagementContext.INSTANCE.Receipts.Remove(SelectedItem);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            EditCommand = new RelayCommand<object>(p =>
            {
                return SelectedItem != null && TotalPrice == null;
            }, p =>
            {
                Receipt receipt = InventoryManagementContext.INSTANCE.Receipts.FirstOrDefault(r => r.Id == SelectedItem.Id);
                receipt.SupplierId = SelectedSupplier.Id;
                receipt.Date = Date;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            AddReceiptDetail = new RelayCommand<object>(p => SelectedItem != null, p =>
            {
                Boolean IsEditable = TotalPrice == null;
                ReceiptDetailWindow receiptDetailWindow = new ReceiptDetailWindow(SelectedItem.Id, SelectedSupplier.Id, IsEditable);
                receiptDetailWindow.ShowDialog();
                LoadFromDatabase();
            });

            FilterCommand = new RelayCommand<object>(p => DateStart != null && DateEnd != null, p =>
            {
                Receipts = new ObservableCollection<Receipt>(InventoryManagementContext.INSTANCE.Receipts
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
