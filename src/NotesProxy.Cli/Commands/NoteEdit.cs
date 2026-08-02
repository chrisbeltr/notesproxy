using System.ComponentModel;

namespace NotesProxy.Cli.Commands;

public class NoteEdit : Command<NoteEdit.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<command name>")]
        [Description("The name of the note to edit")]
        public required string NoteName { get; set; }

        [CommandOption("-n|--name")]
        [Description("The new name of the note")]
        public string? NewName { get; set; }

        [CommandOption("-l|--location")]
        [Description("The new location of the note")]
        public string? NewLocation { get; set; }

        [CommandOption("-e|--editor")]
        [Description("The new editor for the note")]
        public string? NewEditor { get; set; }

        [CommandOption("-c|--category")]
        [Description("The new category for the note")]
        public string? NewCategory { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}