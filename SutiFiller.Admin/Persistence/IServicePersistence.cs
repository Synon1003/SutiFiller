using SutiFiller.Data;

namespace SutiFiller.Admin.Persistence
{
    public interface IServicePersistence
    {
        Task<IEnumerable<SutiDTO>> ReadSutisAsync();
        Task<IEnumerable<CategoryDTO>> ReadCategoriesAsync();
        Task<IEnumerable<StatusDTO>> ReadStatusesAsync();
        Task<IEnumerable<OrderDTO>> ReadOrdersAsync();
        Task<Boolean> CreateSutiAsync(SutiDTO suti);
        Task<Boolean> UpdateSutiAsync(SutiDTO suti);
        Task<Boolean> DeleteSutiAsync(SutiDTO suti);
        Task<Boolean> CreateOrderAsync(OrderDTO order);
        Task<Boolean> UpdateOrderAsync(OrderDTO order);
        Task<Boolean> DeleteOrderAsync(OrderDTO order);
        Task<Boolean> CreateSutiImageAsync(ImageDTO image);
        Task<Boolean> DeleteSutiImageAsync(ImageDTO image);
    }
}
