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
            foreach (var _ in list[0]) grid.AddColumn(new GridColumn { Padding = new Padding(1, 0, 0, 0) });
            for (var i = 0; i < list.Count; i++)
            {
                var note = list[i];

                grid.AddRow([
                    new Text($"{i + 1}."),
                    GetNoteName(note),
                    GetNoteLocation(note),
                    GetNoteEditor(note),
                    GetNoteCategory(note)
                ]);
            }
        }

        AnsiConsole.Write(grid);

        return 0;
    }

    private Text GetNoteName(List<string?> note)
    {
        return new Text($"\"{note[0]}\"");
    }

    private Text GetNoteLocation(List<string?> note)
    {
        return new Text($"\"{note[1]}\"");
    }

    private Text GetNoteEditor(List<string?> note)
    {
        return new Text($"\"{note[2] ?? NoteManager.Instance.Config.GetEditor()}\"");
    }

    private Text GetNoteCategory(List<string?> note)
    {
        return new Text($"\"{note[3] ?? NoteManager.Instance.Config.GetCategory()}\"");
    }
}