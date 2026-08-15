using NotesProxy.Tui;

namespace NotesProxy.Cli.Commands;

public class Interactive : Command<Interactive.Settings>
{
    public class Settings : CommandSettings
    {
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        TuiManager.MainMenu();

        return 0;
    }
}