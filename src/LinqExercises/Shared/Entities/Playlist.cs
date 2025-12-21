namespace LinqExercises.Shared.Entities;

public class Playlist
{
    public int PlaylistId { get; set; }
    public string? Name { get; set; } 
    // Navigation properties
    public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
}
