using Config.Net;

namespace NotesProxy.Manager;

internal class Config : IConfig
{
    private IConfigSettings _settings;

    private string _settingsFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NotesProxy", "appsettings.json");

    public Config()
    {
        var settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NotesProxy");
        if (!File.Exists(settingsPath))
        {
            Directory.CreateDirectory(settingsPath);
        }
        if (!File.Exists(_settingsFile))
        {
            var f = File.CreateText(_settingsFile);
            f.Write("{}");
            f.Dispose();
        }
        _settings = new ConfigurationBuilder<IConfigSettings>()
            .UseJsonFile(_settingsFile)
            .Build();
    }
    
    public string GetEditor()
    {
        return _settings.Editor ?? string.Empty;
    }
    public void SetEditor(string editor)
    {
        _settings.Editor = editor;
    }

    public string GetLocation()
    {
        return _settings.Location ?? string.Empty;
    }
    public void SetLocation(string location)
    {
        _settings.Location = location;
    }

    public Dictionary<string, object> GetAllSettings()
    {
        var dict = new Dictionary<string, object>
        {
            { "Editor", _settings.Editor },
            { "Location", _settings.Location }
        };
        return dict;
    }
}

public interface IConfigSettings
{
    string Editor { get; set; }
    string Location { get; set; }
}