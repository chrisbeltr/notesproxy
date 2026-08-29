using NotesProxy.Cli.Commands;

namespace NotesProxy.Cli;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (args[0] == "nls")
            {
                var notes = NoteManager.Instance.Notes.QueryDatabase();
                foreach (var note in notes)
                {
                    if (note.Name.Contains(' '))
                    {
                        Console.WriteLine($"\\\"{note.Name.Replace(" ", "\\ ")}\\\"");
                    }
                    else
                    {
                        Console.WriteLine(note.Name);
                    }
                }

                return;
            }
            if (args[0] == "cls")
            {
                var categories = NoteManager.Instance.Notes.GetCategories();
                foreach (var category in categories)
                {
                    if (category.Contains(' '))
                    {
                        Console.WriteLine($"\\\"{category.Replace(" ", "\\ ")}\\\"");
                    }
                    else
                    {
                        Console.WriteLine(category);
                    }
                }

                return;
            }
        }
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.SetHelpProvider(new NotesHelp(config.Settings));
            config.SetApplicationName("notesproxy");
            config.PropagateExceptions();
            
            config.AddBranch("note", note =>
            {
                note.SetDescription("Manage notes\n(Alias: n) (Default: create)");
                note.SetDefaultCommand<NoteCreate>();
                
                note.AddCommand<NoteCreate>("create").WithAlias("c").WithDescription("Create a new note\n(Alias: c)");
                note.AddCommand<NoteDelete>("delete").WithAlias("d").WithDescription("Delete a note\n(Alias: d)");
                note.AddCommand<NoteList>("list").WithAlias("ls").WithDescription("List all notes\n(Alias: ls)");
                note.AddCommand<NoteEdit>("edit").WithAlias("e").WithDescription("Edit a note\n(Alias: e)");
                note.AddCommand<NoteOpen>("open").WithAlias("o").WithDescription("Open a note\n(Alias: o)");
                note.AddCommand<NoteNuke>("nuke").WithAlias("n").WithDescription("Delete all currently stored notes.\n(Alias: n)");
            }).WithAlias("n");
            config.AddBranch("config", noteConfig =>
            {
                noteConfig.SetDescription("Manage configuration options\n(Alias: c) (Default: list)");
                noteConfig.SetDefaultCommand<ConfigList>();
                
                noteConfig.AddCommand<ConfigEdit>("edit").WithAlias("e").WithDescription("Edit the configuration options.\n(Alias: e)");
                noteConfig.AddCommand<ConfigList>("list").WithAlias("ls").WithDescription("List the configuration options and their current values\n(Alias: ls)");
            }).WithAlias("c");
            config.AddCommand<Interactive>("interactive").WithAlias("i").WithDescription("Launch interactive TUI\n(Alias: i)");
        });
        app.Run(args);
    }
}