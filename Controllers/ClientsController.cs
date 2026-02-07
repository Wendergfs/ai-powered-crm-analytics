using Microsoft.AspNetCore.Mvc;
using AIClientManager.Data;
using AIClientManager.Models;
using Microsoft.EntityFrameworkCore;
using AIClientManager.Services;
using QuestPDF.Fluent;
using Microsoft.AspNetCore.Authorization;

namespace AIClientManager.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IClientAnalysisService _analysis;

        public ClientsController(AppDbContext context,
                                IClientAnalysisService analysis)
        {
            _context = context;
            _analysis = analysis;
        }

        // ======================
        // GET: Clients
        // ======================
        public async Task<IActionResult> Index(
            string? search,
            string? company,
            string? priority,
            int? minScore,
            int? maxScore)
        {
            var clients = _context.Clients.AsQueryable();

            // 🔎 basic search
            if (!string.IsNullOrWhiteSpace(search))
                clients = clients.Where(c =>
                    c.Name.Contains(search) ||
                    c.Company.Contains(search));

            // 🏢 company filter
            if (!string.IsNullOrWhiteSpace(company))
                clients = clients.Where(c =>
                    c.Company.Contains(company));

            // 🚦 priority filter
            if (!string.IsNullOrWhiteSpace(priority))
                clients = clients.Where(c =>
                    c.Priority == priority);

            // 📊 score range
            if (minScore.HasValue)
                clients = clients.Where(c =>
                    c.Score >= minScore.Value);

            if (maxScore.HasValue)
                clients = clients.Where(c =>
                    c.Score <= maxScore.Value);

            return View(await clients.ToListAsync());
        }

        // ======================
        // GET: Create
        // ======================
        public IActionResult Create()
        {
            return View();
        }

        // ======================
        // POST: Create
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(client);
            }

            var result = _analysis.Analyze(client.Notes);

            client.Priority = result.Priority;
            client.Score = result.Score;
            client.Summary = result.Summary;
            client.Keywords = string.Join(",", result.Keywords);
            client.AnalyzedAt = DateTime.Now;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // ======================
        // GET: Edit
        // ======================
    
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                return NotFound();

            return View(client);
        }




        // ======================
        // POST: Edit
        // ======================
        
        [HttpPost]
        public async Task<IActionResult> Edit(Client client)
        {
            var existing = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == client.Id);

            if (existing == null)
                return Content("CLIENT NOT FOUND");

            var result = _analysis.Analyze(client.Notes);

            existing.Name = client.Name;
            existing.Email = client.Email;
            existing.Company = client.Company;
            existing.Notes = client.Notes;
            existing.Priority = result.Priority;
            existing.Score = result.Score;
            existing.Summary = result.Summary;
            existing.Keywords = string.Join(",", result.Keywords);
            existing.AnalyzedAt = DateTime.Now;

            // 🔥 INSERT HISTORY ICI
            _context.ClientAnalysisHistories.Add(new ClientAnalysisHistory
            {
                ClientId = existing.Id,
                Priority = existing.Priority,
                Score = existing.Score,
                Summary = existing.Summary,
                Keywords = existing.Keywords,
                AnalyzedAt = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }






        // ======================
        // DELETE
        // ======================
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return NotFound();

            return View(client);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ReAnalyzeAll()
        {
            var clients = await _context.Clients.ToListAsync();

            foreach (var client in clients)
            {
                var result = _analysis.Analyze(client.Notes);

                client.Priority = result.Priority;
                client.Score = result.Score;
                client.Summary = result.Summary;
                client.Keywords = string.Join(",", result.Keywords);
                client.AnalyzedAt = DateTime.Now;
                
                _context.ClientAnalysisHistories.Add(new ClientAnalysisHistory
                {
                    ClientId = client.Id,
                    Priority = client.Priority,
                    Score = client.Score,
                    Summary = client.Summary,
                    Keywords = client.Keywords,
                    AnalyzedAt = DateTime.Now
                });
            }
            
            await _context.SaveChangesAsync();

            TempData["Success"] = "All clients have been re-analyzed with LLM successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportPdf(int id)
            {
                var client = await _context.Clients.FindAsync(id);

                if (client == null)
                    return NotFound();

                var doc = new Pdf.ClientReportDocument(client);

                var pdf = doc.GeneratePdf();

                return File(pdf, "application/pdf",
                    $"Client_{client.Name}.pdf");
            }
            public async Task<IActionResult> History(int id)
            {
                var client = await _context.Clients.FindAsync(id);

                if (client == null)
                    return NotFound();

                var history = await _context.ClientAnalysisHistories
                    .Where(h => h.ClientId == id)
                    .OrderBy(h => h.AnalyzedAt)
                    .ToListAsync();

                ViewBag.Client = client;

                return View(history);
            }


    }
}
