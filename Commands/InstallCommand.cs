using System.Diagnostics;
using System.Text;

namespace Sublime.Commit.Commands;

public static class InstallCommand
{
    public static async Task InstallGitCommand()
    {
        // check if folder exists "/usr/local/bin"
        if (!Directory.Exists("/usr/local/bin"))
        {
            Console.WriteLine("Only works on unix systems.");
        }

        StringBuilder shellScript = new();

        shellScript.AppendLine("#!/bin/bash");
        shellScript.AppendLine("echo '#!/bin/sh\nsublimecommit' > /usr/local/bin/git-ac");
        shellScript.AppendLine("chmod +x /usr/local/bin/git-ac");
        shellScript.AppendLine("echo ' alias created successfully: git ac'");

        // Write the shell script to a temporary file
        string tempFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".sh");
        File.WriteAllText(tempFileName, shellScript.ToString());

        // Wait for file to be written
        while (!File.Exists(tempFileName))
        {
            await Task.Delay(100);
        }

        try
        {
            // Execute the shell script with elevated permissions
            var startInfo = new ProcessStartInfo
            {
                FileName = "sudo",
                Arguments = "bash " + tempFileName,
                WorkingDirectory = Path.GetDirectoryName(tempFileName),
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = true,
                CreateNoWindow = true
            };

            Process? process = Process.Start(startInfo);
            process?.WaitForExit();
        }
        finally
        {
            // Clean up the temporary file
            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
        }
    }
}