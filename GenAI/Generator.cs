using GenAI.Enum;
using GenAI.ResponseDTO;
using Newtonsoft.Json;
using System.Text;

namespace GenAI
{
    public class Generator
    {
        public string ApiKey { get; set; }

        public async Task<string> Generate(string userInput, TemperatureEnum temperature = TemperatureEnum.Medium)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new
                            {
                                text = userInput
                            }
                        }
                    },
                },
                    generationConfig = new
                    {
                        temperature = (double)temperature / 100,
                        responseMimeType = "text/plain"
                    }
                };

                string jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-002:generateContent?key={ApiKey}", content);

                response.EnsureSuccessStatusCode();

                var res = await response.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<OneShotResponse.Root>(res);
                return dto.Candidates[0].Content.Parts[0].Text;
            }
        }
    }
}
