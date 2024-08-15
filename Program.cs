using Cocona;
using Sublime.Commit.Commands;

var builder = CoconaApp.CreateBuilder();
var app = builder.Build();

// Default AI commit generation
app.AddCommand(async ([Option] string? files) => {
    if (string.IsNullOrWhiteSpace(files))
        await GenerateCommitSuggestionsCommand.GenerateCommitSuggestions();

    



});

// Set the OpenAI API token
app.AddCommand("token", (string token) => ApiKeyCommand.SaveApiKey(token))
   .WithDescription("Set the OpenAI API token.");

// Initialize the application
app.AddCommand("install", 
    async ([Option("token")] string? token) => {
        if (!string.IsNullOrWhiteSpace(token)) ApiKeyCommand.SaveApiKey(token);
        await InstallCommand.InstallGitCommand();
    }
)
.WithDescription("Creates git alias command 'git ac' for sublime-commit. Must be run with elevated permissions.");

await app.RunAsync();