namespace AIClientManager.DTOs
{
    public class ClientAnalysisResult
    {
        public string Priority { get; set; }
        public double Score { get; set; }
        public string Summary { get; set; }
        public List<string> Keywords { get; set; } = new();

    }
}
