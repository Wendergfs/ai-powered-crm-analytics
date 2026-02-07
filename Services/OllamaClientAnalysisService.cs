using System.Net.Http.Json;
using System.Text.Json;
using AIClientManager.DTOs;
using Microsoft.Extensions.Configuration;

namespace AIClientManager.Services
{
    public class OllamaClientAnalysisService : IClientAnalysisService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public OllamaClientAnalysisService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public ClientAnalysisResult Analyze(string notes)
        {
            try
            {
                var baseUrl = _config["Ollama:BaseUrl"];
                var model = _config["Ollama:Model"];

                var prompt =
                    "You are an AI CRM assistant.\n\n" +
                    "Analyze the following client notes and return STRICT JSON:\n\n" +
                    "{\n" +
                    "  \"priority\": \"High|Medium|Low\",\n" +
                    "  \"score\": number between 0 and 100,\n" +
                    "  \"summary\": \"...\",\n" +
                    "  \"keywords\": \"comma,separated,keywords\"\n" +
                    "}\n\n" +
                    "NOTES:\n" +
                    notes;

                var payload = new
                {
                    model = model,
                    prompt = prompt,
                    stream = false
                };

                var response = _http
                    .PostAsJsonAsync($"{baseUrl}/api/generate", payload)
                    .Result;

                if (!response.IsSuccessStatusCode)
                    return Fallback(notes);

                var raw = response.Content
                    .ReadFromJsonAsync<JsonElement>()
                    .Result;

                if (!raw.TryGetProperty("response", out var content))
                    return Fallback(notes);

                var text = content.GetString();

                Console.WriteLine("OLLAMA RAW TEXT:");
                Console.WriteLine(text);

                if (string.IsNullOrWhiteSpace(text))
                    return Fallback(notes);

                // 🔥 Extract JSON even if model talks
                var start = text.IndexOf('{');
                var end = text.LastIndexOf('}');

                if (start >= 0 && end > start)
                {
                    text = text.Substring(start, end - start + 1);
                }
                else
                {
                    Console.WriteLine("NO JSON FOUND — FALLBACK");
                    return Fallback(notes);
                }

                var result = JsonSerializer.Deserialize<ClientAnalysisResult>(
                    text,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (result == null)
                    return Fallback(notes);

                // flatten keywords array → string DB
                result.Keywords = result.Keywords
                    .Where(k => !string.IsNullOrWhiteSpace(k))
                    .Select(k => k.Trim())
                    .ToList();

                return result;

                }

            catch (Exception ex)
            {
                Console.WriteLine("OLLAMA ERROR: " + ex.Message);
                return Fallback(notes);
            }
        }

        private ClientAnalysisResult Fallback(string notes)
        {
            return new ClientAnalysisResult
            {
                Priority = "Low",
                Score = 10,
                Summary = "Fallback analysis used.",
                Keywords = new List<string>()
            };
        }
    }
}
