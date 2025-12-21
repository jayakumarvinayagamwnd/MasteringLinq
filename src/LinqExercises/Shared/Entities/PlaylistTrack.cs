namespace LinqExercises.Shared.Entities;

public class PlaylistTrack
{
    public int PlaylistId { get; set; }
    public int TrackId { get; set; }
    // Navigation properties
    public Playlist? Playlist { get; set; }
    public Track? Track { get; set; }
}