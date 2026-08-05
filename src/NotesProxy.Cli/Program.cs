using NotesProxy.Cli.Commands;

namespace NotesProxy.Cli;

class Program
{
    static void Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.SetHelpProvider(new NotesHelp(config.Settings));
            config.SetApplicationName("notesproxy");
            
            config.AddBranch("note", note =>
            {
                note.SetDescription("Manage notes\n(Alias: n) (Default: create)");
                note.SetDefaultCommand<NoteCreate>();
                
                note.AddCommand<NoteCreate>("create").WithAlias("c").WithDescription("Create a new note\n(Alias: c)");
                note.AddCommand<NoteDelete>("delete").WithAlias("d").WithDescription("Delete a note\n(Alias: d)");
                note.AddCommand<NoteList>("list").WithAlias("ls").WithDescription("List all notes\n(Alias: ls)");
                note.AddCommand<NoteEdit>("edit").WithAlias("e").WithDescription("Edit a note\n(Alias: e)");
                note.AddCommand<NoteOpen>("open").WithAlias("o").WithDescription("Open a note\n(Alias: o)");
            }).WithAlias("n");
            config.AddBranch("config", noteConfig =>
            {
                noteConfig.SetDescription("Manage configuration options\n(Alias: c) (Default: list)");
                noteConfig.SetDefaultCommand<ConfigList>();
                
                noteConfig.AddCommand<ConfigEditor>("editor").WithAlias("e").WithDescription("Edit the default editor\n(Alias: e)");
                noteConfig.AddCommand<ConfigLocation>("location").WithAlias("l").WithDescription("Edit the default note location\n(Alias: l)");
                noteConfig.AddCommand<ConfigList>("list").WithAlias("ls").WithDescription("List the configuration options and their current values\n(Alias: ls)");
                noteConfig.AddCommand<ConfigCategory>("category").WithAlias("c").WithDescription("Edit the default category for notes\n(Alias: c)");
            }).WithAlias("c");
            config.AddCommand<Interactive>("interactive").WithAlias("i").WithDescription("Launch interactive TUI\n(Alias: i)");
        });
        app.Run(args);
    }
}