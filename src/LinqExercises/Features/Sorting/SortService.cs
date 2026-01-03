using LinqExercises.Shared.Data;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Microsoft.EntityFrameworkCore;

namespace LinqExercises.Features.Sorting
{
    public class SortService : ISortService
    {
        private readonly ILogger<SortService> _logger;
        private readonly ChinookDbContext _dbContext;
        public SortService(ILogger<SortService> logger, ChinookDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }   
        public async Task RunSortingExerciseAsync()
        {
            _logger.LogInformation("[SortService.RunSortingExerciseAsync] - Running sorting exercises...");
            // Basic Sorting Example
            _logger.LogInformation("[SortService.RunSortingExerciseAsync] - Basic Sorting Example");
            var artists = await _dbContext.Artists
                .OrderBy(a => a.Name)
                .ToListAsync();
            var artistTable = new Table().Title("[green]Sorted Artists[/]").Border(TableBorder.Rounded).AddColumn("ArtistId").AddColumn("Name");                                   
            artists.ForEach(artist =>
                artistTable.AddRow(Markup.Escape(artist.ArtistId.ToString()), Markup.Escape(artist.Name ?? string.Empty))
            );
            AnsiConsole.Write(artistTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {artists.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press Enter to continue to the next sorting example...[/]");
            Console.ReadLine();
            
            // Multi-Level Sorting Example
            _logger.LogInformation("[SortService.RunSortingExerciseAsync] - Multi-Level Sorting Example");
            var albums = await _dbContext.Albums
                .Include(al => al.Artist)
                .OrderBy(al => al.Artist!.Name)
                .ThenByDescending(al => al.Title)
                .ToListAsync();
            var albumTable = new Table().Title("[green]Multi-Level Sorted Albums[/]").Border(TableBorder.Rounded).AddColumn("Title").AddColumn("ArtistName");
            albums.ForEach(album =>
                albumTable.AddRow(Markup.Escape(album.Title ?? string.Empty), Markup.Escape(album.Artist!.Name ?? string.Empty))
            );
            AnsiConsole.Write(albumTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {albums.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press Enter to continue to the next sorting example...[/]");
            Console.ReadLine();
            
            // OrderByDescending Example
            _logger.LogInformation("[SortService.RunSortingExerciseAsync] - OrderByDescending Example");
            var tracks = await _dbContext.Tracks
                .OrderByDescending(t => t.UnitPrice)
                .ToListAsync();
            var trackTable = new Table().Title("[green]Tracks Sorted by Unit Price Descending[/]").Border(TableBorder.Rounded).AddColumn("TrackId").AddColumn("Name").AddColumn("UnitPrice");
            tracks.ForEach(track =>
                trackTable.AddRow(Markup.Escape(track.TrackId.ToString()), Markup.Escape(track.Name ?? string.Empty), Markup.Escape(track.UnitPrice.ToString("C")))
            );
            AnsiConsole.Write(trackTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {tracks.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press Enter to continue to the next sorting example...[/]");
            Console.ReadLine();

            // Reverse
            _logger.LogInformation("[SortService.RunSortingExerciseAsync] - Reverse Example");
            var reversedArtists = await _dbContext.Artists
                .OrderBy(a => a.ArtistId)
                .ToListAsync();
            reversedArtists.Reverse();
            var reversedArtistTable = new Table().Title("[green]Reversed Artists[/]").Border(TableBorder.Rounded).AddColumn("ArtistId").AddColumn("Name");                                   
            reversedArtists.ForEach(artist =>
                reversedArtistTable.AddRow(Markup.Escape(artist.ArtistId.ToString()), Markup.Escape(artist.Name ?? string.Empty))
            );
            AnsiConsole.Write(reversedArtistTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {reversedArtists.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press Enter to continue to the next sorting example...[/]");
            Console.ReadLine();

            // ThenBy
            _logger.LogInformation("[SortService.RunSortingExerciseAsync] - ThenBy Example");
            var customers = await _dbContext.Customers
                .OrderBy(c => c.Country)
                .ThenBy(c => c.City)
                .ToListAsync();
            var customerTable = new Table().Title("[green]Customers Sorted by Country and City[/]").Border(TableBorder.Rounded)
                .AddColumn("CustomerId").AddColumn("Name").AddColumn("Country").AddColumn("City");
            customers.ForEach(customer =>
                customerTable.AddRow(
                    Markup.Escape(customer.CustomerId.ToString()),
                    Markup.Escape((customer.FirstName + " " + customer.LastName) ?? string.Empty),
                    Markup.Escape(customer.Country ?? string.Empty),
                    Markup.Escape(customer.City ?? string.Empty))
            );
            AnsiConsole.Write(customerTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {customers.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press Enter to continue to the next sorting example...[/]");
            Console.ReadLine();
            
            // ThenByDescending
            _logger.LogInformation("[SortService.RunSortingExerciseAsync] - ThenByDescending Example");
            var tracksByGenre = await _dbContext.Tracks
                .Include(t => t.Genre)
                .OrderBy(t => t.Genre!.Name)
                .ThenByDescending(t => t.Name)
                .ToListAsync();
            var trackByGenreTable = new Table().Title("[green]Tracks Sorted by Genre and Name Descending[/]").Border(TableBorder.Rounded)
                .AddColumn("TrackId").AddColumn("Name").AddColumn("Genre");
            tracksByGenre.ForEach(track =>
                trackByGenreTable.AddRow(
                    Markup.Escape(track.TrackId.ToString()),
                    Markup.Escape(track.Name ?? string.Empty),
                    Markup.Escape(track.Genre!.Name ?? string.Empty))
            );
            AnsiConsole.Write(trackByGenreTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {tracksByGenre.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press Enter to continue to finish sorting exercises...[/]");
            Console.ReadLine();
            _logger.LogInformation("[SortService.RunSortingExerciseAsync] - Completed sorting exercises.");
        }
    }
}