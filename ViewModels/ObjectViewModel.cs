using System.Collections.ObjectModel;
using System.Windows.Input;
using InventoryManagement.Models;
using Microsoft.Win32;

namespace InventoryManagement.ViewModel
{
    public class ObjectViewModel : BaseViewModel
    {
        private ObservableCollection<Models.Object> _list;
        private ObservableCollection<Unit> _units;
        private ObservableCollection<Supplier> _suppliers;

        private Models.Object _selectedItem;
        private string _displayName;
        private string _qrCode;
        private string _barCode;
        private Supplier _selectedSupplier;
        private Unit _selectedUnit;
        public ObservableCollection<Models.Object> List
        {
            get => _list;
            set { _list = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set { _suppliers = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Unit> Units
        {
            get => _units;
            set { _units = value; OnPropertyChanged(); }
        }

        public Models.Object? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    QRCode = SelectedItem.Qrcode;
                    BarCode = SelectedItem.BarCode;
                    SelectedUnit = SelectedItem.Unit;
                    SelectedSupplier = SelectedItem.Supplier;
                }
            }
        }
        public string? DisplayName
        {
            get => _displayName;
            set { _displayName = value; OnPropertyChanged(); }
        }
        public string? QRCode
        {
            get => _qrCode;
            set { _qrCode = value; OnPropertyChanged(); }
        }
        public string? BarCode
        {
            get => _barCode;
            set { _barCode = value; OnPropertyChanged(); }
        }
        public Supplier? SelectedSupplier
        {
            get => _selectedSupplier;
            set { _selectedSupplier = value; OnPropertyChanged(); }
        }
        public Unit? SelectedUnit
        {
            get => _selectedUnit;
            set { _selectedUnit = value; OnPropertyChanged(); }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ImportCommand { get; set; }

        private void LoadObjects()
        {
            List = new ObservableCollection<Models.Object>(InventoryManagementContext.INSTANCE.Objects);
        }

        private void LoadSuppliers()
        {
            Suppliers = new ObservableCollection<Supplier>(InventoryManagementContext.INSTANCE.Suppliers);
        }

        private void LoadUnits()
        {
            Units = new ObservableCollection<Unit>(InventoryManagementContext.INSTANCE.Units);
        }

        public ObjectViewModel()
        {
            LoadObjects();
            LoadSuppliers();
            LoadUnits();

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedSupplier == null || SelectedUnit == null || string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                return true;
            }, (p) =>
            {
                Models.Object obj = new Models.Object
                {
                    DisplayName = DisplayName,
                    BarCode = BarCode,
                    Qrcode = QRCode,
                    SupplierId = SelectedSupplier.Id,
                    UnitId = SelectedUnit.Id,
                };
                InventoryManagementContext.INSTANCE.Objects.Add(obj);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadObjects();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || SelectedItem == null)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                var currentObject = InventoryManagementContext
                    .INSTANCE
                    .Objects
                    .FirstOrDefault(obj => obj.Id == SelectedItem.Id);
                currentObject.DisplayName = DisplayName;
                currentObject.BarCode = BarCode;
                currentObject.Qrcode = QRCode;
                currentObject.UnitId = SelectedUnit.Id;
                currentObject.SupplierId = SelectedSupplier.Id;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadObjects();
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }

                Boolean IsInStock = InventoryManagementContext.INSTANCE.Stocks
                    .Any(stock => stock.ObjectId == SelectedItem.Id);

                if (IsInStock)
                {
                    return false;
                }
                return true;

            }, (p) =>
            {
                var currentObject = InventoryManagementContext.INSTANCE
                    .Objects
                    .FirstOrDefault(obj => obj.Id == SelectedItem.Id);
                if (currentObject != null)
                {
                    InventoryManagementContext.INSTANCE.Objects.Remove(currentObject);
                    InventoryManagementContext.INSTANCE.SaveChanges();
                }
                LoadObjects();
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
                    Helpers.ImportFromJson.ImportJsonData<Models.Object>(
                        filePath,
                        (Models.Object obj) => InventoryManagementContext.INSTANCE.Objects.Add(obj),
                        () => InventoryManagementContext.INSTANCE.SaveChanges()
                    );
                }
                LoadObjects();
            });
        }
    }
}
