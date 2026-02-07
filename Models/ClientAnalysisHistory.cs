using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIClientManager.Models
{
    public class ClientAnalysisHistory
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;

        public double Score { get; set; }

        public string Priority { get; set; } = "";

        public string Summary { get; set; } = "";

        public string Keywords { get; set; } = "";

        public DateTime AnalyzedAt { get; set; }
    }
}
