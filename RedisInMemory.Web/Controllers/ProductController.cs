using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RedisInMemory.Web.Models;

namespace RedisInMemory.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //// 1.YOL 
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //{
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());
            //}

            //2.YOL BEST PRACTİCE

            MemoryCacheEntryOptions options = new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(60),
                SlidingExpiration = TimeSpan.FromSeconds(10),
                Priority = CacheItemPriority.High
            };
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}-{value}=> sebep:{reason}");
            });

            Product product = new() { Id = 1, Name = "Kalem", Price = 200 };
            _memoryCache.Set<Product>("product:1", product);

            _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);
            return View();
        }

        public IActionResult Show()
        {
            ////memoryden silme işlemi
            //_memoryCache.Remove("time");

            //_memoryCache.GetOrCreate<string>("time", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});

            _memoryCache.TryGetValue("time", out string? timecache);
            _memoryCache.TryGetValue("callback", out string? callback);
            ViewBag.Callback = callback;
            ViewBag.Time = timecache;
            var product = _memoryCache.Get<Product>("product:1");
            return View(product);
        }
    }
}
