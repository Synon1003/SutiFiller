using SutiFiller.Data;

namespace SutiFiller.Admin.Model
{
    public interface IServiceModel
    {
        IReadOnlyList<SutiDTO> Sutis { get; }
        IReadOnlyList<CategoryDTO> Categories { get; }
        IReadOnlyList<StatusDTO> Statuses { get; }
        IReadOnlyList<OrderDTO> Orders { get; }

        event EventHandler<SutiEventArgs> SutiChanged;
        event EventHandler<OrderEventArgs> OrderChanged;

        void CreateSuti(SutiDTO suti);
        void UpdateSuti(SutiDTO suti);
        void CreateOrder(OrderDTO order);
        void UpdateOrder(OrderDTO order);
        void AddSutiToOrder(OrderDTO order, SutiDTO suti);
        void RemoveSutiFromOrder(OrderDTO order, SutiDTO suti);
        void DeleteSuti(SutiDTO suti);
        void DeleteOrder(OrderDTO order);
        void CreateImage(Int32 sutiId, Byte[] imageSmall, Byte[] imageLarge);
        void DeleteImage(ImageDTO image);


        Task SaveAsync();
        Task LoadAsync();
    }
}
