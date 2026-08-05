using System.Diagnostics;

namespace NotesProxy.Manager;

internal class Files : IFiles
{
    private readonly Notes _notes = new Notes();

    public Files()
    {
        var notesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NotesProxy",
            "notes");
        if (!Path.Exists(notesPath)) Directory.CreateDirectory(notesPath);
    }

    public void OpenNote(string name, string? editorOverride = null)
    {
        if (!_notes.NoteExists(name)) throw new Exception("Note does not exist.");

        var note = _notes.GetNote(name);
        var fullPath = Path.Combine(note[1]!, note[0]!);
        if (Path.Exists(fullPath))
        {
            // absolutely awful amount of escaping i need to do
            // and no, i am not supporting more than one nested quote
            // if you name your files like that you're a psycho
            var editor =
                (editorOverride ?? note[2] ?? NoteManager.Instance.Config.GetEditor()).Replace("\"", "\\\\\\\"");
            fullPath = $"\\\"{fullPath.Replace("\"", "\\\\\\\"")}\\\"";
            string commandPrompt;
            if (OperatingSystem.IsWindows()) commandPrompt = "cmd";
            else commandPrompt = "bash";
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = commandPrompt,
                Arguments = $"-c \"{editor} {fullPath}\"",
                UseShellExecute = true,
            };

            using var process = Process.Start(startInfo);
            process?.WaitForExit();
        }
    }

    public void CreateNote(string name, string location)
    {
        if (_notes.NoteExists(name)) throw new Exception("Note already exists.");
        if (Path.Exists(Path.Combine(location, name))) throw new Exception("File already exists.");

        if (!Path.Exists(location)) Directory.CreateDirectory(location);
        File.Create(Path.Combine(location, name)).Close();
    }

    public void MoveNote(string oldName, string? newName, string? newLocation)
    {
        if (!_notes.NoteExists(oldName)) throw new Exception("Note does not exist.");
        if (newName != null && _notes.NoteExists(newName)) throw new Exception("Note already exists.");

        var oldNote = _notes.GetNote(oldName);
        var name = newName ?? oldName;
        var location = newLocation ?? oldNote[1]!;
        
        if (Path.Combine(oldNote[1]!, oldNote[0]!) == Path.Combine(location, name)) return;
        if (!Path.Exists(location)) Directory.CreateDirectory(location);
        
        File.Move(Path.Combine(oldNote[1]!, oldNote[0]!), Path.Combine(location, name));
    }

    public void DeleteNote(string name)
    {
        if (!_notes.NoteExists(name)) throw new Exception("Note does not exist.");
        var note = _notes.GetNote(name);

        try
        {
            File.Delete(Path.Combine(note[1]!, note[0]!));
        }
        catch (Exception)
        {
            // don't really care if the file isn't there, just warn?
            Console.WriteLine("(warning... original file wasn't there. still trying to remove entry from database.)");
        }
    }
}