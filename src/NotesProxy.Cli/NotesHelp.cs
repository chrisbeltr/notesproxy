using Spectre.Console;
using Spectre.Console.Cli;
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
            var argsList = new List<string>();
            foreach (var childCommand in child.Commands)
            {
                if (!childCommand.IsDefaultCommand) argsList.Add(childCommand.Name);
            }
            var arguments = argsList.Count > 0 ? $"[{string.Join("|", argsList)}]" : "";
            var commandDescription = child.Description ?? "";

            grid.AddRow(
                new Text(commandName + " " + arguments),
                // new Text(arguments),
                new Text(commandDescription));
        }

        result.Add(grid);

        return result;
    }
}