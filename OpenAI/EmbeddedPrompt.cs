namespace Sublime.Commit.OpenAI;

public static class EmbeddedPromptResources
{
    public static string StandardPrompt =
    """
    {
        "model": "gpt-4o-2024-08-06",
        "messages": [
            {
            "role": "system",
            "content": "You are a git expert. You are given a compact-summary and summary of edited files using git diff. Generate the correct commit messages for files. For some commits it makes sense to use multiple files for a single commit. A commit must start with build, chore, ci, feat, fix, refactor, revert, test, merge"
            },
            {
            "role": "user",
            "content": ""
            }
        ],
        "response_format": {
            "type": "json_schema",
            "json_schema": {
            "name": "git_commits",
            "schema": {
                "type": "object",
                "properties": {
                "commits": {
                    "type": "array",
                    "items": {
                    "type": "object",
                    "properties": {
                        "commit_message": {
                        "type": "string"
                        },
                        "files": {
                        "type": "array",
                        "items": {
                            "type": "string"
                        }
                        }
                    },
                    "required": [
                        "commit_message",
                        "files"
                    ],
                    "additionalProperties": false
                    }
                }
                },
                "required": [
                "commits"
                ],
                "additionalProperties": false
            },
            "strict": true
            }
        }
    }
    """;
}