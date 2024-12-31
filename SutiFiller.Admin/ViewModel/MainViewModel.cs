using System.Collections.ObjectModel;
using System.Windows.Media;
using SutiFiller.Admin.Model;
using SutiFiller.Admin.Persistence;
using SutiFiller.Data;

namespace SutiFiller.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private IServiceModel _model;
        private ObservableCollection<SutiDTO> _sutis;
        private ObservableCollection<CategoryDTO> _categories;
        private ObservableCollection<StatusDTO> _statuses;
        private ObservableCollection<OrderDTO> _orders;
        private Boolean _isLoadedSuti;
        private Boolean _isLoadedOrder;
        private SutiDTO _selectedSuti;
        private OrderDTO _selectedOrder;
        #endregion

        #region Properties

        public ObservableCollection<SutiDTO> Sutis
        {
            get { return _sutis; }
            private set
            {
                if (_sutis != value)
                {
                    _sutis = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<CategoryDTO> Categories
        {
            get { return _categories; }
            private set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<StatusDTO> Statuses
        {
            get { return _statuses; }
            private set
            {
                if (_statuses != value)
                {
                    _statuses = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<OrderDTO> Orders
        {
            get { return _orders; }
            private set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged();
                }
            }
        }

        public SutiDTO SelectedSuti
        {
            get { return _selectedSuti; }
            set
            {
                if (_selectedSuti != value)
                {
                    _selectedSuti = value;
                    OnPropertyChanged();
                }
            }
        }

        public OrderDTO SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLoadedSuti
        {
            get { return _isLoadedSuti; }
            private set
            {
                if (_isLoadedSuti != value)
                {
                    _isLoadedSuti = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLoadedOrder
        {
            get { return _isLoadedOrder; }
            private set
            {
                if (_isLoadedOrder != value)
                {
                    _isLoadedOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand ListAllDataCommand { get; private set; }
        public DelegateCommand ListAllSutisCommand { get; private set; }
        public DelegateCommand ListAllOrdersCommand { get; private set; }
        public DelegateCommand FilterSutisByNameCommand { get; private set; }
        public SutiDTO EditedSuti { get; private set; }
        public OrderDTO EditedOrder { get; private set; }
        public DelegateCommand FilterOrdersCommand { get; private set; }
        public DelegateCommand FilterOrdersByNameCommand { get; private set; }
        public DelegateCommand FilterOrdersByDateCommand { get; private set; }
        public DelegateCommand FilterOrdersByStatusCommand { get; private set; }
        public DelegateCommand CreateSutiCommand { get; private set; }
        public DelegateCommand CreateOrderCommand { get; private set; }
        public DelegateCommand CreateImageCommand { get; private set; }
        public DelegateCommand UpdateSutiCommand { get; private set; }
        public DelegateCommand CheckOrderCommand { get; private set; }
        public DelegateCommand CheckMonthlyOrdersCommand { get; private set; }
        public DelegateCommand AddSutiToOrderCommand { get; private set; }
        public DelegateCommand RemoveSutiFromOrderCommand { get; private set; }
        public DelegateCommand UpdateOrderCommand { get; private set; }
        public DelegateCommand DeleteSutiCommand { get; private set; }
        public DelegateCommand DeleteOrderCommand { get; private set; }
        public DelegateCommand DeleteImageCommand { get; private set; }
        public DelegateCommand SaveSutiChangesCommand { get; private set; }
        public DelegateCommand CancelSutiChangesCommand { get; private set; }
        public DelegateCommand SaveOrderChangesCommand { get; private set; }
        public DelegateCommand CancelOrderChangesCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand HelpCommand { get; private set; }
        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        #endregion

        #region Events

        public event EventHandler ExitApplication;
        public event EventHandler OpenHelp;
        public event EventHandler SutiEditingStarted;
        public event EventHandler SutiEditingFinished;
        public event EventHandler SelectedOrderShown;
        public event EventHandler OrdersShownByMonths;
        public event EventHandler OrderEditingStarted;
        public event EventHandler OrderEditingFinished;
        public event EventHandler<SutiEventArgs> ImageEditingStarted;
        #endregion

        #region Constructor
        public MainViewModel(IServiceModel model)
        {
            _model = model;
            _isLoadedSuti = false;
            _isLoadedOrder = false;
            _model.SutiChanged += Model_SutiChanged;
            _model.OrderChanged += Model_OrderChanged;

            ListAllSutisCommand = new DelegateCommand(param => ListAllSutis());
            ListAllOrdersCommand = new DelegateCommand(param => ListAllOrders());
            FilterSutisByNameCommand = new DelegateCommand(param => FilterSutisByName(param as String));
            FilterOrdersCommand = new DelegateCommand(param => FilterOrders(param as String));
            FilterOrdersByNameCommand = new DelegateCommand(param => FilterOrdersByName(param as String));
            FilterOrdersByDateCommand = new DelegateCommand(param => FilterOrdersByDate(param as String));
            FilterOrdersByStatusCommand = new DelegateCommand(param => FilterOrdersByStatus(param as String));

            CreateSutiCommand = new DelegateCommand(param => { EditedSuti = new SutiDTO(); OnSutiEditingStarted(); });
            CreateOrderCommand = new DelegateCommand(param => { EditedOrder = new OrderDTO(); OnOrderEditingStarted(); });
            CreateImageCommand = new DelegateCommand(param => OnImageEditingStarted(param == null ? -1 : (param as SutiDTO).Id));
            UpdateSutiCommand = new DelegateCommand(param => UpdateSuti(param as SutiDTO));
            UpdateOrderCommand = new DelegateCommand(param => UpdateOrder(param as OrderDTO));
            CheckOrderCommand = new DelegateCommand(param => CheckOrder(param as OrderDTO));
            CheckMonthlyOrdersCommand = new DelegateCommand(param => CheckMonthlyOrders());
            AddSutiToOrderCommand = new DelegateCommand(param => AddSutiToOrder(param as OrderDTO));
            RemoveSutiFromOrderCommand = new DelegateCommand(param => RemoveSutiFromOrder(param as OrderDTO));
            DeleteSutiCommand = new DelegateCommand(param => DeleteSuti(param as SutiDTO));
            DeleteOrderCommand = new DelegateCommand(param => DeleteOrder(param as OrderDTO));
            DeleteImageCommand = new DelegateCommand(param => DeleteImage(param as ImageDTO));

            SaveSutiChangesCommand = new DelegateCommand(param => SaveSutiChanges());
            CancelSutiChangesCommand = new DelegateCommand(param => CancelSutiChanges());
            SaveOrderChangesCommand = new DelegateCommand(param => SaveOrderChanges());
            CancelOrderChangesCommand = new DelegateCommand(param => CancelOrderChanges());
            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            HelpCommand = new DelegateCommand(param => OnHelp());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
        }
        #endregion

        private void ListAllSutis()
        {
            Sutis = new ObservableCollection<SutiDTO>(_model.Sutis);
        }

        private void ListAllOrders()
        {
            Orders = new ObservableCollection<OrderDTO>(_model.Orders);
        }

        private void FilterOrders(String filter)
        {
            switch (filter)
            {
                case "All": Orders = new ObservableCollection<OrderDTO>(_model.Orders); break;
                case "Ordered": Orders = new ObservableCollection<OrderDTO>(_model.Orders.Where(o => o.Status.ToString() == "Ordered")); break;
                case "Done": Orders = new ObservableCollection<OrderDTO>(_model.Orders.Where(o => o.Status.ToString() == "Done")); break;
                case "Cancelled": Orders = new ObservableCollection<OrderDTO>(_model.Orders.Where(o => o.Status.ToString() == "Cancelled")); break;
                case "Failed": Orders = new ObservableCollection<OrderDTO>(_model.Orders.Where(o => o.Status.ToString() == "Failed")); break;
                case "NoStatus": Orders = new ObservableCollection<OrderDTO>(_model.Orders.Where(o => o.Status.ToString() == "NoStatus")); break;
            }
        }

        private void FilterSutisByName(String filter)
        {
            if (filter == null)
                return;

            Sutis = new ObservableCollection<SutiDTO>(_model.Sutis.Where(s => s.Name.Contains(filter)));
        }

        private void FilterOrdersByName(String searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                Orders = new ObservableCollection<OrderDTO>(Orders.Where(o => o.Name.Contains(searchString)));
            }
        }

        private void FilterOrdersByDate(String searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                Orders = new ObservableCollection<OrderDTO>(Orders.Where(o => o.DueDate.ToString().Contains(searchString)));
            }
        }

        private void FilterOrdersByStatus(String searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                Orders = new ObservableCollection<OrderDTO>(Orders.Where(o => o.Status.ToString().Contains(searchString)));
            }
        }

        private void UpdateSuti(SutiDTO suti)
        {
            if (suti == null)
                return;

            EditedSuti = new SutiDTO
            {
                Id = suti.Id,
                Name = suti.Name,
                Category = suti.Category,
                Price = suti.Price,
                Description = suti.Description
            };

            OnSutiEditingStarted();
        }

        private void CheckOrder(OrderDTO order)
        {
            if (SelectedOrderShown != null)
                SelectedOrderShown(this, EventArgs.Empty);
        }
        private void CheckMonthlyOrders() 
        {
            OrdersShownByMonths(this, EventArgs.Empty);
        }

        private void AddSutiToOrder(OrderDTO order)
        {
            if (order == null || SelectedSuti == null)
                return;

            _model.AddSutiToOrder(order, SelectedSuti);
        }

        private void RemoveSutiFromOrder(OrderDTO order)
        {
            if (order == null || SelectedSuti == null)
                return;

            _model.RemoveSutiFromOrder(order, SelectedSuti);
        }

        private void UpdateOrder(OrderDTO order)
        {
            if (order == null)
                return;

            EditedOrder = new OrderDTO
            {
                Id = order.Id,
                Name = order.Name,
                BillingAddress = order.BillingAddress,
                PrePayment = order.PrePayment,
                TotalPrice = order.TotalPrice,
                PhoneNumber = order.PhoneNumber,
                DueDate = order.DueDate,
                Status = order.Status,
                Comment = order.Comment,
            };

            OnOrderEditingStarted();
        }

        private void DeleteSuti(SutiDTO suti)
        {
            if (suti == null || !Sutis.Contains(suti))
                return;

            Sutis.Remove(suti);

            _model.DeleteSuti(suti);
        }

        private void DeleteOrder(OrderDTO order)
        {
            if (order == null || !Orders.Contains(order))
                return;

            Orders.Remove(order);

            _model.DeleteOrder(order);
        }

        private void DeleteImage(ImageDTO image)
        {
            if (image == null)
                return;

            _model.DeleteImage(image);
        }

        private void SaveSutiChanges()
        {
            if (String.IsNullOrEmpty(EditedSuti.Name))
            {
                OnMessageApplication("A sütinév nincs megadva!");
                return;
            }
            if (EditedSuti.Category == null)
            {
                OnMessageApplication("A kategória nincs megadva!");
                return;
            }
            switch (EditedSuti.Category.Name[0])
            {
                case 'S': EditedSuti.CategoryId = 1; break;
                case 'T': EditedSuti.CategoryId = 2; break;
                case 'K': EditedSuti.CategoryId = 3; break;
            }
            if (EditedSuti.Price == 0)
            {
                OnMessageApplication("Az ár nincs megadva!");
                return;
            }

            // mentés
            if (EditedSuti.Id == 0)
            {
                try
                {
                    _model.CreateSuti(EditedSuti);
                }
                catch (ArgumentException)
                {
                    OnMessageApplication("A süti már létezik!");
                    return;
                }
                Sutis.Add(EditedSuti);
                SelectedSuti = EditedSuti;
            }
            else
            {
                _model.UpdateSuti(EditedSuti);
            }

            EditedSuti = null;
            OnSutiEditingFinished();
        }

        private void SaveOrderChanges()
        {
            if (String.IsNullOrEmpty(EditedOrder.Name))
            {
                OnMessageApplication("A rendelés neve nincs megadva!");
                return;
            }
            if (String.IsNullOrEmpty(EditedOrder.PhoneNumber))
            {
                OnMessageApplication("A telefonszám nincs megadva!");
                return;
            }
            if (String.IsNullOrEmpty(EditedOrder.BillingAddress))
            {
                OnMessageApplication("A szállítási cím nincs megadva!");
                return;
            }
            if (String.IsNullOrEmpty(EditedOrder.DueDate.ToString()))
            {
                OnMessageApplication("A kiszállítás dátuma nincs megadva!");
                return;
            }
            if (EditedOrder.Status == null)
            {
                OnMessageApplication("A státusz nincs megadva!");
                return;
            }
            switch (EditedOrder.Status.Name[0])
            {
                case 'O': EditedOrder.StatusId = 1; break;
                case 'D': EditedOrder.StatusId = 2; break;
                case 'C': EditedOrder.StatusId = 3; break;
                case 'F': EditedOrder.StatusId = 4; break;
                case 'N': EditedOrder.StatusId = 5; break;
            }
            if (EditedOrder.PrePayment == 0)
            {
                OnMessageApplication("Az előleg nincs megadva!");
                return;
            }
            if (EditedOrder.TotalPrice == 0)
            {
                OnMessageApplication("Az összár nincs megadva!");
                return;
            }

            // mentés
            if (EditedOrder.Id == 0)
            {
                try
                {
                    _model.CreateOrder(EditedOrder);
                }
                catch (ArgumentException)
                {
                    OnMessageApplication("A rendelés már létezik!");
                    return;
                }
                Orders.Add(EditedOrder);
                SelectedOrder = EditedOrder;
            }
            else
            {
                _model.UpdateOrder(EditedOrder);
            }

            EditedOrder = null;
            OnOrderEditingFinished();
        }
        private void CancelSutiChanges()
        {
            EditedSuti = null;
            OnSutiEditingFinished();
        }

        private void CancelOrderChanges()
        {
            EditedOrder = null;
            OnOrderEditingFinished();
        }
        private async void SaveAsync()
        {
            try
            {
                await _model.SaveAsync();
                OnMessageApplication("A mentés sikeres!");
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A mentés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();
                Sutis = new ObservableCollection<SutiDTO>(_model.Sutis);
                Categories = new ObservableCollection<CategoryDTO>(_model.Categories);
                Statuses = new ObservableCollection<StatusDTO>(_model.Statuses);
                Orders = new ObservableCollection<OrderDTO>(_model.Orders);
                IsLoadedSuti = true;
                IsLoadedOrder = true;
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        private void Model_SutiChanged(object sender, SutiEventArgs e)
        {
            Int32 index = Sutis.IndexOf(Sutis.FirstOrDefault(suti => suti.Id == e.SutiId));
            Sutis.RemoveAt(index);
            Sutis.Insert(index, _model.Sutis[index]);

            SelectedSuti = Sutis[index];
        }

        private void Model_OrderChanged(object sender, OrderEventArgs e)
        {
            Int32 index = Orders.IndexOf(Orders.FirstOrDefault(order => order.Id == e.OrderId));
            Orders.RemoveAt(index);
            Orders.Insert(index, _model.Orders[index]);

            SelectedOrder = Orders[index];
        }

        private void OnSutiEditingStarted()
        {
            if (SutiEditingStarted != null)
                SutiEditingStarted(this, EventArgs.Empty);
        }

        private void OnSutiEditingFinished()
        {
            if (SutiEditingFinished != null)
                SutiEditingFinished(this, EventArgs.Empty);
        }

        private void OnOrderEditingStarted()
        {
            if (OrderEditingStarted != null)
                OrderEditingStarted(this, EventArgs.Empty);
        }

        private void OnOrderEditingFinished()
        {
            if (OrderEditingFinished != null)
                OrderEditingFinished(this, EventArgs.Empty);
        }

        private void OnImageEditingStarted(Int32 sutiId)
        {
            if (ImageEditingStarted != null)
                ImageEditingStarted(this, new SutiEventArgs { SutiId = sutiId });
        }
        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }

        private void OnHelp()
        {
            if (OpenHelp != null)
                OpenHelp(this, EventArgs.Empty);
        }
    }
}
