using Spectre.Console.Cli;
using NotesProxy.Tui;
using NotesProxy.Manager;

namespace NotesProxy.Cli;

internal class C : Command<C.Settings> // Placeholder Command
{
    public class Settings : CommandSettings
    {
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellation)
    {
        Console.WriteLine("this is a placeholder!");
        return 0;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.SetHelpProvider(new NotesHelp(config.Settings));
            
            config.AddBranch("note", note =>
            {
                note.SetDescription("Manage notes\n(Alias: n) (Default: create)");
                note.SetDefaultCommand<C>();
                
                note.AddCommand<C>("create").WithAlias("c").WithDescription("Create a new note\n(Alias: c)");
                note.AddCommand<C>("delete").WithAlias("d").WithDescription("Delete a note\n(Alias: d)");
                note.AddCommand<C>("list").WithAlias("ls").WithDescription("List all notes\n(Alias: ls)");
                note.AddCommand<C>("edit").WithAlias("e").WithDescription("Edit a note\n(Alias: e)");
                note.AddCommand<C>("move").WithAlias("m").WithDescription("Move a note\n(Alias: m)");
            }).WithAlias("n");
            config.AddBranch("config", noteConfig =>
            {
                noteConfig.SetDescription("Manage configuration options\n(Alias: c) (Default: list)");
                noteConfig.SetDefaultCommand<C>();
                
                noteConfig.AddCommand<C>("editor").WithAlias("e").WithDescription("Edit the default editor\n(Alias: e)");
                noteConfig.AddCommand<C>("location").WithAlias("l").WithDescription("Edit the default note location\n(Alias: l)");
                noteConfig.AddCommand<C>("list").WithAlias("ls").WithDescription("List the configuration options and their current values\n(Alias: ls)");
            }).WithAlias("c");
            config.AddCommand<C>("interactive").WithAlias("i").WithDescription("Launch interactive TUI\n(Alias: i)");
            config.AddCommand<C>("help").WithAlias("h").WithDescription("Shows this help menu\n(Alias: h)");
        });
        app.Run(args);
    }
}