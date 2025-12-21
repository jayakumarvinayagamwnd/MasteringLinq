namespace LinqExercises.Shared.Entities;

public sealed class InvoiceItem
{
    public int InvoiceLineId { get; set; }
    public int InvoiceId { get; set; }
    public int TrackId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    // Navigation properties
    public Invoice? Invoice { get; set; }
    public Track? Track { get; set; }
}
