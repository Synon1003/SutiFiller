using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SutiFiller.Admin.Persistence;
using SutiFiller.Data;

namespace SutiFiller.Admin.Model
{
    class ServiceModel : IServiceModel
    {
        #region Fields
        private enum DataFlag
        {
            Create,
            Update,
            Delete
        }

        private IServicePersistence _persistence;
        private List<SutiDTO> _sutis;
        private List<OrderDTO> _orders;
        private List<CategoryDTO> _categories;
        private List<StatusDTO> _statuses;
        private Dictionary<SutiDTO, DataFlag> _sutiFlags;
        private Dictionary<SutiOrderDTO, DataFlag> _sutiOrderFlags;
        private Dictionary<OrderDTO, DataFlag> _orderFlags;
        private Dictionary<ImageDTO, DataFlag> _imageFlags;

        #endregion

        #region Properties

        public IReadOnlyList<SutiDTO> Sutis { get { return _sutis; } }
        public IReadOnlyList<OrderDTO> Orders { get { return _orders; } }
        public IReadOnlyList<CategoryDTO> Categories { get { return _categories; } }
        public IReadOnlyList<StatusDTO> Statuses { get { return _statuses; } }
        #endregion

        #region Events

        public event EventHandler<SutiEventArgs> SutiChanged;
        public event EventHandler<OrderEventArgs> OrderChanged;

        #endregion

        #region Constructor
        public ServiceModel(IServicePersistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));

