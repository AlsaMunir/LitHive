using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using WebApplication6.Hubs;
using Microsoft.AspNetCore.SignalR;
using Application.Services;

namespace WebApplication6.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class booksController : Controller
    {
        private readonly ILogger<booksController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly GenericService<Books> _repository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public booksController(GenericService<Books> repository, ILogger<booksController> logger, IWebHostEnvironment env, IHubContext<NotificationHub> notify)
        {
            _repository = repository;
            _logger = logger;
            _env = env;
            _hubContext = notify;
        }

        public async Task<IActionResult> Index()
        {
            List<Books> books = await _repository.GetAllAsync();
            return View(books);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Add([FromForm] Books b)
        {
            string wwwrootPath = _env.WebRootPath;
            string path = Path.Combine(wwwrootPath, "UploadedFiles/Books");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, b.CoverImage.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await b.CoverImage.CopyToAsync(fileStream);
            }

            string imagePathinroot = "~/UploadedFiles/Books/" + b.CoverImage.FileName;
            b.ImageUrl = imagePathinroot;

            await _repository.AddAsync(b);
            
            await _hubContext.Clients.All.SendAsync("Msg", b.Bookname);

            return Json(b);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string bookname, int price, string description, int quantity, IFormFile CoverImage)
        {
            var existingBookmark = (await _repository.GetAllAsync()).FirstOrDefault(b => b.Bookname == bookname);

            if (existingBookmark == null)
            {
                return NotFound();
            }

            existingBookmark.Bookname = bookname;
            existingBookmark.Price = price;
            existingBookmark.Description = description;
            existingBookmark.Quantity = quantity;

            if (CoverImage != null)
            {
                string wwwrootPath = _env.WebRootPath;
                string path = Path.Combine(wwwrootPath, "UploadedFiles/Books");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = Path.Combine(path, CoverImage.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await CoverImage.CopyToAsync(fileStream);
                }

                string imagePathinroot = "~/UploadedFiles/Books/" + CoverImage.FileName;
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

            var existingBookmark = (await _repository.GetAllAsync()).FirstOrDefault(b => b.Bookname == markname);

            if (existingBookmark == null)
            {
                return NotFound($"Bookmark with name '{markname}' not found.");
            }

            await _repository.DeleteAsync(existingBookmark.Id);

            return RedirectToAction(nameof(Index));
        }
    }
}
