using Newtonsoft.Json.Linq;

namespace Sublime.Commit.Commands;

public static class ApiKeyCommand {
    public static void SaveApiKey(string apiKey)
    {
        string filePath = GetSettingsFilePath();

        JObject settings = new()
        {
            ["apiKey"] = apiKey
        };
        File.WriteAllText(filePath, settings.ToString());
    }

    static string GetSettingsFilePath()
    {
        // Use the ApplicationData folder for storing settings
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string settingsDirectory = Path.Combine(appDataPath, "SublimeCommit");

        // Ensure the directory exists
        if (!Directory.Exists(settingsDirectory))
            Directory.CreateDirectory(settingsDirectory);

        return Path.Combine(settingsDirectory, "settings.json");
    }

    static string? PromptForApiKey()
    {
        Console.WriteLine("Enter your OpenAI API key:");
        return Console.ReadLine()?.Trim();
    }

    public static string? LoadApiKey()
    {
        // Determine the settings file path using ApplicationData folder
        var settingsFilePath = GetSettingsFilePath();

        // Check if the settings file exists
        string? apiKey = null;
        if (File.Exists(settingsFilePath))
        {
            // Load the API key from the settings file
            JObject settings = JObject.Parse(File.ReadAllText(settingsFilePath));
            apiKey = settings["apiKey"]?.ToString();

            if (string.IsNullOrEmpty(apiKey))
            {
                // If the API key is not found in the file, prompt for it
                apiKey = PromptForApiKey();
                if (apiKey is null) return null;
                SaveApiKey(apiKey);
            }
        }
        else
        {
            // If the file doesn't exist, prompt the user for the API key and save it
            apiKey = PromptForApiKey();
            if (apiKey is null) return null;
            SaveApiKey(apiKey);
        }

        return apiKey;
    }
}