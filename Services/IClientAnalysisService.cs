using AIClientManager.DTOs;

namespace AIClientManager.Services
{
    public interface IClientAnalysisService
    {
        ClientAnalysisResult Analyze(string notes);
    }
}
