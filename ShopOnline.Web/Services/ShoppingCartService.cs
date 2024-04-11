using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.ComponentModel.Design;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient httpClient;

        public ShoppingCartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<CartitemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            //            Bu kod parçacığı, bir HTTP POST isteği gönderir ve yanıtı değerlendirir. İlgili kod adımlarını açıklayalım:
            //1. `httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto)`: Bu satır, `
            //httpClient` adlı bir HttpClient nesnesi aracılığıyla bir HTTP POST isteği gönderir. İstek, belirtilen URL'ye (`"api/ShoppingCart"`) ve belirtilen veri ile (`cartItemToAddDto`) gönderilir. `PostAsJsonAsync` metodu,
            //belirtilen nesneyi JSON biçiminde isteğin gövdesine ekler ve isteği belirtilen URL'ye gönderir.
            //2. `response.IsSuccessStatusCode`: Bu satır, gönderilen isteğin başarılı olup olmadığını kontrol eder.
            //Eğer istek başarılıysa, yani HTTP yanıt durumu 200 - 299 aralığında ise, bu ifade `true` değerini döndürür.
            //3. `response.StatusCode == System.Net.HttpStatusCode.NoContent`: Bu satır, yanıtın durum kodunun `NoContent` olup olmadığını kontrol eder.
            //`NoContent` durumu, sunucunun isteğe yanıt olarak içerik döndürmediğini ifade eder. Eğer yanıt durumu `NoContent` ise, bu durumda içerik olarak `null` veya varsayılan bir değer döndürülmesi gerekebilir.
            //Bu kod parçacığı, bir HTTP POST isteği gönderir, başarılı bir yanıt alındığında ve yanıtın durum kodu `NoContent` ise, belirli bir değeri varsayılan olarak döndürür veya belirli bir işlemi gerçekleştirir.Örneğin, `null` döndürme veya bir başka işlem yapma gibi.
            try
            {
                var response = await httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartitemDto);
                    }

                    return await response.Content.ReadFromJsonAsync<CartitemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http satatus:{response.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CartitemDto>> GetItems(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartitemDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<CartitemDto>>();

                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http satatus:{response.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
           
           
            
        }
    }
}
