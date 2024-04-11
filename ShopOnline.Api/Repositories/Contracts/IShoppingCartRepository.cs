using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<Cartitem> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<Cartitem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
        Task<Cartitem> DeleteItem(int id);
        Task<Cartitem> GetItem(int id);
        Task<IEnumerable<Cartitem>> GetItems(int userId);
    }
}
