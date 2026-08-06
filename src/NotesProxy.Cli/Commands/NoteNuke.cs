using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public class NoteNuke : Command<NoteNuke.Settings>
{
    public class Settings : CommandSettings
    {
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (AnsiConsole.Confirm("[orange1]Are you sure you want to delete all notes? This cannot be undone.[/]"))
        {
            var notes = NoteManager.Instance.Notes.QueryDatabase();
            foreach (var note in notes)
            {
                NoteManager.Instance.Files.DeleteNote(note.Name);
            }
            NoteManager.Instance.Notes.DropNotes();
            AnsiConsole.MarkupLine("[green]Notes deleted.[/]");
        } else AnsiConsole.MarkupLine("[green]Crisis averted.[/]");

        return 0;
    }
}