using SutiFiller.Data;
using System.Net.Http;
using System.Windows.Media;

namespace SutiFiller.Admin.Persistence
{
    class ServicePersistence : IServicePersistence
    {
        private HttpClient _client;

        public ServicePersistence(String baseAddress)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseAddress);
        }

        public async Task<IEnumerable<SutiDTO>> ReadSutisAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/sutis");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<SutiDTO> sutis = await response.Content.ReadAsAsync<IEnumerable<SutiDTO>>();

                    foreach (SutiDTO suti in sutis)
                    {
                        response = await _client.GetAsync("api/images/" + suti.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            suti.Images = (await response.Content.ReadAsAsync<IEnumerable<ImageDTO>>()).ToList();
                        }
                    }

                    return sutis;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<CategoryDTO>> ReadCategoriesAsync() {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/categories");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<CategoryDTO> categories = await response.Content.ReadAsAsync<IEnumerable<CategoryDTO>>();
                    return categories;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<StatusDTO>> ReadStatusesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/statuses");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<StatusDTO> statuses = await response.Content.ReadAsAsync<IEnumerable<StatusDTO>>();
                    return statuses;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<OrderDTO>> ReadOrdersAsync() {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/orders");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<OrderDTO> orders = await response.Content.ReadAsAsync<IEnumerable<OrderDTO>>();

                    foreach (OrderDTO order in orders)
                    {
                        response = await _client.GetAsync("api/orders/" + order.Id + "/sutiorders");
                        if (response.IsSuccessStatusCode)
                        {
                            order.SutiOrders = (await response.Content.ReadAsAsync<IEnumerable<SutiOrderDTO>>()).ToList();
                        }
                    }

                    return orders;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> CreateSutiAsync(SutiDTO suti) {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/sutis", suti);
                suti.Id = (await response.Content.ReadAsAsync<SutiDTO>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> UpdateSutiAsync(SutiDTO suti) {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/sutis", suti);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> CreateOrderAsync(OrderDTO order) 
        {
            List<SutiOrderDTO> sutiOrderDTOsToCreate = new List<SutiOrderDTO>();
            foreach (SutiOrderDTO so in order.SutiOrders)
            {
                sutiOrderDTOsToCreate.Add(new SutiOrderDTO
                {
                    SutiId = so.SutiId,
                    OrderId = so.OrderId,
                    Quantity = so.Quantity,
                    AllInPrice = so.AllInPrice,
                    Message = so.Message,
                });
            }

            var orderToCreate = new OrderDTO
            { 
                Name = order.Name,
                BillingAddress = order.BillingAddress,
                PhoneNumber = order.PhoneNumber,
                DueDate = order.DueDate,
                StatusId = order.StatusId,
                Comment = order.Comment,
                PrePayment = order.PrePayment,
                TotalPrice = order.TotalPrice,
                SutiOrders = sutiOrderDTOsToCreate,
            };

            try
            {
                HttpResponseMessage responseOrder = await _client.PostAsJsonAsync("api/orders", orderToCreate);
                order.Id = (await responseOrder.Content.ReadAsAsync<OrderDTO>()).Id;

                return responseOrder.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> UpdateOrderAsync(OrderDTO order)
        {
            List<SutiOrderDTO> sutiOrderDTOsToUpdate = new List<SutiOrderDTO>();
            foreach (SutiOrderDTO so in order.SutiOrders)
            {
                sutiOrderDTOsToUpdate.Add(new SutiOrderDTO
                {
                    SutiId = so.SutiId,
                    OrderId = so.OrderId,
                    Quantity = so.Quantity,
                    AllInPrice = so.AllInPrice,
                    Message = so.Message,
                });
            }

            var orderToUpdate = new OrderDTO
            {
                Id = order.Id,
                Name = order.Name,
                BillingAddress = order.BillingAddress,
                PhoneNumber = order.PhoneNumber,
                DueDate = order.DueDate,
                StatusId = order.StatusId,
                Comment = order.Comment,
                PrePayment = order.PrePayment,
                TotalPrice = order.TotalPrice,
                SutiOrders = sutiOrderDTOsToUpdate,
            };

            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/orders", orderToUpdate);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> DeleteOrderAsync(OrderDTO order)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/orders/" + order.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> DeleteSutiAsync(SutiDTO suti) 
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/sutis/" + suti.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> CreateSutiImageAsync(ImageDTO image)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/images", image);
                if (response.IsSuccessStatusCode)
                {
                    image.Id = await response.Content.ReadAsAsync<Int32>();
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<Boolean> DeleteSutiImageAsync(ImageDTO image) {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/images/" + image.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
    }
}
