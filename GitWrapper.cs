using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace Sublime.Commit;

public sealed class GitWrapper
{
    /// <summary>
    /// Runs the 'git diff --compact-summary' command and captures its output.
    /// </summary>
    /// <returns>A string containing the compact summary output of the 'git diff' command.</returns>
    public static string GetDiffCompactSummary() => GetDiff("--compact-summary");


    /// <summary>
    /// --diff-filter=[(A|C|D|M|R|T|U|X|B)…​[*]]
    /// Select only files that are Added (A), Copied (C), Deleted (D), Modified (M), Renamed (R), have their type (i.e. regular file, symlink, submodule, …​) changed (T), are Unmerged (U), are Unknown (X), or have had their pairing Broken (B). Any combination of the filter characters (including none) can be used. When * (All-or-none) is added to the combination, all paths are selected if there is any file that matches other criteria in the comparison; if there is no file that matches other criteria, nothing is selected.
    ///
    /// Also, these upper-case letters can be downcased to exclude. E.g. --diff-filter=ad excludes added and deleted paths.
    ///
    /// Note that not all diffs can feature all types. For instance, copied and renamed entries cannot appear if detection for those types is disabled.
    /// </summary>
    public enum DiffFilter
    {
        Added, Copied, Deleted, Modified, Renamed, TypeChanged, Unmerged, Unknown
    }

    static string GetDiffFilterVariable(DiffFilter filter) => filter switch {
        DiffFilter.Added => "A",
        DiffFilter.Copied => "C",
        DiffFilter.Deleted => "D",
        DiffFilter.Modified => "M",
        DiffFilter.Renamed => "R",
        DiffFilter.TypeChanged => "T",
        DiffFilter.Unmerged => "U",
        DiffFilter.Unknown => "X",
        _ => throw new ArgumentException("Invalid DiffFilter value", nameof(filter))
    };

    /// <summary>
    /// Runs the 'git diff --diff-filter=[filter]' command and captures its output.
    /// </summary>
    /// <param name="filter">The filter to apply to the 'git diff' command.</param>
    /// <returns>A string containing the output of the 'git diff' command with the specified filter.</returns>
    public static string GetDiffFiltered(DiffFilter filter) => 
        GetDiff($"--diff-filter={GetDiffFilterVariable(filter)} -U0");

    /// <summary>
    /// Runs the 'git diff' command and captures its output.
    /// </summary>
    /// <returns>A string containing the output of the 'git diff' command.</returns>
    static string GetDiff(string args)
    {
        var diffProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"diff {args}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        StringBuilder outputBuilder = new();

        diffProcess.OutputDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                outputBuilder.AppendLine(args.Data);
            }
        };

        diffProcess.ErrorDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                outputBuilder.AppendLine(args.Data);
            }
        };

        diffProcess.Start();
        diffProcess.BeginOutputReadLine();
        diffProcess.BeginErrorReadLine();
        diffProcess.WaitForExit();
        diffProcess.Close();

        return outputBuilder.ToString();
    }


    /// <summary>
    /// Runs the 'git add [file]' command.
    /// </summary>
    /// <param name="file">The file to add to the Git repository.</param>
    public static void AddCommitFile(string file)
    {
        var addProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"add {file}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        addProcess.OutputDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                Console.WriteLine(args.Data);
            }
        };

        addProcess.ErrorDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                Console.WriteLine(args.Data);
            }
        };

        addProcess.Start();
        addProcess.BeginOutputReadLine();
        addProcess.BeginErrorReadLine();
        addProcess.WaitForExit();
        addProcess.Close();
    }

    /// <summary>
    /// Runs the 'git status --porcelain' command and captures its output.
    /// </summary>
    /// <returns>A string containing the output of the 'git status --porcelain' command.</returns>
    public static string GetStatus() {
        var statusProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = "status --porcelain",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        StringBuilder outputBuilder = new();

        statusProcess.OutputDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                outputBuilder.AppendLine(args.Data);
            }
        };

        statusProcess.ErrorDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                outputBuilder.AppendLine(args.Data);
            }
        };

        statusProcess.Start();
        statusProcess.BeginOutputReadLine();
        statusProcess.BeginErrorReadLine();
        statusProcess.WaitForExit();
        statusProcess.Close();

        return outputBuilder.ToString();
    }


    /// <summary>
    /// Runs the 'git commit -m [message]' command.
    /// </summary>
    /// <param name="message">The commit message.</param>
    /// <returns>True if the commit was successful, false otherwise.</returns>
    public static bool CommitChanges(string message)
    {
        var commitProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"commit -m \"{message}\" --no-verify",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        StringBuilder outputBuilder = new();

        commitProcess.OutputDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                outputBuilder.AppendLine(args.Data);
            }
        };

        commitProcess.ErrorDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                outputBuilder.AppendLine(args.Data);
            }
        };

        commitProcess.Start();
        commitProcess.BeginOutputReadLine();
        commitProcess.BeginErrorReadLine();
        commitProcess.WaitForExit();
        commitProcess.Close();

        return outputBuilder.ToString().Contains("Committed");
    }

    /// <summary>
    /// Checks if the current directory is a Git repository by examining the output of the 'git diff' command.
    /// </summary>
    /// <returns>True if the current directory is a Git repository, false otherwise.</returns>
    public static bool IsGitRepository()
    {
        var diffOutput = GetDiff("");
        if (diffOutput.StartsWith("warning: Not a git repository") || diffOutput.StartsWith("fatal: not a git repository"))
        {
            return false;
        }
        return true;
    }
}