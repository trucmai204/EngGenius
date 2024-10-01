using GenAI.Enum;
using GenAI.ResponseDTO;
using Newtonsoft.Json;
using System.Text;

namespace GenAI
{
    public class Generator
    {
        public string ApiKey { get; set; }

        public async Task<string> GenerateContent(string userInput, bool useJsonForOutput = false, TemperatureEnum temperature = TemperatureEnum.Medium)
        {
            var apiEndpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-002:generateContent?key={ApiKey}";
            using (HttpClient client = new HttpClient())
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = userInput
                                }
                            }
                        }
                    },
                    safetySettings = new[]
                    {
                        new
                        {
                            category = "HARM_CATEGORY_DANGEROUS_CONTENT",
                            threshold = "BLOCK_NONE"
                        },
                        new
                        {
                            category = "HARM_CATEGORY_HARASSMENT",
                            threshold = "BLOCK_NONE"
                        },
                        new
                        {
                            category = "HARM_CATEGORY_HATE_SPEECH",
                            threshold = "BLOCK_NONE"
                        },
                        new
                        {
                            category = "HARM_CATEGORY_SEXUALLY_EXPLICIT",
                            threshold = "BLOCK_NONE"
                        }
                    },
                    generationConfig = new
                    {
                        temperature = (double)temperature / 100,
                        topP = 0.8,
                        topK = 10,
                        responseMimeType = useJsonForOutput ? "application/json" : "text/plain"
                    }
                };

                var body = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                var apiResponse = await client.PostAsync(apiEndpoint, body);
                apiResponse.EnsureSuccessStatusCode();

                var apiResponseContent = await apiResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<OneShotResponse.Root>(apiResponseContent);
                return dto.Candidates[0].Content.Parts[0].Text;
            }
        }
    }
}