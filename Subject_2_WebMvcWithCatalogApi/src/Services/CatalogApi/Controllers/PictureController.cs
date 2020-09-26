using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public PictureController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [Route("{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot + "/Pictures/", fileName);
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/png");
        }
    }
}
