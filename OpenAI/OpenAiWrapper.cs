using System.Text;
using Newtonsoft.Json;

namespace Sublime.Commit.OpenAI;

public class OpenAiWrapper {
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAiWrapper(HttpClient httpClient, string apiKey) {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<CommitSuggestions?> GetCommitSuggestionsAsync(string compactSummary, string editSummary, int maxSummaryLength = 6_000) {
        editSummary = editSummary.Length > maxSummaryLength ? editSummary[..maxSummaryLength] : editSummary;
        var commitPrompt = compactSummary + $"\n\n---- EDIT DIFF (Might be truncated) ----\n\n" + editSummary;

        var prompt = JsonConvert.DeserializeObject<OpenApiRequest>(
            EmbeddedPromptResources.StandardPrompt
        );
        if (prompt is null) return null;

        prompt = prompt with {
            Messages = [
                prompt.Messages[0],
                prompt.Messages[1] with {
                    Content = commitPrompt
                }
            ]
        };

        var serializedPrompt = JsonConvert.SerializeObject(prompt, new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore
        });

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
        request.Content = new StringContent(serializedPrompt, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        OpenApiResponse? openAiResponse = null;
        try {
            openAiResponse = JsonConvert.DeserializeObject<OpenApiResponse>(responseContent);
        } catch (Exception) {}

        if (openAiResponse is null) return null;

        var choice = openAiResponse.Choices.FirstOrDefault();
        if (choice is null) return null;

        try {
            return JsonConvert.DeserializeObject<CommitSuggestions>(choice.Message.Content);
        } catch (Exception) {
            return null;
        }
    }

}