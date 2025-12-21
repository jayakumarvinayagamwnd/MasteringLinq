using LinqExercises.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace LinqExercises.Features.Projection
{
    public class ProjectionService : IProjectionService
    {
        private readonly ILogger<ProjectionService> _logger;
        private readonly ChinookDbContext _dbContext;
        public ProjectionService(ILogger<ProjectionService> logger, ChinookDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task RunProjectionExerciseAsync()
        {
            // Implementation of projection exercises would go here
            _logger.LogInformation("[ProjectionService.RunProjectionExerciseAsync] - Running projection exercises..."); 
            // Basic Projection Example
            _logger.LogInformation("Basic Projection of Artists:");
            var artists = await _dbContext.Artists.Select(a => new { a.Name, a.ArtistId }).ToListAsync();
            var artistTable = new Table().Title("[green]Artists[/]").Border(TableBorder.Rounded).AddColumn("ArtistId").AddColumn("Name");                                   
            artists.ForEach(artist =>
                artistTable.AddRow(Markup.Escape(artist.ArtistId.ToString()), Markup.Escape(artist.Name ?? string.Empty))
            );
            
            AnsiConsole.Write(artistTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {artists.Count}[/]");
            _logger.LogInformation("Press any key to continue to the next projection example..."); 
            Console.ReadKey();
            // Annomous Types Projection Example
            _logger.LogInformation("Projection to Anonymous Types of Albums:");
            var albums = await _dbContext.Albums
                .Select(al => new 
                { 
                    al.Title, 
                    ArtistName = al.Artist!.Name 
                })
                .ToListAsync();
            var albumTable = new Table().Title("[green]Albums[/]").Border(TableBorder.Rounded).AddColumn("Title").AddColumn("ArtistName");
            albums.ForEach(album =>
                albumTable.AddRow(Markup.Escape(album.Title ?? string.Empty), Markup.Escape(album.ArtistName ?? string.Empty))
            );
            AnsiConsole.Write(albumTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {albums.Count}[/]");
            _logger.LogInformation("Press any key to continue to the next projection example..."); 
            Console.ReadKey();
            // DTO-Projection to Record Types Example
            _logger.LogInformation("Projection to DTO Record Types of Customer Addresses:");
            var customerAddresses = await _dbContext.Customers
                .Select(c => new CustomerAddress(
                    c.FirstName + " " + c.LastName,
                    c.Address!,
                    c.City!,
                    c.State!,
                    c.Country!,
                    c.PostalCode!))
                .ToListAsync();
            var customerAddressTable = new Table().Title("[green]Customer Addresses[/]").Border(TableBorder.Rounded)
                .AddColumn("Name").AddColumn("Address").AddColumn("City")
                .AddColumn("State").AddColumn("Country").AddColumn("PostalCode");
            customerAddresses.ForEach(ca =>
                customerAddressTable.AddRow(
                    Markup.Escape(ca.Name ?? string.Empty),
                    Markup.Escape(ca.Address ?? string.Empty),
                    Markup.Escape(ca.City ?? string.Empty),
                    Markup.Escape(ca.State ?? string.Empty),
                    Markup.Escape(ca.Country ?? string.Empty),
                    Markup.Escape(ca.PostalCode ?? string.Empty))
            );
            AnsiConsole.Write(customerAddressTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {customerAddresses.Count}[/]");
            _logger.LogInformation("Press any key to continue to the next projection exercise..."); 
            Console.ReadKey();
            // //Nested Projection Example
            // _logger.LogInformation("Nested Projection of Albums with Tracks:");
            var albumWithTracks = await _dbContext.Customers
                .Select(c => new 
                {
                    c.FirstName,
                    c.LastName,
                    Invoices = c.Invoices!.Select(i => new 
                    {
                        i.InvoiceId,
                        i.Total                        
                    }).ToList()
                })
                .ToListAsync();
            var albumWithTracksTable = new Table().Title("[green]Customers with Invoices[/]").Border(TableBorder.Rounded)
                .AddColumn("FirstName").AddColumn("LastName").AddColumn("Invoices");
            albumWithTracks.ForEach(c =>
                albumWithTracksTable.AddRow(
                    Markup.Escape(c.FirstName ?? string.Empty),
                    Markup.Escape(c.LastName ?? string.Empty),
                    Markup.Escape(string.Join(", ", c.Invoices.Select(i => $"[InvoiceId: {i.InvoiceId}, Total: {i.Total}]"))))
            );
            AnsiConsole.Write(albumWithTracksTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {albumWithTracks.Count}[/]");
            _logger.LogInformation("Press any key to finish projection exercises..."); 
            Console.ReadKey();
            _logger.LogInformation("[ProjectionService.RunProjectionExerciseAsync] - Completed projection exercises.");
        }
    }
}