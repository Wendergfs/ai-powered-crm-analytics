using Microsoft.AspNetCore.Mvc;
using AIClientManager.Data;
using System.Linq;
using QuestPDF.Fluent;
using Microsoft.AspNetCore.Authorization;
using AIClientManager.Services;
using AIClientManager.Pdf;
using Microsoft.EntityFrameworkCore;

namespace AIClientManager.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        

        public IActionResult Index()
        {
            ViewBag.Total = _context.Clients.Count();
            ViewBag.High = _context.Clients.Count(c => c.Priority == "High");
            ViewBag.Medium = _context.Clients.Count(c => c.Priority == "Medium");
            ViewBag.Low = _context.Clients.Count(c => c.Priority == "Low");

            var lastClients = _context.Clients
                .OrderByDescending(c => c.Id)
                .Take(5)
                .ToList();

            return View(lastClients);
        }
        [HttpGet]
        public IActionResult Stats()
        {
            var total = _context.Clients.Count();

            var high = _context.Clients.Count(c => c.Priority == "High");
            var medium = _context.Clients.Count(c => c.Priority == "Medium");
            var low = _context.Clients.Count(c => c.Priority == "Low");

            var scores = _context.Clients
                .Select(c => c.Score)
                .ToList();

            return Json(new
            {
                total,
                priorities = new { high, medium, low },
                scores
            });
        }

        public async Task<IActionResult> ExportDashboardPdf()
        {
            var total = await _context.Clients.CountAsync();
            var high = await _context.Clients.CountAsync(c => c.Priority == "High");
            var medium = await _context.Clients.CountAsync(c => c.Priority == "Medium");
            var low = await _context.Clients.CountAsync(c => c.Priority == "Low");

            var topClients = await _context.Clients
                .OrderByDescending(c => c.Score)
                .Take(5)
                .ToListAsync();

            // 📊 generate charts
            var priorityChart = await _charts.GeneratePriorityChartAsync();
            var topChart = await _charts.GenerateTopScoresChartAsync();

            var doc = new DashboardReportDocument(
                total,
                high,
                medium,
                low,
                topClients,
                priorityChart,
                topChart);

            var pdf = doc.GeneratePdf();

            return File(pdf, "application/pdf", "AIClientManager_Dashboard.pdf");
        }

        private readonly DashboardChartService _charts;

        public DashboardController(
            AppDbContext context,
            DashboardChartService charts)
        {
            _context = context;
            _charts = charts;
        }

        
    }
}