            _persistence = persistence;
        }
        #endregion

        #region Public Create Methods
        public void CreateSuti(SutiDTO suti)
        {
            if (suti == null)
                throw new ArgumentNullException(nameof(suti));

            if (_sutis.Contains(suti))
                throw new ArgumentException("The suti is already in the collection.", nameof(suti));

            suti.Id = (_sutis.Count > 0 ? _sutis.Max(s => s.Id) : 0) + 1;
            _sutiFlags.Add(suti, DataFlag.Create);
            _sutis.Add(suti);
        }

        public void CreateImage(Int32 sutiId, Byte[] imageSmall, Byte[] imageLarge)
        {
            SutiDTO suti = _sutis.FirstOrDefault(b => b.Id == sutiId);
            if (suti == null)
                throw new ArgumentException("The suti does not exist.", nameof(sutiId));

            ImageDTO image = new ImageDTO()
            {
                Id = _sutis.Max(s => s.Images.Any() ? s.Images.Max(im => im.Id) : 0) + 1,
                SutiId = sutiId,
                ImageSmall = imageSmall,
                ImageLarge = imageLarge
            };

            suti.Images.Add(image);
            _imageFlags.Add(image, DataFlag.Create);

            OnSutiChanged(suti.Id);
        }

        public void CreateOrder(OrderDTO order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (_orders.Contains(order))
                throw new ArgumentException("The order is already in the collection.", nameof(order));

            order.Id = (_orders.Count > 0 ? _orders.Max(s => s.Id) : 0) + 1;
            _orderFlags.Add(order, DataFlag.Create);
            _orders.Add(order);
        }

        #endregion

        #region Public Update Methods

        public void UpdateSuti(SutiDTO suti)
        {
            if (suti == null)
                throw new ArgumentNullException(nameof(suti));

            SutiDTO sutiToModify = _sutis.FirstOrDefault(s => s.Id == suti.Id);

            if (sutiToModify == null)
                throw new ArgumentException("The suti does not exist.", nameof(suti));

            sutiToModify.Category = suti.Category;
            sutiToModify.Name = suti.Name;
            sutiToModify.Price = suti.Price;
            sutiToModify.Description = suti.Description;

            switch (suti.Category.Name[0])
            {
                case 'S': sutiToModify.CategoryId = 1; break;
                case 'T': sutiToModify.CategoryId = 3; break;
                case 'K': sutiToModify.CategoryId = 2; break;
            }

            // külön állapottal jelezzük, ha egy adat újonnan hozzávett
            if (_sutiFlags.ContainsKey(sutiToModify) && _sutiFlags[sutiToModify] == DataFlag.Create)
            {
                _sutiFlags[sutiToModify] = DataFlag.Create;
            }
            else
            {
                _sutiFlags[sutiToModify] = DataFlag.Update;
            }

            OnSutiChanged(suti.Id);
        }

        public void UpdateOrder(OrderDTO order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            OrderDTO orderToModify = _orders.FirstOrDefault(o => o.Id == order.Id);

            if (orderToModify == null)
                throw new ArgumentException("The order does not exist.", nameof(order));

            orderToModify.Status = order.Status;
            orderToModify.Name = order.Name;
            orderToModify.PhoneNumber = order.PhoneNumber;
            orderToModify.PrePayment = order.PrePayment;
            orderToModify.TotalPrice = order.TotalPrice;
            orderToModify.BillingAddress = order.BillingAddress;
            orderToModify.DueDate = order.DueDate;
            orderToModify.Comment = order.Comment;

            switch (order.Status.Name[0])
            {
                case 'O': orderToModify.StatusId = 1; break;
                case 'D': orderToModify.StatusId = 2; break;
                case 'C': orderToModify.StatusId = 3; break;
                case 'F': orderToModify.StatusId = 4; break;
                case 'N': orderToModify.StatusId = 5; break;
            }

            if (_orderFlags.ContainsKey(orderToModify) && _orderFlags[orderToModify] == DataFlag.Create)
            {
                _orderFlags[orderToModify] = DataFlag.Create;
            }
            else
            {
                _orderFlags[orderToModify] = DataFlag.Update;
            }

            OnOrderChanged(order.Id);
        }

        public void AddSutiToOrder(OrderDTO order, SutiDTO suti) {
            var sutiOrder = order.SutiOrders.FirstOrDefault(so => so.SutiId == suti.Id);

            if (sutiOrder == null)
                order.SutiOrders.Add(new SutiOrderDTO
                {
                    Id = (order.SutiOrders.Any() ? order.SutiOrders.Max(so => so.Id) : 0) + 1,
                    Quantity = 1,
                    AllInPrice = suti.Price,
                    SutiId = suti.Id,
                    Suti = new SutiDTO { Id = suti.Id, Name = suti.Name, Price = suti.Price,
                        Category = new CategoryDTO { Id = suti.CategoryId, Name = suti.Category.Name }
                    },
                    OrderId = order.Id,
                    Message = "",
                });
            else 
            {
                sutiOrder.Quantity += 1;
                sutiOrder.AllInPrice += suti.Price;
            }

            if (_orderFlags.ContainsKey(order) && _orderFlags[order] == DataFlag.Create)
            {
                _orderFlags[order] = DataFlag.Create;
            }
            else
            {
                _orderFlags[order] = DataFlag.Update;
            }

            OnOrderChanged(order.Id);
        }

        public void RemoveSutiFromOrder(OrderDTO order, SutiDTO suti)
        {
            var sutiOrder = order.SutiOrders.FirstOrDefault(so => so.SutiId == suti.Id);
            if (sutiOrder == null) return;

            if (sutiOrder.Quantity == 1)
                order.SutiOrders.Remove(sutiOrder);
            else 
            {
                sutiOrder.Quantity -= 1;
                sutiOrder.AllInPrice -= suti.Price;
            }

            if (_orderFlags.ContainsKey(order) && _orderFlags[order] == DataFlag.Create)
            {
                _orderFlags[order] = DataFlag.Create;
            }
            else
            {
                _orderFlags[order] = DataFlag.Update;
            }

            OnOrderChanged(order.Id);
        }

        #endregion

        #region Public Delete Methods

        public void DeleteSuti(SutiDTO suti)
        {
            if (suti == null)
                throw new ArgumentNullException(nameof(suti));

            SutiDTO sutiToDelete = _sutis.FirstOrDefault(s => s.Id == suti.Id);

            if (sutiToDelete == null)
                throw new ArgumentException("The suti does not exist.", nameof(suti));

            if (_sutiFlags.ContainsKey(sutiToDelete) && _sutiFlags[sutiToDelete] == DataFlag.Create)
                _sutiFlags.Remove(sutiToDelete);
            else
                _sutiFlags[sutiToDelete] = DataFlag.Delete;
        }

        public void DeleteOrder(OrderDTO order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            OrderDTO orderToDelete = _orders.FirstOrDefault(o => o.Id == order.Id);

            if (orderToDelete == null)
                throw new ArgumentException("The order does not exist.", nameof(order));

            if (_orderFlags.ContainsKey(orderToDelete) && _orderFlags[orderToDelete] == DataFlag.Create)
                _orderFlags.Remove(orderToDelete);
            else
                _orderFlags[orderToDelete] = DataFlag.Delete;
        }

        public void DeleteImage(ImageDTO image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            foreach (SutiDTO suti in _sutis)
            {
                if (!suti.Images.Contains(image))
                    continue;

                if (_imageFlags.ContainsKey(image))
                    _imageFlags.Remove(image);
                else
                    _imageFlags.Add(image, DataFlag.Delete);

                suti.Images.Remove(image);

                OnSutiChanged(suti.Id);

                return;
            }

            throw new ArgumentException("The image does not exist.", nameof(image));
        }

        #endregion

        #region Save/Load
        public async Task SaveAsync()
        {
            List<SutiDTO> sutisToSave = _sutiFlags.Keys.ToList();
            List<OrderDTO> ordersToSave = _orderFlags.Keys.ToList();

            foreach (SutiDTO suti in sutisToSave)
            {
                Boolean result = true;

                switch (_sutiFlags[suti])
                {
                    case DataFlag.Create:
                        result = await _persistence.CreateSutiAsync(suti);
                        break;
                    case DataFlag.Delete:
                        result = await _persistence.DeleteSutiAsync(suti);
                        break;
                    case DataFlag.Update:
                        result = await _persistence.UpdateSutiAsync(suti);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _sutiFlags[suti] + " failed on suti " + suti.Id);

                _sutiFlags.Remove(suti);
            }

            foreach (OrderDTO order in ordersToSave)
            {
                Boolean result = true;

                switch (_orderFlags[order])
                {
                    case DataFlag.Create:

                        result = await _persistence.CreateOrderAsync(order);
                        break;
                    case DataFlag.Delete:
                        result = await _persistence.DeleteOrderAsync(order);
                        break;
                    case DataFlag.Update:
                        result = await _persistence.UpdateOrderAsync(order);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _orderFlags[order] + " failed on order " + order.Id);

                _orderFlags.Remove(order);
            }

            List<ImageDTO> imagesToSave = _imageFlags.Keys.ToList();

            foreach (ImageDTO image in imagesToSave)
            {
                Boolean result = true;

                switch (_imageFlags[image])
                {
                    case DataFlag.Create:
                        result = await _persistence.CreateSutiImageAsync(image);
                        break;
                    case DataFlag.Delete:
                        result = await _persistence.DeleteSutiImageAsync(image);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _imageFlags[image] + " failed on image " + image.Id);

                _imageFlags.Remove(image);
            }
        }

        public async Task LoadAsync()
        {
            _sutis = (await _persistence.ReadSutisAsync()).ToList();
            _categories = (await _persistence.ReadCategoriesAsync()).ToList();
            _statuses = (await _persistence.ReadStatusesAsync()).ToList();
            _orders = (await _persistence.ReadOrdersAsync()).ToList();

            _sutiFlags = new Dictionary<SutiDTO, DataFlag>();
            _imageFlags = new Dictionary<ImageDTO, DataFlag>();
            _orderFlags = new Dictionary<OrderDTO, DataFlag>();
        }

        #endregion

        #region EventHandlers

        private void OnSutiChanged(Int32 sutiId)
        {
            if (SutiChanged != null)
                SutiChanged(this, new SutiEventArgs { SutiId = sutiId });
        }

        private void OnOrderChanged(Int32 orderId)
        {
            if (OrderChanged != null)
                OrderChanged(this, new OrderEventArgs { OrderId = orderId });
        }

        #endregion

    }
}
