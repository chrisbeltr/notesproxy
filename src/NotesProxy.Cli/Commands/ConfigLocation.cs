using System.ComponentModel;
using Spectre.Console.Cli;

namespace NotesProxy.Cli.Commands;

public class ConfigLocation : Command<ConfigLocation.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<location>")]
        [Description("The default location for new notes")]
        public string? Command { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}