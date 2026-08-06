using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace NotesProxy.Cli.Commands;

public class NoteList : Command<NoteList.Settings>
{
    public class Settings : CommandSettings
    {
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine("\n[yellow]NOTES:[/]");

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 0, 0, 0) });

        var list = NoteManager.Instance.Notes.QueryDatabase();
        if (list.Count > 0)
        {
            for (var i = 0; i < 5; i++) grid.AddColumn(new GridColumn { Padding = new Padding(1, 0, 0, 0) });
            for (var i = 0; i < list.Count; i++)
            {
                var note = list[i];
        
                grid.AddRow([
                    new Text($"{i + 1}."),
                    GetNoteName(note),
                    GetNoteLocation(note),
                    GetNoteEditor(note),
                    GetNoteCategory(note),
                    GetNoteAutoOpen(note),
                ]);
            }
        }

        AnsiConsole.Write(grid);

        return 0;
    }

    private Text GetNoteName(Note note)
    {
        return new Text($"\"{note.Name}\"");
    }

    private Text GetNoteLocation(Note note)
    {
        return new Text($"\"{note.Location}\"");
    }

    private Text GetNoteEditor(Note note)
    {
        return new Text($"\"{note.Editor}\"");
    }

    private Text GetNoteCategory(Note note)
    {
        return new Text($"\"{note.Category}\"");
    }

    private Text GetNoteAutoOpen(Note note)
    {
        return new Text($"{note.AutoOpen}");
    }
}