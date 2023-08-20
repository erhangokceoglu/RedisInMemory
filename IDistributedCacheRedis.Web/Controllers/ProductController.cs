using IDistributedCacheRedis.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedis.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions options = new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1)
            };
            //await _distributedCache.SetStringAsync("Product", "Pencel", options);

            Product product = new()
            {
                Id = 1,
                Name = "Pencel",
                Price = 150
            };
            var jsonProduct = JsonConvert.SerializeObject(product);
            //await _distributedCache.SetStringAsync("Product:1", jsonProduct, options);
            var byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            await _distributedCache.SetAsync("Product:1", byteProduct, options);


            return View();
        }

        public async Task<IActionResult> Show()
        {
            //var name = await _distributedCache.GetStringAsync("Product");
            //ViewBag.Name = name;

            var byteProduct = await _distributedCache.GetAsync("Product:1");
            var jsonProduct = Encoding.UTF8.GetString(byteProduct!);
            var product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            //var jsonProduct = await _distributedCache.GetStringAsync("Product:1");
            //var product = JsonConvert.DeserializeObject<Product>(jsonProduct!);

            return View(product);
        }

        public async Task<IActionResult> Remove()
        {
            //await _distributedCache.RemoveAsync("Product");
            await _distributedCache.RemoveAsync("Product:1");
            return View();
        }

        public async Task<IActionResult> ImageCache()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/erhanprofil.jpg");
            var iamageByte = System.IO.File.ReadAllBytes(path);
            await _distributedCache.SetAsync("Image", iamageByte);
            return View();
        }

        public async Task<IActionResult> ImageUrl()
        {
            var imagePath = await _distributedCache.GetAsync("Image");
            return File(imagePath!, "image/jpg");
        }
        public async Task<IActionResult> RemoveImage()
        {
            await _distributedCache.RemoveAsync("Image");
            return View();
        }

    }
}
