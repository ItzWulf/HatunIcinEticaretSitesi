using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
            
        }
        /*
         * Bu metod, belirli bir alışveriş sepetinde (cart) belirli bir ürünün (product) zaten var olup olmadığını kontrol ediyor. 
         * Şimdi daha detaylı açıklayalım:
            1. **Parametreler**: Metot, iki parametre alır: `cartId` ve `productId`. Bu parametreler, 
                 kontrol edilmek istenen alışveriş sepetinin kimliği ve ürünün kimliğidir.
            2. **Veritabanı Sorgusu**: Metot, bu parametrelerle birlikte `shopOnlineDbContext` üzerindeki `Cartitems` 
                 tablosunda bir sorgu çalıştırır. `AnyAsync` metodu, belirtilen koşulları sağlayan herhangi bir kayıt olup olmadığını kontrol eder.
            3. **Koşullar**: Sorguda, belirtilen `cartId` ve `productId` değerlerine sahip herhangi bir `Cartitem` kaydı var mı diye kontrol edilir. 
                 Yani, aynı sepette aynı ürünün birden fazla kez eklenip eklenmediği kontrol edilir.
            4. **Sonuç**: Eğer belirtilen koşulları sağlayan bir kayıt varsa, metot `true` değeri döner. Aksi takdirde, 
                 yani belirtilen koşulları sağlayan bir kayıt yoksa, `false` değeri döner.
            Bu metod, alışveriş sepetine eklenen ürünlerin benzersiz olmasını sağlamak için kullanılır. 
                 Eğer aynı ürün zaten sepette varsa, bu metod bu durumu tespit eder ve buna göre işlem yapılmasını sağlar.
         */
        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await this.shopOnlineDbContext.Cartitems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
        }
        /*
        Tabii ki, bu metod bir alışveriş sepetine yeni bir ürün eklemek için kullanılıyor. 
        Şimdi biraz daha detaylı anlatayım:
        1. **CartItemToAddDto**: Bu metod, bir `CartItemToAddDto` türünden bir parametre alıyor. Bu parametre, 
           sepete eklemek istediğimiz ürünün bilgilerini içeriyor.
        2. **Ürünün Aranması**: Metot, önce alışveriş sepetine eklemek istediğimiz ürünün veritabanında var olup olmadığını kontrol ediyor. 
            Bunun için ürünün Id'sine göre veritabanında bir sorgu yapıyor.
        3. **Yeni Ürünün Oluşturulması**: Eğer ürün bulunursa, bu ürünün bilgilerini kullanarak yeni bir 
            `Cartitem` nesnesi oluşturuluyor. Bu nesne, sepete eklenmek üzere hazırlanıyor.
        4. **Ürünün Eklenmesi ve Kaydedilmesi**: Yeni oluşturulan `Cartitem` nesnesi, 
            veritabanına ekleniyor (`AddAsync` metodu ile) ve değişiklikler veritabanına kaydediliyor (`SaveChangesAsync` metodu ile).
        5. **Sonuç**: Eğer ürün başarıyla eklenirse, bu işlem sonucunda oluşturulan `Cartitem` nesnesi dönülüyor. Eğer ürün bulunamazsa, 
            `null` değeri dönülüyor.
            Yani, bu metod bir alışveriş sepetine ürün eklerken izlenen adımları temsil ediyor. 
            Ürünün var olup olmadığını kontrol ediyor, yeni bir ürün oluşturuyor, ekliyor ve sonuç olarak dönüyor.
         */
        public async Task<Cartitem> AddItem(CartItemToAddDto cartItemToAddDto)
        {

            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in this.shopOnlineDbContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new Cartitem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = product.Id,
                                      Qty = cartItemToAddDto.Qty
                                  }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await this.shopOnlineDbContext.AddAsync(item);
                    await this.shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }
            
            return null;
        }

        public Task<Cartitem> DeleteItem(int id)
        {
            throw new NotImplementedException();
        }
        /*
         * Bu metot, belirli bir kimliğe sahip olan alışveriş sepetindeki bir ürünü getirmek için kullanılıyor. İşte bu metotun adım adım açıklaması:

            1. **Parametre**: Metot, `id` adında bir parametre alır. Bu parametre, getirilmek istenen ürünün kimliğini belirtir.
            2. **Veritabanı Sorgusu**: Metot, LINQ, "Language Integrated Query" sorgusu kullanarak iki tabloyu birleştiriyor: `Carts` ve `Cartitems`. 
               Bu birleştirme işlemi `CartId` alanı üzerinden yapılır ve her bir alışveriş sepeti ile bu sepete ait olan ürünlerin ilişkisi kurulur.
            3. **Filtreleme (Where)**: `where` ifadesiyle, `Cartitems` tablosundaki `Id` alanı verilen `id` parametresiyle karşılaştırılır. Böylece, 
               istenen ürünün belirli bir kimliğe sahip olup olmadığı kontrol edilir.
            4. **Yeni Nesne Oluşturma**: Eğer belirtilen `id` değerine sahip bir ürün bulunursa, 
               bu ürün bilgileri bir `Cartitem` nesnesine kopyalanır. Bu nesne, alışveriş sepetindeki bir ürünü temsil eder ve `Id`, `ProductId`, `Qty` ve 
               `CartId` gibi özellikleri alır.
            5. **Sonuç**: Son olarak, `SingleOrDefaultAsync` metodu kullanılarak sorgunun sonucu alınır ve bu sonuç bir `Cartitem` olarak döndürülür. 
               Eğer belirtilen `id` değerine sahip bir ürün bulunamazsa, `null` döner.
               Bu metot, belirli bir kimliğe sahip olan alışveriş sepetindeki bir ürünü getirmek için kullanılır. 
               Eğer belirtilen `id` değerine sahip bir ürün bulunursa, bu ürünün bilgileri bir `Cartitem` nesnesi olarak döndürülür.*/
        public async Task<Cartitem> GetItem(int id)
        {
            return await (from cart in this.shopOnlineDbContext.Carts
                          join carItem in this.shopOnlineDbContext.Cartitems
                          on cart.Id equals carItem.CartId
                          where carItem.Id == id
                          select new Cartitem
                          {
                              Id = carItem.Id,
                              ProductId = carItem.ProductId,
                              Qty = carItem.Qty,
                              CartId = carItem.CartId,
                          }).SingleOrDefaultAsync();
        }

        /*
         Bu metod, belirli bir kullanıcıya ait alışveriş sepetinde bulunan ürünleri getirmek için kullanılıyor. 
         İşte bu metodun ne yaptığını daha detaylı açıklayalım:
            1. **Parametre**: Metot, `userId` adında bir parametre alıyor. Bu parametre, alışveriş sepetindeki ürünleri getirmek istediğimiz 
            kullanıcının kimliğini belirtir.
            2. **Veritabanı Sorgusu**: Metot, LINQ sorgusu kullanarak iki tabloyu birleştiriyor: `Carts` ve `Cartitems`. `Carts` tablosu, 
            kullanıcıların alışveriş sepetlerini temsil ederken, `Cartitems` tablosu, alışveriş sepetlerindeki ürünleri temsil eder.
            3. **Birleştirme (Join)**: `join` ifadesiyle `Carts` ve `Cartitems` tablolarını birleştiriyoruz. 
            Bu birleştirme, `CartId` alanı üzerinden yapılıyor. Yani, her bir alışveriş sepetinin içindeki ürünlerle ilgili bilgileri alıyoruz.
            4. **Filtreleme (Where)**: Kullanıcının sepetine ait olan ürünleri seçmek için `where` ifadesi kullanıyoruz. 
            Bu ifade, `Carts` tablosundaki `UserId` alanı ile verilen `userId` parametresini karşılaştırıyor ve eşleşen kayıtları seçiyor.
            5. **Yeni Nesne Oluşturma**: Seçilen her bir ürün için, bir `Cartitem` nesnesi oluşturuyoruz. 
            Bu nesne, alışveriş sepetindeki bir ürünü temsil eder ve `Id`, `ProductId` ve `Qty` gibi özellikleri alır.
            6. **Sonuç**: Son olarak, `ToListAsync` metodu ile sorgunun sonucu alınıyor ve bir `IEnumerable<Cartitem>` olarak döndürülüyor. 
            Bu, alışveriş sepetinde bulunan ürünlerin listesidir.
            Bu metod, belirli bir kullanıcının alışveriş sepetindeki ürünleri getirmek için kullanılır ve bu ürünleri `Cartitem` nesneleri şeklinde döndürür.
         */
        public async Task<IEnumerable<Cartitem>> GetItems(int userId)
        {
            return await (from cart in this.shopOnlineDbContext.Carts
                          join cartItem in this.shopOnlineDbContext.Cartitems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId 
                          select new Cartitem {
                              Id = cartItem.CartId,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty
                          }).ToListAsync();
        }

        public Task<Cartitem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
