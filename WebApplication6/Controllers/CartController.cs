using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication6.Data;
using Domain.Models;


public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }
   
   
    [HttpPost]
    public async Task<IActionResult> AddToCart(int id)
    {
        Console.WriteLine("Received id: " + id);

        if (id <= 0)
        {
            return BadRequest("Invalid book ID");
        }

        Books book = null;
        var connectionString = _context.Database.GetDbConnection().ConnectionString;

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM Books WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        book = new Books
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Bookname = reader.GetString(reader.GetOrdinal("Bookname")),
                            Price = reader.GetInt32(reader.GetOrdinal("Price")),  
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                            
                        };
                    }
                }
            }
        }

        if (book == null)
        {
            return RedirectToAction("fiction", "home"); 
        }

        var sessionId = HttpContext.Session.Id;

        var existingCartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.SessionId == sessionId && ci.BookId == id);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity++;
        }
        else
        {
            var cartItem = new CartItem
            {
                SessionId = sessionId,
                BookId = id,
                Quantity = 1

            };
            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index","home");
       
    }
   
    [HttpGet]
    public async Task<IActionResult> ViewCart()
    {
        var sessionId = HttpContext.Session.Id;

        var cartItems = await _context.CartItems
            .Include(ci => ci.Book)
            .Where(ci => ci.SessionId == sessionId)
            .ToListAsync();

        return View(cartItems);
    }
    public IActionResult Checkout()
    {
        return View();
    }
}