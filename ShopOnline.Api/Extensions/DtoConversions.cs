using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extension
{
    public static class DtoConversions
    {
        /* "Veri Transfer Nesneleri"
         `IEnumerable`, .NET platformunda yaygın olarak kullanılan bir arabirimdir ve genellikle koleksiyonlar üzerinde döngü işlemlerinin gerçekleştirilmesini sağlar. 
        "I" harfi arayüzü belirtir, "Enumerable" ise numaralandırılabilir bir nesne olduğunu gösterir.
        `IEnumerable`, bir koleksiyonun elemanlarını tek tek gezinmek için kullanılan bir arabirimdir. 
        Bu arabirimi uygulayan nesneler, koleksiyonları üzerinde döngü işlemleri gerçekleştirebilir ve bu koleksiyonlardaki elemanlara erişebilirler.
        `IEnumerable` arayüzünün avantajları şunlardır:
        1. **Genel Kullanılabilirlik**: `IEnumerable`, birçok .NET koleksiyon türü tarafından uygulanır. Bu nedenle, `IEnumerable`'i kullanarak, 
        farklı koleksiyon türlerindeki verilere genel bir şekilde erişebilirsiniz.
        2. **Lazy Evaluation (Tembel Değerlendirme)**: `IEnumerable`, tembel değerlendirme prensibini destekler. Bu, koleksiyon elemanlarına erişildiğinde elemanların gerçekten hesaplanmadığı, 
        ancak ihtiyaç duyulduğunda hesaplandığı anlamına gelir. Bu, bellek kullanımını ve işlemci gücünü optimize etmeye yardımcı olabilir.
        3. **LINQ Desteği**: LINQ (Language Integrated Query) sorguları, `IEnumerable` koleksiyonlarını işlemek için sıklıkla kullanılır. Bu nedenle, `IEnumerable` arabirimi, 
        LINQ sorgularıyla birlikte kullanılarak verilerin filtrelenmesi, sıralanması, gruplanması ve dönüştürülmesi gibi işlemlerde yaygın olarak kullanılır.
        4. **Foreach Döngüsü Desteği**: `IEnumerable` arabirimi, `foreach` döngüsüyle kullanılabilir. Bu, koleksiyon üzerindeki elemanları kolayca döngüye almanızı sağlar.
        Genel olarak, `IEnumerable` arabirimi, .NET platformunda koleksiyonlar üzerinde çalışmak için temel bir araçtır ve genellikle veri işleme ve döngü işlemleri için kullanılır.*/
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products, IEnumerable<ProductCategory> productCategories)
        {
            return (from product in products
                    join productCategory in productCategories on product.CategoryId equals productCategory.Id
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageUrl = product.ImageURL,
                        Price = product.Price,
                        Qty = product.Qty,
                        CategoryId = product.CategoryId,
                        CategoryName = productCategory.Name
                    }).ToList();
        }

        public static ProductDto ConvertToDto(this Product product, ProductCategory productCategory)
        {
        
          
                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageURL,
                    Price = product.Price,
                    Qty = product.Qty,
                    CategoryId = product.CategoryId,
                    CategoryName = productCategory.Name
                };
           
        }


    }
}