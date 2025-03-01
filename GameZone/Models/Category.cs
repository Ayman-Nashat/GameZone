namespace GameZone.Models
{
    public class Category:BaseEntity
    {
        ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
