using System.ComponentModel;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public partial class ConfigEdit : Command<ConfigEdit.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-l|--location")]
        [Description("The default location for new notes")]
        public string? Location { get; set; }

        [CommandOption("-e|--editor")]
        [Description("The command to run the default editor")]
        public string? Editor { get; set; }

        [CommandOption("-c|--category")]
        [Description("The default category for notes")]
        public string? Category { get; set; }

        [CommandOption("-a|--autoopen")]
        [Description("The setting that decides whether notes will automatically open when using the create command")]
        public bool? AutoOpen { get; set; }
        
        [CommandOption("-s|--server")]
        [Description("The remote NotesProxy server URL")]
        public string? Server { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (settings.Category != null && AlphanumCheck().IsMatch(settings.Category))
            throw new Exception("Please change category name, supported characters are [a-zA-Z0-9 -_.]");
        if (settings.Location != null) NoteManager.Instance.Config.SetLocation(settings.Location);
        if (settings.Editor != null) NoteManager.Instance.Config.SetEditor(settings.Editor);
        if (settings.Category != null) NoteManager.Instance.Config.SetCategory(settings.Category);
        if (settings.AutoOpen.HasValue) NoteManager.Instance.Config.SetAutoOpen(settings.AutoOpen.Value);
        if (settings.Server != null) NoteManager.Instance.Config.SetServer(settings.Server);
        
        AnsiConsole.MarkupLine(
            $"[green]Successfully changed configuration file.[/]");

        return 0;
    }
    
    [GeneratedRegex(@"[^a-zA-Z0-9 \-_\.]*")]
    private static partial Regex AlphanumCheck();
}