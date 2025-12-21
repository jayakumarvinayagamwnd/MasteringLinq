namespace LinqExercises.Shared.Entities;

public class MediaType
{
    public int MediaTypeId { get; set; }
    public string? Name { get; set; }
    // Navigation properties
    public ICollection<Track> Tracks { get; set; } = new List<Track>();
}
