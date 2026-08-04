using System.ComponentModel;
using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public class ConfigCategory : Command<ConfigCategory.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<category>")]
        [Description("The default category for notes")]
        public required string Category { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        NoteManager.Instance.Config.SetCategory(settings.Category);
        AnsiConsole.MarkupLine($"[green]Successfully set the default category to [/][yellow]\"{settings.Category}\"[/][green].[/]");

        return 0;
    }
}