namespace NotesProxy.Manager.Remote;

internal class NoteClient
{
    public NoteClient(string uri)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        };
        HttpClient httpClient = new(handler);
        httpClient.BaseAddress = new Uri(uri);

        Notes = new RemoteNotes(httpClient);
        Files = new RemoteFiles(httpClient);
    }
    
    public INotes Notes { get; }
    public IFiles Files { get; }
}