## Automatic git commit messages

Installation guide:

This will enable the command: `git ac` for git auto commit.
The token is your OpenAI api key.

```
dotnet install -g SublimeCommit
SublimeCommit install --token my_token_1234
```

You can change the api key at any time:

```
SublimeCommit token my_token_5678
```

## Note for windows:

The install command only works on unix based systems. 
If you are on windows, you can still use the command "SublimeCommit" instead of "git ac".