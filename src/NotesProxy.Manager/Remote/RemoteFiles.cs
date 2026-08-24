namespace NotesProxy.Manager.Remote;

public class RemoteFiles(HttpClient client) : IFiles
{
    public void OpenNote(string name, string? editor = null)
    {
        throw new NotImplementedException();
    }

    public void CreateNote(string name, string location)
    {
        throw new NotImplementedException();
    }

    public void MoveNote(string oldName, string? newName = null, string? newLocation = null)
    {
        throw new NotImplementedException();
    }

    public void DeleteNote(string name)
    {
        throw new NotImplementedException();
    }
}