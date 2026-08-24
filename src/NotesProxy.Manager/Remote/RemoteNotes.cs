using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace NotesProxy.Manager.Remote;

public class RemoteNotes(HttpClient client) : INotes
{
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
        var note = JsonSerializer.Deserialize<Note>(body, new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
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
        var note = JsonSerializer.Deserialize<List<Note>>(body, new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
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
        var note = JsonSerializer.Deserialize<List<string>>(body, new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
        return note!;
    }

    public void InsertNote(Note note)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, "api/notes/" + note.Name);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        request.Content = JsonContent.Create(note, mediaType: null, options);
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }
    }

    public void DeleteNote(string note)
    {
        using HttpRequestMessage request = new(HttpMethod.Delete, "api/notes/" + note);
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }
    }

    public void UpdateNote(string name, Note newNote)
    {
        using HttpRequestMessage request = new(HttpMethod.Put, "api/notes/" + name);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        request.Content = JsonContent.Create(newNote, mediaType: null, options);
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
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