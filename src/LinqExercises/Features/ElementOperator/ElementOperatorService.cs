using LinqExercises.Shared.Data;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace LinqExercises.Features.ElementOperator
{
    public class ElementOperatorService : IElementOperator
    {
        private readonly ILogger<ElementOperatorService> _logger;
        private readonly ChinookDbContext _dbContext;

        public ElementOperatorService(ILogger<ElementOperatorService> logger, ChinookDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task RunElementOperatorExerciseAsync()
        {
            // Implementation goes here
            _logger.LogInformation("[ElementOperatorService.RunElementOperatorExerciseAsync] - Running element operator exercises...");
            await Task.CompletedTask;
            // First, FirstOrDefault, Single, SingleOrDefault, ElementAt, ElementAtOrDefault, Last, LastOrDefault examples would be implemented here
            var firstTrack = _dbContext.Tracks.First();
            var firstOrDefaultTrack = _dbContext.Tracks.FirstOrDefault(t => t.Composer == "NonExistentComposer");
            var singleTrack = _dbContext.Tracks.Single(t => t.TrackId == 1);
            var singleOrDefaultTrack = _dbContext.Tracks.SingleOrDefault(t => t.TrackId == 9999);
            var elementAtTrack = _dbContext.Tracks.ElementAt(5);
            var elementAtOrDefaultTrack = _dbContext.Tracks.Skip(9999).FirstOrDefault();
            var lastTrack = _dbContext.Tracks.OrderBy(t => t.TrackId).Last();
            var lastOrDefaultTrack = _dbContext.Tracks.OrderBy(t => t.TrackId).LastOrDefault(t => t.Composer == "NonExistentComposer");
            var elementOperatorTable = new Table().Title("[green]Element Operator Results[/]").Border(TableBorder.Rounded)
                .AddColumn("Operator")
                .AddColumn("Result");
            elementOperatorTable.AddRow("First Track", Markup.Escape(firstTrack.Name ?? "N/A"));
            elementOperatorTable.AddRow("FirstOrDefault Track (NonExistent)", Markup.Escape(firstOrDefaultTrack?.Name ?? "N/A"));
            elementOperatorTable.AddRow("Single Track", Markup.Escape(singleTrack.Name ?? "N/A"));
            elementOperatorTable.AddRow("SingleOrDefault Track (NonExistent)", Markup.Escape(singleOrDefaultTrack?.Name ?? "N/A"));
            elementOperatorTable.AddRow("ElementAt Track", Markup.Escape(elementAtTrack.Name ?? "N/A"));
            elementOperatorTable.AddRow("ElementAtOrDefault Track (NonExistent)", Markup.Escape(elementAtOrDefaultTrack?.Name ?? "N/A"));
            elementOperatorTable.AddRow("Last Track", Markup.Escape(lastTrack.Name ?? "N/A"));
            elementOperatorTable.AddRow("LastOrDefault Track (NonExistent)", Markup.Escape(lastOrDefaultTrack?.Name ?? "N/A"));   
            AnsiConsole.Write(elementOperatorTable);
            _logger.LogInformation("Press any key to finish the element operator exercises...");
            Console.ReadKey();
            _logger.LogInformation("[ElementOperatorService.RunElementOperatorExerciseAsync] - Completed element operator exercises.");
        }
    }
}