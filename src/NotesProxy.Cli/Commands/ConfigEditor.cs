using System.ComponentModel;
using Spectre.Console.Cli;

namespace NotesProxy.Cli.Commands;

public class ConfigEditor : Command<ConfigEditor.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<command>")]
        [Description("The command to run the default editor")]
        public string? Command { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}