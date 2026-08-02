using System.ComponentModel;

namespace NotesProxy.Cli.Commands;

public class NoteList : Command<NoteList.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-l|--location")]
        [Description("The location of the note")]
        public string? Location { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}