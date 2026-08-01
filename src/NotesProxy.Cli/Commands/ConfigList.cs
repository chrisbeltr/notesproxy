using System.ComponentModel;
using Spectre.Console.Cli;

namespace NotesProxy.Cli.Commands;

public class ConfigList : Command<ConfigList.Settings>
{
    public class Settings : CommandSettings
    {
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}