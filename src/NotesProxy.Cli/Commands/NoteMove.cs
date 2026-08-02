using System.ComponentModel;

namespace NotesProxy.Cli.Commands;

public class NoteMove : Command<NoteMove.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--name", isRequired: true)]
        [Description("The name of the note")]
        public string? Name { get; set; }
        
        [CommandOption("-l|--location", isRequired: true)]
        [Description("The location of the note")]
        public string? Location { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}