using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using AIClientManager.Models;
using AIClientManager.Pdf;

namespace AIClientManager.Pdf;

public class DashboardReportDocument : IDocument
{
    private readonly int _total;
    private readonly int _high;
    private readonly int _medium;
    private readonly int _low;
    private readonly List<Client> _top;
    private readonly string _priorityChart;
    private readonly string _topChart;

    public DashboardReportDocument(
        int total,
        int high,
        int medium,
        int low,
        List<Client> top,
        string priorityChart,
        string topChart)
    {
        _total = total;
        _high = high;
        _medium = medium;
        _low = low;
        _top = top;
        _priorityChart = priorityChart;
        _topChart = topChart;
    }

    public DocumentMetadata GetMetadata() =>
        DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(30);

            page.Header().Text("AI Client Manager — Dashboard Report")
                .FontSize(22)
                .Bold();

            page.Content().Column(col =>
            {
                col.Spacing(15);

                col.Item().Text($"Generated: {DateTime.Now}");

                col.Item().Row(r =>
                {
                    r.RelativeItem().Text($"Total: {_total}");
                    r.RelativeItem().Text($"High: {_high}");
                    r.RelativeItem().Text($"Medium: {_medium}");
                    r.RelativeItem().Text($"Low: {_low}");
                });

                col.Item().Image(_priorityChart);

                col.Item().Text("Top Clients").FontSize(16).Bold();

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn();
                        c.RelativeColumn();
                        c.ConstantColumn(80);
                        c.ConstantColumn(80);
                    });

                    table.Header(h =>
                    {
                        h.Cell().Text("Name").Bold();
                        h.Cell().Text("Company").Bold();
                        h.Cell().Text("Score").Bold();
                        h.Cell().Text("Priority").Bold();
                    });

                    foreach (var c in _top)
                    {
                        table.Cell().Text(c.Name);
                        table.Cell().Text(c.Company);
                        table.Cell().Text(c.Score.ToString());
                        table.Cell().Text(c.Priority);
                    }
                });

                col.Item().Image(_topChart);
            });
        });
    }
}
