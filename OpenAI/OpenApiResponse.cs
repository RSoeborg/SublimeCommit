using Newtonsoft.Json;

namespace Sublime.Commit.OpenAI;

record Choice(
    [property: JsonProperty("index")] int Index,
    [property: JsonProperty("message")] Message Message,
    [property: JsonProperty("logprobs")] object Logprobs,
    [property: JsonProperty("finish_reason")] string FinishReason
);

record Message(
    [property: JsonProperty("role")] string Role,
    [property: JsonProperty("content")] string Content,
    [property: JsonProperty("refusal")] object Refusal
);

record OpenApiResponse(
    [property: JsonProperty("id")] string Id,
    [property: JsonProperty("object")] string Object,
    [property: JsonProperty("created")] int Created,
    [property: JsonProperty("model")] string Model,
    [property: JsonProperty("choices")] IReadOnlyList<Choice> Choices,
    [property: JsonProperty("usage")] Usage Usage,
    [property: JsonProperty("system_fingerprint")] string SystemFingerprint
);

record Usage(
    [property: JsonProperty("prompt_tokens")] int PromptTokens,
    [property: JsonProperty("completion_tokens")] int CompletionTokens,
    [property: JsonProperty("total_tokens")] int TotalTokens
);

