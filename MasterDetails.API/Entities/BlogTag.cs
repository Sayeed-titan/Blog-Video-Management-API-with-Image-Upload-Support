namespace MasterDetails.API.Entities
{
    public class BlogTag
    {
        public int BlogID { get; set; }
        public Blog Blog { get; set; } = null!;

        public int TagID { get; set; }
        public Tag Tag { get; set; } = null!;
    }

}
