namespace LinqExercises.Shared.Entities;

public class Track
{
    public int TrackId { get; set; }
    public string? Name { get; set; }
    public int AlbumId { get; set; }
    public int MediaTypeId { get; set; }
    public int GenreId { get; set; }
    public string? Composer { get; set; }
    public int Milliseconds { get; set; }
    public int? Bytes { get; set; }
    public decimal UnitPrice { get; set; }

    // Navigation properties
    public Album? Album { get; set; } = null!;    
    public Genre? Genre { get; set; } = null!;    
    public MediaType? MediaType { get; set; } = null!;
    public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    public ICollection<PlaylistTrack> Playlists { get; set; } = new List<PlaylistTrack>();
}
