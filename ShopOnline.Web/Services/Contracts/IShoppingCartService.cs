using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<CartitemDto>> GetItems(int userId);
        Task<CartitemDto> AddItem(CartItemToAddDto cartItemToAddDto);
    }
}
