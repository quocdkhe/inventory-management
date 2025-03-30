using System.Collections.ObjectModel;
using System.Windows.Input;
using InventoryManagement.Models;

namespace InventoryManagement.ViewModel
{
    public class UnitViewModel : BaseViewModel
    {
        private ObservableCollection<Unit> _list;
        private Unit _selectedItem;
        private string _displayName;

        public ObservableCollection<Unit> List
        {
            get => _list; set
            {
                _list = value; OnPropertyChanged();
            }
        }

        public Unit SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                }
            }
        }
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value; OnPropertyChanged();
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        private void LoadFromDatabase()
        {
            List = new ObservableCollection<Unit>(InventoryManagementContext.INSTANCE.Units);
        }
        public UnitViewModel()
        {
            LoadFromDatabase();
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }

                bool isExisted = InventoryManagementContext.INSTANCE.Units
                    .Where(unit => unit.DisplayName == DisplayName).Count() != 0;
                return !isExisted;
            }, (p) =>
            {
                Unit unit = new Unit { DisplayName = DisplayName };
                InventoryManagementContext.INSTANCE.Units.Add(unit);
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || SelectedItem == null)
                {
                    return false;
                }
                bool isExisted = InventoryManagementContext.INSTANCE.Units
                    .Where(unit => unit.DisplayName == DisplayName).Count() != 0;
                return !isExisted;

            }, (p) =>
            {
                var currentUnit = InventoryManagementContext
                    .INSTANCE
                    .Units
                    .FirstOrDefault(unit => unit.Id == SelectedItem.Id);
                currentUnit.DisplayName = DisplayName;
                InventoryManagementContext.INSTANCE.SaveChanges();
                LoadFromDatabase();
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }
                // Check foreign key
                var obj = InventoryManagementContext.INSTANCE
                    .Objects
                    .FirstOrDefault(
                    obj => obj.UnitId == SelectedItem.Id
                );
                // Return false if found an object that is referencing to that object
                if (obj != null) return false;

                return true;

            }, (p) =>
            {
                var currentUnit = InventoryManagementContext.INSTANCE
                    .Units
                    .FirstOrDefault(unit => unit.Id == SelectedItem.Id);
                if (currentUnit != null)
                {
                    InventoryManagementContext.INSTANCE.Units.Remove(currentUnit);
                    InventoryManagementContext.INSTANCE.SaveChanges();
                }
                LoadFromDatabase();
            });
        }
    }
}
