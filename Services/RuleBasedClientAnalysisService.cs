using AIClientManager.DTOs;

namespace AIClientManager.Services
{
    public class RuleBasedClientAnalysisService : IClientAnalysisService
    {
        public ClientAnalysisResult Analyze(string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
            {
                return new ClientAnalysisResult
                {
                    Priority = "Low",
                    Score = 10,
                    Summary = "No client notes provided.",
                    Keywords = new List<string>()
                };
            }

            notes = notes.ToLower();
            int score = 0;

            var hits = new List<string>();

            if (notes.Contains("urgent")) { score += 30; hits.Add("urgent"); }
            if (notes.Contains("contract")) { score += 30; hits.Add("contract"); }
            if (notes.Contains("meeting")) { score += 20; hits.Add("meeting"); }
            if (notes.Contains("email")) { score += 10; hits.Add("email"); }

            string priority =
                score >= 70 ? "High" :
                score >= 40 ? "Medium" :
                "Low";

            return new ClientAnalysisResult
            {
                Priority = priority,
                Score = score,
                Summary = $"Rule-based analysis detected {priority.ToLower()} priority signals.",
                Keywords = hits
            };
        }
    }
}
