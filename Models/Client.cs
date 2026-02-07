
using System.ComponentModel.DataAnnotations;

namespace AIClientManager.Models
{
    public class Client
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Email { get; set; }
    public string? Phone { get; set; }

    public string Company { get; set; }
    public string Notes { get; set; }

    // 🔥 AI GENERATED DATA
    public string Priority { get; set; } = "Medium";

    public string Summary { get; set; }

    public string Keywords { get; set; } // stocké CSV ou JSON

    public double Score { get; set; }

    public DateTime AnalyzedAt { get; set; }
}

}
