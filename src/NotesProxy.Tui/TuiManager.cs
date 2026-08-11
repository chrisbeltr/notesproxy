using NotesProxy.Manager;
using Spectre.Console;

namespace NotesProxy.Tui;

public static class TuiManager
{
    public static void DisplayNotes(string title, IEnumerable<string> schema, IEnumerable<Note> notes)
    {
        var table = new Table().Title(title).HeavyHeadBorder();

        foreach (var col in schema)
        {
            table.AddColumn(col);
        }

        foreach (var note in notes)
        {
            table.AddRow([note.Name, note.Location, note.Editor, note.Category]);
        }
        
        AnsiConsole.Write(table);
    }

    public static void DisplayConfig(string title, IDictionary<string, object> config)
    {
        var table = new Table().Title(title).HideHeaders();
        table.AddColumns(["key", "value"]);

        foreach (var key in config.Keys)
        {
            table.AddRow(key, $"{config[key]}");
        }
        
        AnsiConsole.Write(table);
    }
}