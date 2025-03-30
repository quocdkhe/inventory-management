using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.ViewModel
{
    public class ReceiptDetailViewModel : BaseViewModel
    {
        private ObservableCollection<ReceiptDetail> _receiptDetails;
        private ObservableCollection<Models.Object> _objects;
        private ReceiptDetail _selectedItem;
        private Models.Object _selectedObject;
        private int? _quantity;
        private int? _price;
        private int _receiptId;
        private int _supplierId;
        private string _unit;

        public Boolean IsEditable { get; set; } = true;
        public ICommand RefreshCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CompleteReceipt { get; set; }
        public int? Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged(); }
        }
        public string Unit
        {
            get { return _unit; }
            set
            { _unit = value; OnPropertyChanged(); }
        }
        public int? Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged(); }
        }
        public Models.Object SelectedObject
        {
            get => _selectedObject;
            set
            {
                _selectedObject = value; OnPropertyChanged();
                if (SelectedObject != null)
                {
                    Unit = SelectedObject.Unit.DisplayName;
                }
            }
        }
        public int ReceiptId
        {
            get => _receiptId;
            set { _receiptId = value; OnPropertyChanged(); }
        }
        public int SupplierId
        {
            get => _supplierId;
            set { _supplierId = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ReceiptDetail> ReceiptDetails
        {
            get => _receiptDetails;
            set { _receiptDetails = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Models.Object> Objects
        {
            get => _objects;
            set { _objects = value; OnPropertyChanged(); }
        }

        public ReceiptDetail SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectedObject = SelectedItem.Object;
                    Quantity = SelectedItem.Quantity;
                    Unit = SelectedObject.Unit.DisplayName;
                    Price = SelectedItem.Price;
                }
            }
        }
        private void LoadFromDatabase()
        {
            ReceiptDetails = new ObservableCollection<ReceiptDetail>(InventoryManagementContext.INSTANCE.ReceiptDetails.Where(rd => rd.ReceiptId == ReceiptId));
            Objects = new
                ObservableCollection<Models.Object>
                (InventoryManagementContext.INSTANCE.Objects
                    .Include(obj => obj.Unit)  // Eagerly load the Unit entity
                    .Where(obj => obj.SupplierId == SupplierId));
        }

        public ReceiptDetailViewModel(int ReceiptId, int SupplierId, Boolean IsEditable)
        {
            this.ReceiptId = ReceiptId;
            this.SupplierId = SupplierId;
            this.IsEditable = IsEditable;
            LoadFromDatabase();

            AddCommand = new RelayCommand<object>(p => IsEditable, p =>
            {
                ReceiptDetail receiptDetail = new ReceiptDetail
                {
                    ReceiptId = ReceiptId,
                    ObjectId = SelectedObject.Id,
                    Quantity = Quantity,
                    Price = Price,
                };
                InventoryManagementContext.INSTANCE.ReceiptDetails.Add(receiptDetail);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            EditCommand = new RelayCommand<object>(p => SelectedItem != null && IsEditable, p =>
            {
                ReceiptDetail current = InventoryManagementContext.INSTANCE
                                            .ReceiptDetails.FirstOrDefault(rd => rd.Id == SelectedItem.Id);
                current.ReceiptId = ReceiptId;
                current.ObjectId = SelectedObject.Id;
                current.Quantity = Quantity;
                current.Price = Price;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            DeleteCommand = new RelayCommand<object>(p => SelectedItem != null && IsEditable, p =>
            {
                InventoryManagementContext.INSTANCE.ReceiptDetails.Remove(SelectedItem);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            CompleteReceipt = new RelayCommand<object>(p => IsEditable, p =>
            {
                MessageBoxResult result = MessageBox.Show(
                "Bạn muốn lưu thông tin nhập kho ?" +
                "\nLưu ý: Tất cả vật tư sẽ được nhập vào kho, và tính tổng thành tiền." +
                "\nBạn không thể chỉnh sửa lại phiếu nhập kho sau bước này.",
                "Xác nhận nhập kho",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);
                if (result == MessageBoxResult.No)
                {
                    return;
                }

                int? TotalPrice = ReceiptDetails.Sum(rd => rd.Quantity * rd.Price);

                IEnumerable<Stock>? newStock = from rd in ReceiptDetails
                                               where rd.ReceiptId == ReceiptId
                                               group rd by rd.ObjectId into g
                                               select new Stock
                                               {
                                                   ObjectId = g.Key,
                                                   Quantity = g.Sum(rd => rd.Quantity)
                                               };

                foreach (var stock in newStock)
                {
                    var existingStock = InventoryManagementContext.INSTANCE.Stocks
                            .FirstOrDefault(s => s.ObjectId == stock.ObjectId);
                    if (existingStock != null)
                    {
                        existingStock.Quantity += stock.Quantity;
                    }
                    else
                    {
                        InventoryManagementContext.INSTANCE.Stocks.Add(stock);
                    }
                }

                var currentReceipt = InventoryManagementContext.INSTANCE.Receipts.FirstOrDefault(r => r.Id == ReceiptId);
                currentReceipt.TotalPrice = TotalPrice;
                var currentCashFlow = InventoryManagementContext.INSTANCE.CashFlows.FirstOrDefault(cf => cf.Id == 1);
                currentCashFlow.TotalCosts += TotalPrice;
                IsEditable = false;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });
        }

    }
}
