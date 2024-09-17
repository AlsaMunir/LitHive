using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using WebApplication6.Models.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Threading.Tasks;
using Application.Services;

public class HomeController : Controller
{
    private readonly GenericService<Books> _booksRepository;
    private readonly GenericService<Bookmarks> _bookmarksRepository;
    private readonly GenericService<Booksleeves> _booksleevesRepository;
    private readonly IMemoryCache bmcache;
    private readonly IMemoryCache booksleevescache;
    private readonly IMemoryCache booksmarkscache;

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> h, GenericService<Books> booksRepository, GenericService<Bookmarks> bookmarksRepository, GenericService<Booksleeves> booksleevesRepository, IMemoryCache bs, IMemoryCache bm)
    {
        _logger = h;
        _booksRepository = booksRepository;
        _bookmarksRepository = bookmarksRepository;
        _booksleevesRepository = booksleevesRepository;
        booksleevescache = bs;
        bmcache = bm;
    }

    [Authorize(Policy = "PakOnly")]
    public IActionResult index()
    {
        HttpContext.Session.SetString("Username", "Fatima");
        string message = string.Empty;
        if (HttpContext.Request.Cookies.ContainsKey("first_visit"))
        {
            string data = HttpContext.Request.Cookies["first_visit"];
            message = $"Welcome back! Your first visit was on {data}";
        }
        else
        {
            HttpContext.Response.Cookies.Append("first_visit", DateTime.Now.ToString());
            message = "Welcome to our website!";
        }
        ViewBag.WelcomeMessage = message;
        return View();
    }

    [Authorize]
    [HttpGet]
    public IActionResult fiction()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<JsonResult> fictionJ()
    {
        List<Books> b1 = await _booksRepository.GetAllAsync();
        return Json(b1);
    }

    [Authorize]
    [HttpGet]
    public IActionResult nonfiction()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<JsonResult> nonfictionJ()
    {
        List<Books> b1 = await _booksRepository.GetAllAsync();
        return Json(b1);
    }

    [Authorize]
    [HttpGet]
    public IActionResult selfhelp()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<JsonResult> selfhelpJ()
    {
        List<Books> b1 = await _booksRepository.GetAllAsync();
        return Json(b1);
    }

    [Authorize]
    [HttpGet]
    public IActionResult bookmark()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<JsonResult> bookmarkJ()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        string ck = "bmKey";
        if (!bmcache.TryGetValue(ck, out IEnumerable<Bookmarks> b1))
        {
            b1 = await _bookmarksRepository.GetAllAsync();
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal);

            bmcache.Set(ck, b1, options);
            _logger.Log(LogLevel.Information, "Bookmarks not found in cache, fetched from database.");
        }
        else
        {
            _logger.Log(LogLevel.Information, "Bookmarks found in cache");
        }

        stopwatch.Stop();
        _logger.Log(LogLevel.Information, "Passed time: " + stopwatch.ElapsedMilliseconds);

        return Json(b1);
    }

    [Authorize]
    public IActionResult bookproduct()
    {
        Books book = new Books
        {
            Id = 1,
            CategoryId = 1,
            Bookname = "Silent Patient",
            Description = "It tells about...",
            Price = 300,
            Quantity = 2,
            ImageUrl = "~/UploadedFiles/fiction/silent.jpg"
        };

        Bookmarks bookmark = new Bookmarks
        {
            Id = 1,
            markname = "hand painted",
            price = 300,
            quantity = 4
        };

        bookproduct model = new bookproduct
        {
            b = book,
            bms = bookmark
        };

        return View(model);
    }

    [Authorize]
    public async Task<JsonResult> booksleeveJ()
    {
        string ck = "bscacheKey";
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        if (!booksleevescache.TryGetValue(ck, out IEnumerable<Booksleeves> b1))
        {
            b1 = await _booksleevesRepository.GetAllAsync();
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal);

            booksleevescache.Set(ck, b1, options);
            _logger.Log(LogLevel.Information, "Products not found in cache, fetched from database.");
        }
        else
        {
            _logger.Log(LogLevel.Information, "Products found in cache");
        }

        stopwatch.Stop();
        _logger.Log(LogLevel.Information, "Passed time: " + stopwatch.ElapsedMilliseconds);

        return Json(b1);
    }

    [Authorize]
    [HttpGet]
    public IActionResult booksleeve()
    {
        return View();
    }

    [Authorize]
    public IActionResult cart()
    {
        return View();
    }

    [Authorize]
    public IActionResult UploadImage()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<JsonResult> SearchJ(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return Json(new { success = false, message = "Query is empty." });
        }

        var books = await _booksRepository.GetAllAsync();
        var bookmarks = await _bookmarksRepository.GetAllAsync();
        var booksleeves = await _booksleevesRepository.GetAllAsync();

        var results = new List<object>();
        results.AddRange(books.Where(b => b.Bookname.Contains(query)).Select(b => new { b.Bookname, b.ImageUrl, b.CategoryId, b.Price }));
        results.AddRange(bookmarks.Where(b => b.markname.Contains(query)).Select(b => new { b.markname, b.ImageUrl, b.CategoryId, b.price }));
        results.AddRange(booksleeves.Where(b => b.sleevename.Contains(query)).Select(b => new { b.sleevename, b.ImageUrl, b.CategoryId, b.price }));

        if (results.Any())
        {
            return Json(new { success = true, data = results });
        }

        return Json(new { success = false, message = "No results found." });
    }
}
