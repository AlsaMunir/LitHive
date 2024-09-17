using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Application.Services;

namespace WebApplication6.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly ILogger<BookmarksController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly GenericService<Bookmarks> _repository;

        public BookmarksController(GenericService<Bookmarks> repository, ILogger<BookmarksController> logger, IWebHostEnvironment env)
        {
            _repository = repository;
            _logger = logger;
            _env = env;
        }

        string cs = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QueensDB;Integrated Security=True;";

        public async Task<IActionResult> Index()
        {
            List<Bookmarks> bookmarks = await _repository.GetAllAsync();
            return View(bookmarks);
        }

        [HttpGet]
        public IActionResult AddBookmark()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBookmark([FromForm] Bookmarks b)
        {
            string wwwrootPath = _env.WebRootPath;
            string path = Path.Combine(wwwrootPath, "UploadedFiles/Bookmarks");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, b.CoverImage.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await b.CoverImage.CopyToAsync(fileStream);
            }

            string imagePathinroot = "~/UploadedFiles/Bookmarks/" + b.CoverImage.FileName;
            b.ImageUrl = imagePathinroot;

            await _repository.AddAsync(b);
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string markname, int price, int quantity, IFormFile CoverImage)
        {
            var existingBookmark = (await _repository.GetAllAsync()).FirstOrDefault(b => b.markname == markname);

            if (existingBookmark == null)
            {
                return NotFound();
            }

            existingBookmark.markname = markname;
            existingBookmark.price = price;
            existingBookmark.quantity = quantity;

            if (CoverImage != null)
            {
                string wwwrootPath = _env.WebRootPath;
                string path = Path.Combine(wwwrootPath, "UploadedFiles/Bookmarks");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = Path.Combine(path, CoverImage.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await CoverImage.CopyToAsync(fileStream);
                }

                string imagePathinroot = "~/UploadedFiles/Bookmarks/" + CoverImage.FileName;
                existingBookmark.ImageUrl = imagePathinroot;
            }

            await _repository.UpdateAsync(existingBookmark);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string markname)
        {
            if (string.IsNullOrEmpty(markname))
            {
                return BadRequest("Bookmark name cannot be empty.");
            }

            var existingBookmark = (await _repository.GetAllAsync()).FirstOrDefault(b => b.markname == markname);

            if (existingBookmark == null)
            {
                return NotFound($"Bookmark with name '{markname}' not found.");
            }

            await _repository.DeleteAsync(existingBookmark.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
