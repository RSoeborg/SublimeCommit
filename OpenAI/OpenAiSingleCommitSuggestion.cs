using Newtonsoft.Json;

namespace Sublime.Commit.OpenAI;

public record SingleCommitSuggestion(
    [property: JsonProperty("commit_message")] string CommitMessage
);
