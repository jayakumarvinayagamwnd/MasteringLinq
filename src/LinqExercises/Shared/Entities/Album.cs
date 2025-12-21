namespace LinqExercises.Shared.Entities;
public class Album
{
    public int AlbumId { get; set; }
    public string? Title { get; set; }
    public int ArtistId { get; set; }
    // Navigation properties
    public Artist? Artist { get; set; } = null!;
    public ICollection<Track> Tracks { get; set; } = new List<Track>();
}
