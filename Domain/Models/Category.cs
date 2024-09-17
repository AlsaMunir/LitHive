namespace Domain.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Books> Books { get; set; }
        public ICollection<Bookmarks> Bookmarks { get; set; }
       
    }
}
