using Castle.Components.DictionaryAdapter.Xml;
using Config.Net;

namespace NotesProxy.Manager;

internal class Config : IConfig
{
    private IConfigSettings _settings;

    private string _settingsFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NotesProxy", "appsettings.json");

    public Config()
    {
        if (!File.Exists(Path.GetDirectoryName(_settingsFile)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_settingsFile)!);
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

    public string GetCategory()
    {
        return _settings.Category ?? string.Empty;
    }

    public void SetCategory(string category)
    {
        _settings.Category = category;
    }

    public Dictionary<string, object> GetAllSettings()
    {
        var dict = new Dictionary<string, object>
        {
            { "Editor", _settings.Editor },
            { "Location", _settings.Location },
            { "Category", _settings.Category },
        };
        return dict;
    }
}

public interface IConfigSettings
{
    string Editor { get; set; }
    string Location { get; set; }
    string Category { get; set; }
}