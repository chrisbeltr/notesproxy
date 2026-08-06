namespace NotesProxy.Manager;

public interface INotes
{
    public Note GetNote(string name);
    public bool NoteExists(string name);
    public List<Note> QueryDatabase(string? queryLocation = null);
    public void InsertNote(Note note);
    public void DeleteNote(string note);
    public void UpdateNote(string name, Note newNote);
}