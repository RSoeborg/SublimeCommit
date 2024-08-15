// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;

namespace Sublime.Commit.OpenAI;

public record StagedCommitMessage(
        [property: JsonProperty("type")] string Type
    );

public record StagedJsonSchema(
    [property: JsonProperty("name")] string Name,
    [property: JsonProperty("schema")] StagedSchema Schema,
    [property: JsonProperty("strict")] bool Strict
);

public record StagedMessage(
    [property: JsonProperty("role")] string Role,
    [property: JsonProperty("content")] string Content
);

public record StagedProperties(
    [property: JsonProperty("commit_message")] StagedCommitMessage CommitMessage
);

public record StagedResponseFormat(
    [property: JsonProperty("type")] string Type,
    [property: JsonProperty("json_schema")] StagedJsonSchema JsonSchema
);

public record StagedOpenApiRequest(
    [property: JsonProperty("model")] string Model,
    [property: JsonProperty("messages")] IReadOnlyList<StagedMessage> Messages,
    [property: JsonProperty("response_format")] StagedResponseFormat ResponseFormat
);

public record StagedSchema(
    [property: JsonProperty("type")] string Type,
    [property: JsonProperty("properties")] StagedProperties Properties,
    [property: JsonProperty("required")] IReadOnlyList<string> Required,
    [property: JsonProperty("additionalProperties")] bool AdditionalProperties
);

