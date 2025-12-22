using LinqExercises.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace LinqExercises.Features.Filtering
{
    public interface IFilterService
    {
        Task RunFilteringExerciseAsync();
    }

    public class FilterService : IFilterService
    {
        private readonly ILogger<FilterService> _logger;
        private readonly ChinookDbContext _dbContext;
        public FilterService(ILogger<FilterService> logger, ChinookDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task RunFilteringExerciseAsync()
        {
            // Implementation of filtering exercises would go here
            _logger.LogInformation("[FilterService.RunFilteringExerciseAsync] - Running filtering exercises..."); 
            // Basic Filtering Example
            _logger.LogInformation("Basic Filtering of Tracks with UnitPrice > 0.99:");
            var expensiveTracks = await _dbContext.Tracks
                .Where(t => t.UnitPrice > 0.99m)
                .ToListAsync(); 
            var trackTable = new Table().Title("[green]Expensive Tracks[/]").Border(TableBorder.Rounded).AddColumn("TrackId").AddColumn("Name").AddColumn("UnitPrice");                                   
            expensiveTracks.ForEach(track =>
                trackTable.AddRow(Markup.Escape(track.TrackId.ToString()), Markup.Escape(track.Name ?? string.Empty), Markup.Escape(track.UnitPrice.ToString("C")))
            );
            AnsiConsole.Write(trackTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {expensiveTracks.Count}[/]");
            _logger.LogInformation("Press any key to continue to the next filtering example..."); 
            Console.ReadKey();
            // Complex Filtering Example
            _logger.LogInformation("Complex Filtering of Customers from USA or Canada with PostalCode starting with '9':");
            var filteredCustomers = await _dbContext.Customers
                .Where(c => (c.Country == "USA" || c.Country == "Canada") && c.PostalCode!.StartsWith("9"))
                .ToListAsync();
            var customerTable = new Table().Title("[green]Filtered Customers[/]").Border(TableBorder.Rounded).AddColumn("CustomerId").AddColumn("FirstName").AddColumn("LastName").AddColumn("Country").AddColumn("PostalCode");
            filteredCustomers.ForEach(customer =>
                customerTable.AddRow(Markup.Escape(customer.CustomerId.ToString()), Markup.Escape(customer.FirstName ?? string.Empty), Markup.Escape(customer.LastName ?? string.Empty), Markup.Escape(customer.Country ?? string.Empty), Markup.Escape(customer.PostalCode ?? string.Empty))
            );
            AnsiConsole.Write(customerTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {filteredCustomers.Count}[/]");
             _logger.LogInformation("Press any key to continue to the next filtering example..."); 
            Console.ReadKey();
            // Filtering from another list
            _logger.LogInformation("Filtering Tracks that are in a predefined list of TrackIds:");
            var trackIdsToFilter = new List<int> { 1, 3, 5, 7, 9 };
            var tracksInList = await _dbContext.Tracks
                .Where(t => trackIdsToFilter.Contains(t.TrackId))
                .ToListAsync();
            var tracksInListTable = new Table().Title("[green]Tracks in Predefined List[/]").Border(TableBorder.Rounded).AddColumn("TrackId").AddColumn("Name").AddColumn("UnitPrice");
            tracksInList.ForEach(track =>
                tracksInListTable.AddRow(Markup.Escape(track.TrackId.ToString()), Markup.Escape(track.Name ?? string.Empty), Markup.Escape(track.UnitPrice.ToString("C")))
            );
            AnsiConsole.Write(tracksInListTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {tracksInList.Count}[/]");
            
            _logger.LogInformation("[FilterService.RunFilteringExerciseAsync] - Press any key to finish filtering exercises..."); 
            Console.ReadKey();
            _logger.LogInformation("[FilterService.RunFilteringExerciseAsync] - Filtering exercises completed.");   
        }
    }
}