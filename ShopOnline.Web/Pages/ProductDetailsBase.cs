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
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

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

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItem= await ShoppingCartService.AddItem(cartItemToAddDto);
                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {

                //Log Exeption
            }
        }

    }
}
