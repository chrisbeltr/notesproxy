using System.ComponentModel;

namespace NotesProxy.Cli.Commands;

public class NoteCreate : Command<NoteCreate.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("The name of the note")]
        public string? Name { get; set; }
        
        [CommandOption("-l|--location")]
        [Description("The location of the note")]
        public string? Location { get; set; }
        
        [CommandOption("-e|--editor")]
        [Description("The editor overwrite for the note")]
        public string? Editor { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}