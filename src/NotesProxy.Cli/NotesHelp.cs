using Spectre.Console;
using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;

namespace NotesProxy.Cli;

internal class NotesHelp : HelpProvider
{
    public NotesHelp(ICommandAppSettings settings) : base(settings)
    {
    }

    public override IEnumerable<IRenderable> GetCommands(ICommandModel model, ICommandInfo? command)
    {
        var commandContainer = command ?? (ICommandContainer)model;
        var isDefaultCommand = command?.IsDefaultCommand ?? false;

        var commands = isDefaultCommand ? model.Commands : commandContainer.Commands;
        commands = commands.Where(x => !x.IsHidden).ToList();

        if (commands.Count == 0)
        {
            return Array.Empty<IRenderable>();
        }

        var result = new List<IRenderable>
        {
            new Markup("\n[yellow]COMMANDS:[/]\n")
        };

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 0), NoWrap = true });
        // grid.AddColumn(new GridColumn { Padding = new Padding(0, 0, 4, 0) });
        grid.AddColumn(new GridColumn { Padding = new Padding(0) });

        foreach (var child in commands)
        {
            var commandName = child.Name;
            var additionalList = new List<string>();
            var additionals = "";
            if (child.IsBranch)
            {
                foreach (var childCommand in child.Commands)
                {
                    if (!childCommand.IsDefaultCommand) additionalList.Add(childCommand.Name);
                }
                additionals = $"[{string.Join("|", additionalList)}]";
            }
            else
            {
                foreach (var childArgs in child.Parameters.OfType<ICommandArgument>())
                {
                    additionalList.Add(childArgs.IsRequired ? $"<{childArgs.Value}>" : $"[{childArgs.Value}]");
                }
                foreach (var childOpts in child.Parameters.OfType<ICommandOption>())
                {
                    var optsList = new List<string>();
                    optsList.Add(string.Join("|", childOpts.ShortNames.Select(x => $"-{x}")));
                    optsList.Add(string.Join("|", childOpts.LongNames.Select(x => $"--{x}")));
                    additionalList.Add(childOpts.IsRequired ? $"{string.Join("|",  optsList)}" : $"[{string.Join("|",  optsList)}]");
                }
                additionals = $"{string.Join(" ", additionalList)}";
            }
            var commandDescription = child.Description ?? "";

            grid.AddRow(
                new Text(commandName + " " + additionals),
                // new Text(additionals),
                new Text(commandDescription));
        }

        result.Add(grid);

        return result;
    }
}