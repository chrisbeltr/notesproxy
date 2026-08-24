using System.Diagnostics;
using System.Net.Http.Headers;

namespace NotesProxy.Manager.Remote;

public class RemoteFiles(HttpClient client) : IFiles
{
    private string GetNoteContentTemporaryFile(string name)
    {
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/content/{name}");
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }
        
        using Stream body = response.Content.ReadAsStream();
        var tempDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NotesProxy/temp");
        Directory.CreateDirectory(tempDirectoryPath);
        var tempFilePath = Path.Combine(tempDirectoryPath, name);
        using var f = File.OpenWrite(tempFilePath);
        body.CopyTo(f);
        return tempFilePath;
    }

    private void UpdateNoteContent(string name, FileStream stream)
    {
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"api/content/{name}");
        using var content = new StreamContent(stream);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        request.Content = content;
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }
    }
    
    public void OpenNote(string name, string? editorOverride = null)
    {
        
        var fullPath = GetNoteContentTemporaryFile(name);
        Console.WriteLine(fullPath);
        if (!Path.Exists(fullPath)) throw new Exception("Could not get note.");
        ProcessStartInfo startInfo;
        var editor = (editorOverride ?? NoteManager.Instance.Config.GetEditor());
        if (OperatingSystem.IsWindows())
        {
            startInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c {editor} {fullPath}",
                UseShellExecute = true,
            };
        }
        else
        {
            // absolutely awful amount of escaping i need to do
            // and no, i am not supporting more than one nested quote
            // if you name your files like that you're a psycho
            editor = editor.Replace("\"", "\\\\\\\"");
            var escapedPath = $"\\\"{fullPath.Replace("\"", "\\\\\\\"")}\\\"";
            startInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{editor} {escapedPath}\"",
                UseShellExecute = true,
            };
        }

        using var process = Process.Start(startInfo);
        process?.WaitForExit();
        
        // get new file contents
        using var f = File.OpenRead(fullPath);
        // send new file contents to server
        UpdateNoteContent(name, f);
        // delete temp file
        File.Delete(fullPath);
    }

    public void CreateNote(string name, string location)
    {
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"api/files/{name}");
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }
    }

    public void MoveNote(string oldName, string? newName = null, string? newLocation = null)
    {
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"api/files/{oldName}?newName={newName}");
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }
    }

    public void DeleteNote(string name)
    {
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"api/files/{name}");
        using HttpResponseMessage response = client.Send(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unknown error, please report this!");
        }
    }
}