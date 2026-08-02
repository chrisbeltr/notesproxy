using System.ComponentModel;

namespace NotesProxy.Cli.Commands;

public class NoteEdit : Command<NoteEdit.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--name", isRequired: true)]
        [Description("The name of the note")]
        public string? Name { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}