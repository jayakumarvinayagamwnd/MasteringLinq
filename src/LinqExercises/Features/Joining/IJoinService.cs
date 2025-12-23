using LinqExercises.Shared.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace LinqExercises.Features.Joining
{
    public interface IJoinService
    {
        Task RunJoiningExerciseAsync();
    }

    public class JoinService : IJoinService
    {
        private readonly ILogger<JoinService> _logger;
        private readonly ChinookDbContext _dbContext;
        public JoinService(ILogger<JoinService> logger, ChinookDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }   
        public async Task RunJoiningExerciseAsync()
        {
            _logger.LogInformation("[JoinService.RunJoiningExerciseAsync] - Running joining exercises...");
            // Inner join example (commented out)
            var artistAlbum = await _dbContext.Artists
                .Join(_dbContext.Albums,
                    artist => artist.ArtistId,
                    album => album.ArtistId,
                    (artist, album) => new
                    {
                        ArtistName = artist.Name,
                        AlbumTitle = album.Title
                    })
                .ToListAsync();
            var artistAlbumTable = new Spectre.Console.Table().Title("[green]Artist - Album Join[/]").Border(Spectre.Console.TableBorder.Rounded)
                .AddColumn("Artist Name")
                .AddColumn("Album Title");
            artistAlbum.ForEach(item =>
                artistAlbumTable.AddRow(Spectre.Console.Markup.Escape(item.ArtistName ?? string.Empty), Spectre.Console.Markup.Escape(item.AlbumTitle ?? string.Empty))
            );
            Spectre.Console.AnsiConsole.Write(artistAlbumTable);
            Spectre.Console.AnsiConsole.MarkupLine($"[bold yellow]Results: {artistAlbum.Count}[/]");
            _logger.LogInformation("Press any key to continue to the next joining example..."); 
            Console.ReadKey();
            // Left join example 
            var albumTrack = await _dbContext.Albums
                .GroupJoin(_dbContext.Tracks,
                    album => album.AlbumId,
                    track => track.AlbumId,
                    (album, tracks) => new
                    {
                        AlbumTitle = album.Title,
                        TrackCount = tracks.Count()
                    })
                .ToListAsync();
            var albumTrackTable = new Spectre.Console.Table().Title("[green]Album - Track Left Join[/]").Border(Spectre.Console.TableBorder.Rounded)
                .AddColumn("Album Title")
                .AddColumn("Track Count");
            albumTrack.ForEach(item =>
                albumTrackTable.AddRow(Spectre.Console.Markup.Escape(item.AlbumTitle ?? string.Empty), Spectre.Console.Markup.Escape(item.TrackCount.ToString()))
            );
            Spectre.Console.AnsiConsole.Write(albumTrackTable);
            Spectre.Console.AnsiConsole.MarkupLine($"[bold yellow]Results: {albumTrack.Count}[/]");
            _logger.LogInformation("Press any key to continue to the next joining example...");
            Console.ReadKey();
            // Group join example
            var genreTrack = await _dbContext.Genres
                .GroupJoin(_dbContext.Tracks,   
                    genre => genre.GenreId,
                    track => track.GenreId,
                    (genre, tracks) => new
                    {
                        GenreName = genre.Name,
                        Tracks = tracks
                    })
                .ToListAsync();
            var genreTrackTable = new Spectre.Console.Table().Title("[green]Genre - Track Group Join[/]").Border(Spectre.Console.TableBorder.Rounded)
                .AddColumn("Genre Name")
                .AddColumn("Track Count");  
            genreTrack.ForEach(item =>
                genreTrackTable.AddRow(Spectre.Console.Markup.Escape(item.GenreName ?? string.Empty), Spectre.Console.Markup.Escape(item.Tracks.Count().ToString()))
            );
            Spectre.Console.AnsiConsole.Write(genreTrackTable);
            Spectre.Console.AnsiConsole.MarkupLine($"[bold yellow]Results: {genreTrack.Count}[/]");
            _logger.LogInformation("Press any key to continue to the next joining example...");
            Console.ReadKey();
            // Left Outer join 
            var customerInvoice = await _dbContext.Customers
                .GroupJoin(_dbContext.Invoices,
                    customer => customer.CustomerId,
                    invoice => invoice.CustomerId,
                    (customer, invoices) => new
                    {
                        CustomerName = customer.FirstName + " " + customer.LastName,
                        Invoices = invoices.DefaultIfEmpty()
                    })
                .SelectMany(
                    ci => ci.Invoices,
                    (ci, invoice) => new
                    {
                        ci.CustomerName,
                        InvoiceTotal = invoice != null ? invoice.Total : 0m
                    })
                .ToListAsync();
            var customerInvoiceTable = new Spectre.Console.Table().Title("[green]Customer - Invoice Left Outer Join[/]").Border(Spectre.Console.TableBorder.Rounded)
                .AddColumn("Customer Name")
                .AddColumn("Invoice Total");  
            customerInvoice.ForEach(item => 
                customerInvoiceTable.AddRow(Spectre.Console.Markup.Escape(item.CustomerName ?? string.Empty), Spectre.Console.Markup.Escape(item.InvoiceTotal.ToString("C")))
            );
            Spectre.Console.AnsiConsole.Write(customerInvoiceTable);
            Spectre.Console.AnsiConsole.MarkupLine($"[bold yellow]Results: {customerInvoice.Count}[/]");
            _logger.LogInformation("Press any key to finish the joining exercises...");
            Console.ReadKey();
            _logger.LogInformation("[JoinService.RunJoiningExerciseAsync] - Completed joining exercises.");
        }
    }
}