using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailsBase: ComponentBase
    {
        [Parameter]
        public int id { get; set; }
        [Inject]
        public IProductsServices ProductsServices { get; set; }
        public ProductDto Product { get; set; }
        public string ErrorMessage {  get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Product = await ProductsServices.GetItem(id);
            }
            catch (Exception ex)
            {

               ErrorMessage = ex.Message;
            }
        }

    }
}
