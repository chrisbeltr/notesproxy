using System.ComponentModel;
using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public class ConfigLocation : Command<ConfigLocation.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<location>")]
        [Description("The default location for new notes")]
        public required string Location { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        NoteManager.Instance.Config.SetLocation(settings.Location);
        AnsiConsole.MarkupLine($"[green]Successfully set default location to [/][yellow]\"{settings.Location}\"[/][green].[/]");

        return 0;
    }
}