namespace LinqExercises.Shared.Entities;

public class Artist
{
    public int ArtistId { get; set; }
    public string? Name { get; set; }
    // Navigation properties
    public ICollection<Album> Albums { get; set; } = new List<Album>();
}
