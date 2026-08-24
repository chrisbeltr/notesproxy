using Castle.Components.DictionaryAdapter.Xml;
using Config.Net;

namespace NotesProxy.Manager;

internal class Config : IConfig
{
    private IConfigSettings _settings;

    private string _settingsFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NotesProxy",
            "appsettings.json");

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

        // need to have at least something
        if (OperatingSystem.IsWindows()) _settings.Editor ??= "notepad.exe";
        else _settings.Editor ??= "nano";
        _settings.Location ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NotesProxy", "notes");
        _settings.Category ??= "default";
        _settings.AutoOpen ??= true;
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

    public bool GetAutoOpen()
    {
        return _settings.AutoOpen ?? true;
    }

    public void SetAutoOpen(bool autoOpen)
    {
        _settings.AutoOpen = autoOpen;
    }

    public string GetServer()
    {
        return _settings.Server ?? string.Empty;
    }

    public void SetServer(string address)
    {
        _settings.Server = address;
    }

    public Dictionary<string, object> GetAllSettings()
    {
        var dict = new Dictionary<string, object>
        {
            { "Editor", GetEditor() },
            { "Location", GetLocation() },
            { "Category", GetCategory() },
            { "Auto Open", GetAutoOpen() },
            { "Server", GetServer() }
        };
        return dict;
    }
}

public interface IConfigSettings
{
    string? Editor { get; set; }
    string? Location { get; set; }
    string? Category { get; set; }
    bool? AutoOpen { get; set; }
    string? Server { get; set; }
}