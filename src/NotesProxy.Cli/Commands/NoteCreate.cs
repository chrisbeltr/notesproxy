using System.ComponentModel;
using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public class NoteCreate : Command<NoteCreate.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[name]")]
        [Description("The name of the note")]
        public string? Name { get; set; }

        [CommandOption("-l|--location")]
        [Description("The location of the note")]
        public string? Location { get; set; }

        [CommandOption("-e|--editor")]
        [Description("The editor overwrite for the note")]
        public string? Editor { get; set; }

        [CommandOption("-c|--category")]
        [Description("The category for the note")]
        public string? Category { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var noteName = settings.Name ?? $"note-{DateTime.Now:M-d-yy-HHmmss}";
        var noteLocation = settings.Location ?? NoteManager.Instance.Config.GetLocation();
        if (noteLocation == "")
            noteLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NotesProxy", "notes");

        NoteManager.Instance.Files.CreateNote(noteName, noteLocation);

        NoteManager.Instance.Notes.InsertNote([
            noteName,
            noteLocation,
            settings.Editor,
            settings.Category
        ]);
        
        NoteManager.Instance.Files.OpenNote(noteName);

        AnsiConsole.MarkupLine($"[green]Successfully created note [/][yellow]\"{noteName}\"[/][green].[/]");

        return 0;
    }
}