using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.ViewModel
{
    public class DeliveryDetailViewModel : BaseViewModel
    {
        private ObservableCollection<DeliveryDetail> _deliveryDetails;
        private ObservableCollection<Models.Object> _objects;
        private DeliveryDetail _selectedItem;
        private Models.Object _selectedObject;
        private int _quantity;
        private int _price;
        private int _deliveryId;
        private int _customerId;
        private string _unit;

        public Boolean IsEditable { get; set; } = true;
        public ICommand RefreshCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CompleteDelivery { get; set; }
        public int Quantity
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
        public int Price
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
        public int DeliveryId
        {
            get => _deliveryId;
            set { _deliveryId = value; OnPropertyChanged(); }
        }
        public int CustomerId
        {
            get => _customerId;
            set { _customerId = value; OnPropertyChanged(); }
        }
        public ObservableCollection<DeliveryDetail> DeliveryDetails
        {
            get => _deliveryDetails;
            set { _deliveryDetails = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Models.Object> Objects
        {
            get => _objects;
            set { _objects = value; OnPropertyChanged(); }
        }

        public DeliveryDetail SelectedItem
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
            DeliveryDetails = new ObservableCollection<DeliveryDetail>(InventoryManagementContext.INSTANCE.DeliveryDetails.Where(rd => rd.DeliveryId == DeliveryId));
            Objects = new ObservableCollection<Models.Object>(InventoryManagementContext.INSTANCE.Objects
                .Where(obj => InventoryManagementContext.INSTANCE.Stocks.Any(stock => stock.ObjectId == obj.Id))
                .Include(obj => obj.Unit));
        }

        public DeliveryDetailViewModel(int DeliveryId, int CustomerId, Boolean IsEditable)
        {
            this.DeliveryId = DeliveryId;
            this.CustomerId = CustomerId;
            this.IsEditable = IsEditable;
            LoadFromDatabase();

            AddCommand = new RelayCommand<object>(p => IsEditable, p =>
            {
                DeliveryDetail deliveryDetail = new DeliveryDetail
                {
                    DeliveryId = DeliveryId,
                    ObjectId = SelectedObject.Id,
                    Quantity = Quantity,
                    Price = Price,
                };
                InventoryManagementContext.INSTANCE.DeliveryDetails.Add(deliveryDetail);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            EditCommand = new RelayCommand<object>(p => SelectedItem != null && IsEditable, p =>
            {
                DeliveryDetail? current = InventoryManagementContext.INSTANCE
                                            .DeliveryDetails.FirstOrDefault(rd => rd.Id == SelectedItem.Id);
                current.DeliveryId = DeliveryId;
                current.ObjectId = SelectedObject.Id;
                current.Quantity = Quantity;
                current.Price = Price;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            DeleteCommand = new RelayCommand<object>(p => SelectedItem != null && IsEditable, p =>
            {
                InventoryManagementContext.INSTANCE.DeliveryDetails.Remove(SelectedItem);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            CompleteDelivery = new RelayCommand<object>(p => IsEditable, p =>
            {
                MessageBoxResult result = MessageBox.Show(
                "Bạn muốn lưu thông tin xuất kho ?" +
                "\nLưu ý: Tất cả vật tư sẽ được xuất cho khách hàng và tính tổng thành tiền." +
                "\nBạn không thể chỉnh sửa lại phiếu xuất kho sau bước này.",
                "Xác nhận xuất kho",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);
                if (result == MessageBoxResult.No)
                {
                    return;
                }

                int? TotalPrice = DeliveryDetails.Sum(rd => rd.Quantity * rd.Price);
                IEnumerable<Stock>? deliveryStock = from rd in DeliveryDetails
                                                    where rd.DeliveryId == DeliveryId
                                                    group rd by rd.ObjectId into g
                                                    select new Stock
                                                    {
                                                        ObjectId = g.Key,
                                                        Quantity = g.Sum(rd => rd.Quantity)
                                                    };

                foreach (var stock in deliveryStock)
                {
                    var currentStock = InventoryManagementContext.INSTANCE.Stocks
                            .FirstOrDefault(s => s.ObjectId == stock.ObjectId);

                    if (stock.Quantity > currentStock.Quantity)
                    {
                        MessageBox.Show("Lỗi: Số lượng xuất phải nhỏ hơn hoặc bằng số lượng kho hiện tại",
                            "Error", MessageBoxButton.OKCancel, MessageBoxImage.Stop);
                        return;
                    }
                }

                foreach (var stock in deliveryStock)
                {
                    var currentStock = InventoryManagementContext.INSTANCE.Stocks
                            .FirstOrDefault(s => s.ObjectId == stock.ObjectId);
                    currentStock.Quantity -= stock.Quantity;
                }

                var currentDelivery = InventoryManagementContext.INSTANCE.Deliveries.FirstOrDefault(r => r.Id == DeliveryId);
                currentDelivery.TotalPrice = TotalPrice;
                var currentCashFlow = InventoryManagementContext.INSTANCE.CashFlows.FirstOrDefault(r => r.Id == 1);
                currentCashFlow.TotalIncome += TotalPrice;
                IsEditable = false;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });
        }

    }
}
