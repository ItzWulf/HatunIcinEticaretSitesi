using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IProductsServices
    {
        Task<IEnumerable<ProductDto>> GetItems();
    }
}
