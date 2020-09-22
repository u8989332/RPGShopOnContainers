using CatalogApi.Data;
using CatalogApi.Domain;
using CatalogApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private readonly IOptionsSnapshot<CatalogSettings> _settings;
        private const string pictureUrlTemplate = "/api/picture/{0}";

        public CatalogController(CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogContext = catalogContext;
            _settings = settings;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await _catalogContext.CatalogTypes.ToArrayAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _catalogContext.CatalogItems
                .Select(x => new CatalogItemResponseVM {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    PictureUrl = x.PictureFileName
                })
                .SingleOrDefaultAsync(c => c.Id == id);

            if (item != null)
            {
                item.PictureUrl = ChangeItemPictureUrl(item.PictureUrl);
                return Ok(item);
            }

            return NotFound();
        }

        //Get api/Catalog/items[?catalogTypeId=&pageSize=4&pageIndex=3]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items(int? catalogTypeId, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var root = _catalogContext.CatalogItems.AsQueryable();
            if (catalogTypeId.HasValue)
            {
                root = root.Where(c => c.CatalogTypeId == catalogTypeId);
            }

            var totalItems = await root
                                .LongCountAsync();
            var itemsOnPage = await root
                                .Select(x => new CatalogItemResponseVM
                                {
                                    Description = x.Description,
                                    Id = x.Id,
                                    Name = x.Name,
                                    Price = x.Price,
                                    PictureUrl = x.PictureFileName
                                })
                                .OrderBy(c => c.Name)
                                .Skip(pageSize * pageIndex)
                                .Take(pageSize)
                                .ToListAsync();

            ChangeItemPictureUrls(itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItemResponseVM>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }

        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateProduct([FromBody] CatalogItem product)
        {
            var item = new CatalogItem
            {
                CatalogTypeId = product.CatalogTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };
            _catalogContext.CatalogItems.Add(item);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        [HttpPut]
        [Route("items")]
        public async Task<IActionResult> UpdateProduct([FromBody] CatalogItem productToUpdate)
        {
            var catalogItem = await _catalogContext.CatalogItems
                                .SingleOrDefaultAsync(c => c.Id == productToUpdate.Id);
            if (catalogItem == null)
            {
                return NotFound(new { Message = $"item with id {productToUpdate.Id} not found." });
            }

            catalogItem = productToUpdate;
            _catalogContext.CatalogItems.Update(catalogItem);
            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = productToUpdate.Id }, catalogItem);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _catalogContext.CatalogItems.SingleOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _catalogContext.CatalogItems.Remove(product);
            await _catalogContext.SaveChangesAsync();
            return NoContent();
        }

        private string ChangeItemPictureUrl(string fileName)
        {
            return _settings.Value.ExternalCatalogBaseUrl + string.Format(pictureUrlTemplate, fileName);
        }

        private void ChangeItemPictureUrls(List<CatalogItemResponseVM> list)
        {
            list.ForEach(x => x.PictureUrl = ChangeItemPictureUrl(x.PictureUrl));
        }
    }
}
