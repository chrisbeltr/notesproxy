using Microsoft.AspNetCore.Mvc;
using NotesProxy.Manager;

namespace Server.Controllers;

[ApiController]
[Route("api/notes")]
public class NotesController : Controller
{
    private readonly IFiles _files;
    private readonly INotes _notes;

    public NotesController(IFiles files, INotes notes)
    {
        _files = files;
        _notes = notes;
    }

    [HttpGet]
    public ActionResult<List<Note>> GetNotes([FromQuery] string? category)
    {
        var notes = _notes.QueryDatabase(category);
        return notes;
    }

    [HttpGet("/api/schema")]
    public ActionResult<IEnumerable<string>> GetSchema()
    {
        var schema = _notes.GetSchema();
        return schema.ToList();
    }

    [HttpGet("{name}")]
    public ActionResult<Note> GetNote(string name)
    {
        try
        {
            var note = _notes.GetNote(name);
            return note;
        }
        catch
        {
            return NotFound();
        }
    }
}