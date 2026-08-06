namespace NotesProxy.Manager;

public interface IFiles
{
    public void OpenNote(string name, string? editor = null);
    public void CreateNote(string name, string location);
    public void MoveNote(string oldName, string? newName = null, string? newLocation = null);
    public void DeleteNote(string name);
}