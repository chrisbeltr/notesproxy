namespace NotesProxy.Manager;

public interface IConfig
{
    public string GetEditor();
    public void SetEditor(string editor);

    public string GetLocation();
    public void SetLocation(string location);

    public Dictionary<string, object> GetAllSettings();
}