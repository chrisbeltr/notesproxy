namespace NotesProxy.Manager;

public interface INotes
{
    public Note GetNote(string name);
    public bool NoteExists(string name);
    public List<Note> QueryDatabase(string? queryCategory = null);
    public IEnumerable<string> GetSchema();
    public void InsertNote(Note note);
    public void DeleteNote(string note);
    public void UpdateNote(string name, Note newNote);
    public void DropNotes();
    public List<string> GetCategories();
}