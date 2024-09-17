using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Application.Services;

namespace WebApplication6.Controllers
{
    public class BooksleevesController : Controller
    {
        private readonly ILogger<BooksleevesController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly GenericService<Booksleeves> _repository;

        public BooksleevesController(GenericService<Booksleeves> repository, ILogger<BooksleevesController> logger, IWebHostEnvironment env)
        {
            _repository = repository;
            _logger = logger;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Booksleeves> books = await _repository.GetAllAsync();
            return View(books);
        }

        [HttpGet]
        public IActionResult AddBooksleeves()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBooksleeves([FromForm] Booksleeves b)
        {
            string wwwrootPath = _env.WebRootPath;
            string path = Path.Combine(wwwrootPath, "UploadedFiles/Booksleeve");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, b.CoverImage.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await b.CoverImage.CopyToAsync(fileStream);
            }

            string imagePathinroot = "~/UploadedFiles/Booksleeve/" + b.CoverImage.FileName;
            b.ImageUrl = imagePathinroot;

            await _repository.AddAsync(b);
            return View();
        }
    }
}
