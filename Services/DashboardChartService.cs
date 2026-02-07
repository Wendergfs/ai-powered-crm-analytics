using AIClientManager.Data;
using Microsoft.EntityFrameworkCore;
using ScottPlot;

namespace AIClientManager.Services
{
    public class DashboardChartService
    {
        private readonly AppDbContext _context;

        public DashboardChartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GeneratePriorityChartAsync()
        {
            var high = await _context.Clients.CountAsync(c => c.Priority == "High");
            var medium = await _context.Clients.CountAsync(c => c.Priority == "Medium");
            var low = await _context.Clients.CountAsync(c => c.Priority == "Low");

            var plot = new Plot();

            double[] values = { high, medium, low };
            double[] positions = { 0, 1, 2 };
            string[] labels = { "High", "Medium", "Low" };

            plot.Add.Bars(positions, values);

            plot.Axes.Bottom.SetTicks(positions, labels);

            plot.Title("Client Priority Distribution");

            var path = Path.Combine(
                Path.GetTempPath(),
                $"priority_chart_{Guid.NewGuid()}.png");

            plot.SavePng(path, 600, 400);

            return path;
        }

        public async Task<string> GenerateTopScoresChartAsync()
        {
            var top = await _context.Clients
                .OrderByDescending(c => c.Score)
                .Take(5)
                .Select(c => new { c.Name, c.Score })
                .ToListAsync();

            var plot = new Plot();

            double[] values = top.Select(t => t.Score).ToArray();
            double[] positions = Enumerable.Range(0, values.Length)
                .Select(x => (double)x)
                .ToArray();

            string[] labels = top.Select(t => t.Name).ToArray();

            plot.Add.Bars(positions, values);

            plot.Axes.Bottom.SetTicks(positions, labels);

            plot.Title("Top Clients by Score");

            var path = Path.Combine(
                Path.GetTempPath(),
                $"top_scores_{Guid.NewGuid()}.png");

            plot.SavePng(path, 700, 400);

            return path;
        }
    }
}
