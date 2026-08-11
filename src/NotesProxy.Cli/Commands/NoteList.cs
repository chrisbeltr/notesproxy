using System.ComponentModel;
using NotesProxy.Tui;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace NotesProxy.Cli.Commands;

public class NoteList : Command<NoteList.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[category]")]
        [Description("Only return notes in a certain category.")]
        public string? Category { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        TuiManager.DisplayNotes("NOTES", NoteManager.Instance.Notes.GetSchema(), NoteManager.Instance.Notes.QueryDatabase(settings.Category));

        return 0;
    }
}