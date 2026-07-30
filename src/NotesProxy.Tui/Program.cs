using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace NotesProxy.Tui;

class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.Status().Start("fucking...", ctx => { Thread.Sleep(5000); });

        AnsiConsole.MarkupLine("[green]Done![/]");
    }
}