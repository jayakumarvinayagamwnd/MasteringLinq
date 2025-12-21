namespace LinqExercises.Shared.Entities;

public class Invoice
{
    public int InvoiceId { get; set; }
    public int CustomerId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string? BillingAddress { get; set; }
    public string? BillingCity { get; set; }
    public string? BillingState { get; set; }
    public string? BillingCountry { get; set; }
    public string? BillingPostalCode { get; set; }
    public decimal Total { get; set; }
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}
