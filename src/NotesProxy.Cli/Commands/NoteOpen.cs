using System.ComponentModel;
using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public class NoteOpen : Command<NoteOpen.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")]
        [Description("The name of the note")]
        public required string Name { get; set; }
        
        [CommandOption("-e|--editor")]
        [Description("The editor to open this note with")]
        public string? Editor { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        NoteManager.Instance.Files.OpenNote(settings.Name,  settings.Editor);
        
        return 0;
    }
}