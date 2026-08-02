using System.ComponentModel;
using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public class ConfigEditor : Command<ConfigEditor.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<command>")]
        [Description("The command to run the default editor")]
        public required string Command { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        NoteManager.Instance.Config.SetEditor(settings.Command);
        AnsiConsole.MarkupLine($"[green]Successfully set the editor to [/][yellow]\"{settings.Command}\"[/][green].[/]");

        return 0;
    }
}