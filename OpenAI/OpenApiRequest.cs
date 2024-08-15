using Newtonsoft.Json;

namespace Sublime.Commit.OpenAI;

public record CommitMessage(
    [property: JsonProperty("type")] string Type
);

public record Commits(
    [property: JsonProperty("type")] string Type,
    [property: JsonProperty("items")] Items Items
);

public record Files(
    [property: JsonProperty("type")] string Type,
    [property: JsonProperty("items")] Items Items
);

public record Items(
    [property: JsonProperty("type")] string Type,
    [property: JsonProperty("properties")] Properties Properties,
    [property: JsonProperty("required")] IReadOnlyList<string> Required,
    [property: JsonProperty("additionalProperties")] bool AdditionalProperties
);

public record JsonSchema(
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("schema")] Schema Schema,
    [property: JsonProperty("strict")] bool Strict
);

public record RequestMessage(
    [property: JsonProperty("role")] string Role,
    [property: JsonProperty("content")] string Content
);

public record Properties(
    [property: JsonProperty("commits")] Commits Commits,
    [property: JsonProperty("commit_message")] CommitMessage CommitMessage,
    [property: JsonProperty("files")] Files Files
);

public record ResponseFormat(
    [property: JsonProperty("type")] string Type,
    [property: JsonProperty("json_schema")] JsonSchema JsonSchema
);

public record OpenApiRequest(
    [property: JsonProperty("model")] string Model,
    [property: JsonProperty("messages")] IReadOnlyList<RequestMessage> Messages,
    [property: JsonProperty("response_format")] ResponseFormat ResponseFormat
);

public record Schema(
    [property: JsonProperty("type")] string Type,
    [property: JsonProperty("properties")] Properties Properties,
    [property: JsonProperty("required")] IReadOnlyList<string> Required,
    [property: JsonProperty("additionalProperties")] bool AdditionalProperties
);

