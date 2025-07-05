namespace MasterDetails.API.Entities
{
    public class Tag
    {
        public int TagID { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
    }
}
