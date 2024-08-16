dotnet tool uninstall SublimeCommit --global
dotnet tool install --global --add-source ./nupkg SublimeCommit

# Pushing:
# dotnet nuget push nupkg/SublimeCommit.1.0.3.nupkg --api-key HERE --source https://api.nuget.org/v3/index.json