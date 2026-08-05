namespace NotesProxy.Manager;

public interface INotes
{
    public List<List<string?>> QueryDatabase(string? queryLocation = null);
    public void InsertNote(List<string?> note);
    public void DeleteNote(string note);
    public void UpdateNote(string name, List<string?> newNote);
}