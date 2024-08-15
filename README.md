## Automatic git commit messages

Small command line tool, that helps you generate commits quickly. 
It chunks relevant files together, and tries to generate a commit message.

Installation guide (MacOs/Linux):

This will enable the command: `git ac` for git auto commit.
The token is your OpenAI api key.

```
dotnet tool install --global SublimeCommit
SublimeCommit install --token my_token_1234
```

You can change the api key at any time:

```
SublimeCommit token my_token_5678
```

## Commands

For the CLI to automatically figure out the commits and their messages, just use:

```
git ac
```

If you want to have more granular control, or want all files in a single commit - instead use:
```
git ac add .
```

or 
```
git ac add my/files
```

If you prefer to stage the files yourself, you can use
```
git add my/file
git add my/other/file
git ac --staged
```

## Note for windows:

The install command only works on unix based systems. 
If you are on windows, you can still use the command "SublimeCommit" instead of "git ac".