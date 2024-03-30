using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.NetworkInformation;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase:ComponentBase
    {
        [Inject]
        public IProductsServices ProductService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetItems();
        }

        //Bu metot, Products koleksiyonunu kategoriye göre gruplayarak gruplanmış ürünleri döndürür.Gruplar, CategoryId özelliğine göre anahtarlanır ve anahtara göre sıralanır.
        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
                                //Sonuç olarak, bu metot, kategoriye göre gruplanmış ve sıralanmış ürünlerin bir koleksiyonunu döndürür.
                                //Bu koleksiyon, her bir kategorinin anahtarını ve o kategoriye ait ürünlerin bir listesini içerir.
                                             return from product in Products
                                             group product by product.CategoryId into prodByCatGroup
                                             orderby prodByCatGroup.Key
                                             select prodByCatGroup;
        }
        //Bu metod, IGrouping<int, ProductDto> tipinden bir gruplanmış veri koleksiyonunu alır ve bu grup içindeki öğelerin kategori adını döndürür.
        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos) 
        {

                             return groupedProductDtos.FirstOrDefault(pg=> pg.CategoryId == groupedProductDtos.Key).CategoryName;//kategori adını döndürür.
        }
    }
}
