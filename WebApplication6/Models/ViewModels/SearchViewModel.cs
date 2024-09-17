using Domain.Models;
namespace WebApplication6.Models.ViewModels
{
    public class SearchViewModel
    {
        public string Query { get; set; }
        public List<Books> Books { get; set; }
        public List<Bookmarks> Bookmarks { get; set; }
        public List<Booksleeves> Booksleeves { get; set; }
    }
}
