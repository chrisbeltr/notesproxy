using NotesProxy.Tui;
using Spectre.Console;

namespace NotesProxy.Cli.Commands;

public class ConfigList : Command<ConfigList.Settings>
{
    public class Settings : CommandSettings
    {
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        TuiManager.DisplayConfig("CONFIG", NoteManager.Instance.Config.GetAllSettings());

        return 0;
    }
}