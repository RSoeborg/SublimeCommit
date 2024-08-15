using Newtonsoft.Json;

namespace Sublime.Commit.OpenAI;

public record Commit(
    [property: JsonProperty("commit_message")] string CommitMessage,
    [property: JsonProperty("files")] IReadOnlyList<string> Files
);

public record CommitSuggestions(
    [property: JsonProperty("commits")] IReadOnlyList<Commit> Commits
);
