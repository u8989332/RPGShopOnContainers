using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.Services
{
    public interface ICatalogService
    {
        Task<Catalog> GetCatalogItems(int pageIndex, int pageSize, int? type);
        Task<IEnumerable<SelectListItem>> GetTypes();
    }
}
