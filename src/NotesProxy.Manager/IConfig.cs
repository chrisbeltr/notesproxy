namespace NotesProxy.Manager;

public interface IConfig
{
    public string GetEditor();
    public void SetEditor(string editor);

    public string GetLocation();
    public void SetLocation(string location);

    public string GetCategory();
    public void SetCategory(string category);

    public bool GetAutoOpen();
    public void SetAutoOpen(bool autoOpen);

    public string GetServer();
    public void SetServer(string address);

    public Dictionary<string, object> GetAllSettings();
}