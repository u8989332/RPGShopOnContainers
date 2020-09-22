using CatalogApi.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Data
{
    public class CatalogSeed
    {
        public static async Task SeedAsync(CatalogContext context)
        {
            if (!context.CatalogTypes.Any())
            {
                context.CatalogTypes.AddRange(GetDefaultCatalogTypes());
                await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                context.CatalogItems.AddRange(GetDefaultItems());
                await context.SaveChangesAsync();
            }
        }

        static IEnumerable<CatalogType> GetDefaultCatalogTypes()
        {
            return new List<CatalogType>()
            {
                new CatalogType() { Type = "Head"},
                new CatalogType() { Type = "Chest" },
                new CatalogType() { Type = "Hands" },
                new CatalogType() { Type = "Legs" },
                new CatalogType() { Type = "Feet" }
            };
        }
        static IEnumerable<CatalogItem> GetDefaultItems()
        {
            return new List<CatalogItem>()
            {
                new CatalogItem() { CatalogTypeId=1, Description = "Normal Helmet", Name = "Normal Helmet", Price = 10M, PictureFileName = "1.png" },
                new CatalogItem() { CatalogTypeId=1, Description = "Great Helmet", Name = "Great Helmet", Price= 50M, PictureFileName = "2.png" },
                new CatalogItem() { CatalogTypeId=2, Description = "Normal Armor", Name = "Normal Armor", Price = 15M, PictureFileName = "3.png" },
                new CatalogItem() { CatalogTypeId=2, Description = "Great Armor", Name = "Great Armor", Price = 42M, PictureFileName = "4.png" },
                new CatalogItem() { CatalogTypeId=4, Description = "Normal Robe", Name = "Normal Robe", Price = 12M, PictureFileName = "5.png" },
                new CatalogItem() { CatalogTypeId=4, Description = "Great Robe", Name = "Great Robe", Price = 44M, PictureFileName = "6.png" },
                new CatalogItem() { CatalogTypeId=3, Description = "Normal Gloves", Name = "Normal Gloves", Price = 5M, PictureFileName = "7.png" },
                new CatalogItem() { CatalogTypeId=3, Description = "Great Gloves", Name = "Great Gloves", Price = 69M, PictureFileName = "8.png" },
                new CatalogItem() { CatalogTypeId=5, Description = "Normal Boots", Name = "Normal Boots", Price = 13M, PictureFileName = "9.png" },
                new CatalogItem() { CatalogTypeId=5, Description = "Great Boots", Name = "Great Boots", Price = 55M, PictureFileName = "10.png" }
            };
        }
    }
}
