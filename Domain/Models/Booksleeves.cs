using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace Domain.Models
{
    public class Booksleeves
    {
        public int Id { get; set; }
        public string sleevename { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile CoverImage { get; set; }
        public int CategoryId { get; set; }
    }
}
