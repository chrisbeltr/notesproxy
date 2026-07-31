using Spectre.Console;

namespace NotesProxy.Tui;

public class TuiManager()
{
    public void Test()
    {
        AnsiConsole.Status().Start("testing!...", ctx => { Thread.Sleep(5000); });

        AnsiConsole.MarkupLine("[green]Done![/]");
    }
}