using NotesProxy.Manager.Remote;

namespace NotesProxy.Manager;

public sealed class NoteManager
{
    private NoteManager()
    {
        Config = new Config();
        if (Config.GetServer() != String.Empty)
        {
            Client = new NoteClient(Config.GetServer());
            Notes = Client.Notes;
            Files = Client.Files;
        }
        else
        {
            Notes = new Notes();
            Files = new Files();
        }
    }

    private static readonly Lazy<NoteManager> LazyInstance = new(() => new NoteManager());
    public static NoteManager Instance => LazyInstance.Value;

    public IConfig Config { get; }
    public INotes Notes { get; }
    public IFiles Files { get; }
    private NoteClient? Client { get; }
}