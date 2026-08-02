namespace NotesProxy.Manager;

public sealed class NoteManager
{
    private NoteManager()
    {
        Config = new Config();
    }

    private static readonly Lazy<NoteManager> LazyInstance = new(() => new NoteManager());
    public static NoteManager Instance => LazyInstance.Value;

    public IConfig Config { get; }
}