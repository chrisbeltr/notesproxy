using Spectre.Console.Cli;
using NotesProxy.Tui;
using NotesProxy.Manager;

namespace NotesProxy.Cli;

class Program
{
    static void Main(string[] args)
    {
        TuiManager tui = new TuiManager();
        tui.Test();
    }
}