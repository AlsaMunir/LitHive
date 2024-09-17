using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;
namespace Domain.Models
{
    public class Books
    {
        public int Id { get; set; }
        [Required]
        public string Bookname { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
       
        public string ImageUrl { get; set; }
        [NotMapped]
      
        public IFormFile CoverImage { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
