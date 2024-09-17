using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public string SessionId { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Books Book { get; set; }
       
        public int Quantity { get; set; }
    }
}
