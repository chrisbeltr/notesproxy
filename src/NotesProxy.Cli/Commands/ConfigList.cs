using NotesProxy.Manager;
using Spectre.Console.Cli;

namespace NotesProxy.Cli.Commands;

public class ConfigList : Command<ConfigList.Settings>
{
    public class Settings : CommandSettings
    {
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        Console.WriteLine("Settings:");
        foreach (var (setting, value) in NoteManager.Instance.Config.GetAllSettings())
        {
            Console.WriteLine($"\t{setting}: \"{value}\"");
        }

        return 0;
    }
}