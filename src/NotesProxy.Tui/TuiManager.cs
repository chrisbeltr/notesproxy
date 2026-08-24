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

    private static void DisplayNote(Note note)
    {
        var table = new Table().Title("NOTE").HideHeaders();
        table.AddColumns(["key", "value"]);

        table.AddRow("Name", note.Name);
        table.AddRow("Location", note.Location);
        table.AddRow("Editor", note.Editor);
        table.AddRow("Category", note.Category);
        
        AnsiConsole.Write(table);
    }

    public static void MainMenu()
    {
        var shouldExit = false;
        while (!shouldExit)
        {
            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What would you like to do?")
                .AddChoices("Notes list", "New note", "Nuke notes", "Configuration", "Exit"));

            switch (choice)
            {
                case "Notes list":
                    NoteListMenu();
                    break;
                case "New note":
                    NewNoteMenu();
                    break;
                case "Nuke notes":
                    NukeConfirmation();
                    break;
                case "Configuration":
                    ConfigurationListMenu();
                    break;
                case "Exit":
                    shouldExit = true;
                    break;
            }
        }
    }

    private static void NoteListMenu()
    {
        var notes = NoteManager.Instance.Notes.QueryDatabase();
        if (notes.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]There are no notes.[/]");
            return;
        }
        var choice = AnsiConsole.Prompt(new SelectionPrompt<Note>().Title("Choose a note.")
            .UseConverter(note => $"{note.Name} ({note.Category})").AddChoices(notes)
            .EnableSearch().SearchPlaceholderText("Start typing to search for a note..."));
        NoteInteractionMenu(choice);
    }

    private static void NoteInteractionMenu(Note note)
    {
        var shouldExit = false;
        while (!shouldExit)
        {
            note = NoteManager.Instance.Notes.GetNote(note.Name);
            DisplayNote(note);
            
            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What would you like to do?")
                .AddChoices("Open", "Edit", "Delete", "Back"));
            switch (choice)
            {
                case "Open":
                    NoteManager.Instance.Files.OpenNote(note.Name);
                    break;
                case "Edit":
                    NoteEditMenu(note);
                    break;
                case "Delete":
                    if (AnsiConsole.Confirm("[orange1]Are you sure you want to delete this note? This cannot be undone.[/]"))
                    {
                        NoteManager.Instance.Files.DeleteNote(note.Name);
                        NoteManager.Instance.Notes.DeleteNote(note.Name);
                        AnsiConsole.MarkupLine(
                            $"[green]Successfully deleted note [/][yellow]\"{note.Name}\"[/][green].[/]");
                    }
                    else
                        AnsiConsole.MarkupLine("[green]Crisis averted.[/]");
                    shouldExit = true;
                    break;
                case "Back":
                    shouldExit = true;
                    break;
            }
        }
    }

    private static void NoteEditMenu(Note note)
    {
        var name = AnsiConsole.Prompt(new TextPrompt<string?>("New note name (leave blank to skip):").DefaultValue(null).HideDefaultValue());
        var location = AnsiConsole.Prompt(new TextPrompt<string?>("New note location (leave blank to skip):").DefaultValue(null).HideDefaultValue());
        var editor = AnsiConsole.Prompt(new TextPrompt<string?>("New note editor (leave blank to skip):").DefaultValue(null).HideDefaultValue());
        var category = AnsiConsole.Prompt(new TextPrompt<string?>("New note category (leave blank to skip):").DefaultValue(null).HideDefaultValue());
        
        NoteManager.Instance.Files.MoveNote(note.Name, name, location);

        NoteManager.Instance.Notes.UpdateNote(note.Name,
            new Note(name ?? note.Name, location ?? note.Location, editor ?? note.Editor, category ?? note.Category));
        AnsiConsole.MarkupLine($"[green]Successfully edited note [/][yellow]\"{note.Name}\"[/][green].[/]");
    }
    
    private static void NewNoteMenu()
    {
        var name = AnsiConsole.Prompt(new TextPrompt<string?>("New note name (leave blank for default):").DefaultValue(null).HideDefaultValue());
        var location = AnsiConsole.Prompt(new TextPrompt<string?>("New note location (leave blank for default):").DefaultValue(null).HideDefaultValue());
        var editor = AnsiConsole.Prompt(new TextPrompt<string?>("New note editor (leave blank for default):").DefaultValue(null).HideDefaultValue());
        var category = AnsiConsole.Prompt(new TextPrompt<string?>("New note category (leave blank for default):").DefaultValue(null).HideDefaultValue());
        
        var noteName = name ?? $"note-{DateTime.Now:M-d-yy-HHmmss}";
        var noteLocation = location ?? NoteManager.Instance.Config.GetLocation();
        if (noteLocation == "")
            noteLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NotesProxy", "notes");

        NoteManager.Instance.Files.CreateNote(noteName, noteLocation);

        NoteManager.Instance.Notes.InsertNote(new Note(
            noteName,
            noteLocation,
            editor ?? NoteManager.Instance.Config.GetEditor(),
            category ?? NoteManager.Instance.Config.GetCategory()
        ));
        
        if (NoteManager.Instance.Config.GetAutoOpen())
            NoteManager.Instance.Files.OpenNote(noteName);

        if (name == null)
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
    }

    private static void NukeConfirmation()
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
    }

    private static void ConfigurationListMenu()
    {
        var settings = NoteManager.Instance.Config.GetAllSettings().ToArray();
        var choice = AnsiConsole.Prompt(new SelectionPrompt<KeyValuePair<string, object>>().Title("Choose a setting to change.")
            .UseConverter(setting => $"{setting.Key}: {setting.Value}").AddChoices(settings)
            .EnableSearch().SearchPlaceholderText("Start typing to search for a setting..."));
        ConfigurationEditMenu(choice.Key);
    }

    private static void ConfigurationEditMenu(string setting)
    {
        switch (setting)
        {
            case "Editor":
                var newEditor = AnsiConsole.Prompt(new TextPrompt<string>($"Enter new value for setting \"{setting}\":"));
                NoteManager.Instance.Config.SetEditor(newEditor);
                break;
            case "Location":
                var newLocation = AnsiConsole.Prompt(new TextPrompt<string>($"Enter new value for setting \"{setting}\":"));
                NoteManager.Instance.Config.SetLocation(newLocation);
                break;
            case "Category":
                var newCategory = AnsiConsole.Prompt(new TextPrompt<string>($"Enter new value for setting \"{setting}\":"));
                NoteManager.Instance.Config.SetCategory(newCategory);
                break;
            case "Auto Open":
                var newAutoOpen = AnsiConsole.Prompt(new TextPrompt<bool>($"Enter new value for setting \"{setting}\":"));
                NoteManager.Instance.Config.SetAutoOpen(newAutoOpen);
                break;
            case "Server":
                var newServer = AnsiConsole.Prompt(new TextPrompt<string>($"Enter new value for setting \"{setting}\":"));
                NoteManager.Instance.Config.SetServer(newServer);
                break;
        }
        AnsiConsole.MarkupLine($"[green]Successfully changed configuration file.[/]");
    }
}