using System.Buffers;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public partial class NoteCreate : Command<NoteCreate.Settings>
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
        
        [CommandOption("-a|--autoopen")]
        [Description("The setting that decides whether notes will automatically open when using the create command")]
        public bool? AutoOpen { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (settings.Name != null && AlphanumCheck().IsMatch(settings.Name))
            throw new Exception("Please change note name, supported characters are [a-zA-Z0-9 -_.]");
        if (settings.Category != null && AlphanumCheck().IsMatch(settings.Category))
            throw new Exception("Please change category name, supported characters are [a-zA-Z0-9 -_.]");
        var noteName = settings.Name ?? $"note-{DateTime.Now:M-d-yy-HHmmss}";
        var noteLocation = settings.Location ?? NoteManager.Instance.Config.GetLocation();
        if (noteLocation == "")
            noteLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NotesProxy", "notes");

        NoteManager.Instance.Files.CreateNote(noteName, noteLocation);

        NoteManager.Instance.Notes.InsertNote(new Note(
            noteName,
            noteLocation,
            settings.Editor ?? NoteManager.Instance.Config.GetEditor(),
            settings.Category ?? NoteManager.Instance.Config.GetCategory()
        ));
        
        if (NoteManager.Instance.Config.GetAutoOpen() || (settings.AutoOpen.HasValue && settings.AutoOpen.Value))
            NoteManager.Instance.Files.OpenNote(noteName);

        if (settings.Name == null)
        {
            var prompt = new TextPrompt<string?>($"[orange1]Would you like to rename the note?[/] [orange1 italic](Press enter to keep it as \"{noteName}\")[/]").DefaultValue(null).ShowDefaultValue(false);
            var newName = AnsiConsole.Prompt(prompt);
            if (newName != null)
            {
                var oldNote = NoteManager.Instance.Notes.GetNote(noteName);
                var newNote = oldNote with { Name = newName };
                NoteManager.Instance.Files.MoveNote(noteName, newName);
                NoteManager.Instance.Notes.UpdateNote(noteName, newNote);
                noteName = newName;
            }
        }

        AnsiConsole.MarkupLine($"[green]Successfully created note [/][yellow]\"{noteName}\"[/][green].[/]");

        return 0;
    }

    [GeneratedRegex(@"[^a-zA-Z0-9 \-_\.]*")]
    private static partial Regex AlphanumCheck();
}