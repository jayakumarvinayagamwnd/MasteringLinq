namespace LinqExercises.Shared.Entities;

public class Genre
{
    public int GenreId { get; set; }
    public string? Name { get; set; }
    // Navigation properties
    public ICollection<Track> Tracks { get; set; } = new List<Track>();
}
