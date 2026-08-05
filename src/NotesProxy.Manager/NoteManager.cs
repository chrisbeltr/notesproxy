namespace NotesProxy.Manager;

public sealed class NoteManager
{
    private NoteManager()
    {
        Config = new Config();
        Notes = new Notes();
        Files = new Files();
    }

    private static readonly Lazy<NoteManager> LazyInstance = new(() => new NoteManager());
    public static NoteManager Instance => LazyInstance.Value;

    public IConfig Config { get; }
    public INotes Notes { get; }
    public IFiles Files { get; }
}