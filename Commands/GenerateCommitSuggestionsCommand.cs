using Kurukuru;
using Newtonsoft.Json.Linq;
using Sublime.Commit.OpenAI;

namespace Sublime.Commit.Commands;

public static class GenerateCommitSuggestionsCommand
{
    public static async Task GenerateCommitSuggestions()
    {
        // load api key from saved settings
        string? apiKey = ApiKeyCommand.LoadApiKey();
        if (apiKey is null) return;

        if (!GitWrapper.IsGitRepository())
        {
            Console.WriteLine("Not a git repository");
            return;
        }

    redoMoreFiles:

        string compactSummary = GitWrapper.GetDiffCompactSummary();
        if (string.IsNullOrWhiteSpace(compactSummary))
        {
            Console.WriteLine("No changes detected");
            return;
        }
        string editSummary = GitWrapper.GetDiffFiltered(GitWrapper.DiffFilter.Modified);

        var httpClient = new HttpClient();
        var openAi = new OpenAiWrapper(httpClient, apiKey);

        bool skipped = false;
        await Spinner.StartAsync("Generating suggestions ...", async (spinner) =>
        {
            var suggestions = await openAi.GetCommitSuggestionsAsync(
                compactSummary,
                editSummary
            );

            if (suggestions is null)
            {
                spinner.Fail();
                Console.WriteLine("Failed to automatically get commit suggestions.");
                return;
            }

            spinner.Succeed();

            foreach (var commit in suggestions.Commits)
            {
                Console.WriteLine($"Commit message: {commit.CommitMessage}");
                Console.WriteLine("Files:");
                foreach (var file in commit.Files)
                {
                    Console.WriteLine($"- {file}");
                }
                Console.WriteLine();

                // ask for confirmation
                Console.Write("Commit this? (Y/n) ");
                var response = Console.ReadKey();

                if (response.Key != ConsoleKey.Y && response.Key != ConsoleKey.Enter)
                {
                    Console.WriteLine("\nSkipping commit");
                    skipped = true;
                    continue;
                }
                Console.WriteLine();

                foreach (var file in commit.Files) GitWrapper.AddCommitFile(file);
                GitWrapper.CommitChanges(commit.CommitMessage);
            }
        });

        if (!string.IsNullOrWhiteSpace(GitWrapper.GetStatus()) && !skipped)
            goto redoMoreFiles;
    }


    public static async Task GenerateStagedCommitSuggestion()
    {
        string? apiKey = ApiKeyCommand.LoadApiKey();
        if (apiKey is null) return;

        string compactSummary = GitWrapper.GetDiffCompactSummary(staged: true);
        if (string.IsNullOrWhiteSpace(compactSummary))
        {
            Console.WriteLine("No changes detected");
            return;
        }
        string editSummary = GitWrapper.GetDiffFiltered(GitWrapper.DiffFilter.Modified);

        var httpClient = new HttpClient();
        var openAi = new OpenAiWrapper(httpClient, apiKey);

        await Spinner.StartAsync("Generating suggestion ...", async (spinner) =>
        {
            var commit = await openAi.GetStagedCommitSuggestionsAsync(
                compactSummary,
                editSummary
            );

            if (commit is null)
            {
                spinner.Fail();
                Console.WriteLine("Failed to automatically get commit suggestions.");
                return;
            }

            spinner.Succeed();

            Console.WriteLine($"Commit message: {commit.CommitMessage}");
            // ask for confirmation
            Console.Write("Commit this? (Y/n) ");
            var response = Console.ReadKey();

            if (response.Key != ConsoleKey.Y && response.Key != ConsoleKey.Enter)
            {
                return;
            }
            Console.WriteLine();
            GitWrapper.CommitChanges(commit.CommitMessage);
        });

    }
}