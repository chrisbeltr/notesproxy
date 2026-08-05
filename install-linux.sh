dotnet publish src/NotesProxy.Cli -r linux-x64 -c Release -o src/NotesProxy.Cli/bin/publish
cp src/NotesProxy.Cli/bin/publish/notesproxy ~/.local/bin
cp ~/.local/bin/notesproxy ~/.local/bin/np
