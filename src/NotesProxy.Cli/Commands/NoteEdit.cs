using System.ComponentModel;
using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public class NoteEdit : Command<NoteEdit.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")]
        [Description("The name of the note to edit")]
        public required string NoteName { get; set; }

        [CommandOption("-n|--name")]
        [Description("The new name of the note")]
        public string? NewName { get; set; }

        [CommandOption("-l|--location")]
        [Description("The new location of the note")]
        public string? NewLocation { get; set; }

        [CommandOption("-e|--editor")]
        [Description("The new editor for the note")]
        public string? NewEditor { get; set; }

        [CommandOption("-c|--category")]
        [Description("The new category for the note")]
        public string? NewCategory { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (!NoteManager.Instance.Notes.NoteExists(settings.NoteName)) throw new Exception("Note does not exist.");
        var oldNote = NoteManager.Instance.Notes.GetNote(settings.NoteName);

        NoteManager.Instance.Files.MoveNote(settings.NoteName, settings.NewName, settings.NewLocation);

        NoteManager.Instance.Notes.UpdateNote(settings.NoteName,
            new Note(settings.NewName ?? oldNote.Name, settings.NewLocation ?? oldNote.Location,
                settings.NewEditor ?? oldNote.Editor, settings.NewCategory ?? oldNote.Category));
        AnsiConsole.MarkupLine($"[green]Successfully edited note [/][yellow]\"{settings.NoteName}\"[/][green].[/]");

        return 0;
    }
}