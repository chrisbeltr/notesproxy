using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace NotesProxy.Manager.Remote;

public class RemoteNotes(HttpClient client) : INotes
{
    private T? Deserialize<T>(Stream body) =>
        JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    private JsonContent CreateContent(Note note) => JsonContent.Create(note, mediaType: null,
        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

    private Note? FindNote(string name)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "api/notes/" + name);
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            throw new Exception("Unknown error, please report this!");
        }

        using Stream body = response.Content.ReadAsStream();
        var note = Deserialize<Note>(body);
        return note;
    }

    public Note GetNote(string name)
    {
        return FindNote(name) ?? throw new Exception("Note does not exist.");
    }

    public bool NoteExists(string name) => FindNote(name) != null;

    public List<Note> QueryDatabase(string? queryCategory = null)
    {
        using HttpRequestMessage request = new(HttpMethod.Get,
            "api/notes/" + (queryCategory != null ? $"?category={queryCategory}" : ""));
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }

        using Stream body = response.Content.ReadAsStream();
        var note = Deserialize<List<Note>>(body);
        return note!;
    }

    public IEnumerable<string> GetSchema()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "api/schema");
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }

        using Stream body = response.Content.ReadAsStream();
        var note = Deserialize<List<string>>(body);
        return note!;
    }

    public void InsertNote(Note note)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, "api/notes/" + note.Name);
        request.Content = CreateContent(note);
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Conflict)
                throw new Exception("Note already exists.");
            throw new Exception("Unknown error, please report this!");
        }
    }

    public void DeleteNote(string note)
    {
        using HttpRequestMessage request = new(HttpMethod.Delete, "api/notes/" + note);
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new Exception("Note not found.");
            throw new Exception("Unknown error, please report this!");
        }
    }

    public void UpdateNote(string name, Note newNote)
    {
        using HttpRequestMessage request = new(HttpMethod.Put, "api/notes/" + name);
        request.Content = CreateContent(newNote);
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Conflict)
                throw new Exception("Note already exists.");
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new Exception("Note not found.");
            throw new Exception("Unknown error, please report this!");
        }
    }

    public void DropNotes()
    {
        using HttpRequestMessage request = new(HttpMethod.Delete, "api/notes");
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }
    }
}