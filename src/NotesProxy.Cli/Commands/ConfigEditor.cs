using System.ComponentModel;
using NotesProxy.Manager;
using Spectre.Console.Cli;

namespace NotesProxy.Cli.Commands;

public class ConfigEditor : Command<ConfigEditor.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<command>")]
        [Description("The command to run the default editor")]
        public required string Command { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        NoteManager.Instance.Config.SetEditor(settings.Command);
        Console.WriteLine($"Successfully set the editor to \"{settings.Command}\".");

        return 0;
    }
}