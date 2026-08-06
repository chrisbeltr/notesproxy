using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public class ConfigList : Command<ConfigList.Settings>
{
    public class Settings : CommandSettings
    {
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine("\n[yellow]SETTINGS:[/]");

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 0, 0, 0) });
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 0, 0, 0) });

        foreach (var (setting, value) in NoteManager.Instance.Config.GetAllSettings())
        {
            grid.AddRow([new Text(setting), new Text($"\"{value}\"")]);
        }
        
        AnsiConsole.Write(grid);

        return 0;
    }
}