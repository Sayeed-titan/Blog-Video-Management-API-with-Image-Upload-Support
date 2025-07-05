namespace MasterDetails.API.Entities
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
