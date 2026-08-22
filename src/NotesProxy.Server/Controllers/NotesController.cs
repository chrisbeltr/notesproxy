using System.Text;
using Microsoft.AspNetCore.Mvc;
using NotesProxy.Manager;

namespace Server.Controllers;

[ApiController]
[Route("api/notes")]
public class NotesController : Controller
{
    private readonly IFiles _files;
    private readonly INotes _notes;
    private readonly IConfig _config;

    public NotesController(IFiles files, INotes notes, IConfig config)
    {
        _files = files;
        _notes = notes;
        _config = config;
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
            return NotFound("Note not found.");
        }
    }

    [HttpPost("{name}")]
    public ActionResult CreateNote(string name)
    {
        try
        {
            _files.CreateNote(name, _config.GetLocation());
            return Ok();
        }
        catch
        {
            return Conflict($"Note already exists.");
        }
    }

    [HttpPost]
    public ActionResult InsertNote(Note note)
    {
        try
        {
            _notes.InsertNote(note with { Location = _config.GetLocation() });
            return Ok();
        }
        catch
        {
            return Conflict($"Note already exists.");
        }
    }

    [HttpPut("{name}")]
    public ActionResult UpdateNote(string name, Note note)
    {
        try
        {
            _notes.UpdateNote(name, note with { Location = _config.GetLocation() });
            return Ok();
        }
        catch (Exception ex)
        {
            if (ex.Message == "Note does not exist.")
                return BadRequest("Note does not exist.");
            return Conflict($"Note already exists.");
        }
    }

    [HttpPut("{name}/move")]
    public ActionResult MoveNote(string name, string newName)
    {
        try
        {
            _files.MoveNote(name, newName);
            return Ok();
        }
        catch (Exception ex)
        {
            if (ex.Message == "Note does not exist.")
                return NotFound("Note does not exist.");
            return Conflict($"Note already exists.");
        }
    }

    [HttpDelete("{name}")]
    public ActionResult DeleteNote(string name)
    {
        _notes.DeleteNote(name);
        return Ok();
    }

    [HttpDelete("{name}/delete")]
    public ActionResult DeleteNoteFile(string name)
    {
        _files.DeleteNote(name);
        return Ok();
    }

    [HttpGet("{name}/content")]
    public ActionResult GetNoteContent(string name)
    {
        if (!System.IO.File.Exists(Path.Combine(_config.GetLocation(), name)))
            return NotFound("Note not found.");
        
        return PhysicalFile(Path.Combine(_config.GetLocation(), name), "text/plain");
    }

    [HttpPut("{name}/content")]
    public async Task<ActionResult> UpdateNoteContent(string name)
    {
        if (!System.IO.File.Exists(Path.Combine(_config.GetLocation(), name)))
            return NotFound("Note not found.");

        await using var stream = System.IO.File.OpenWrite(Path.Combine(_config.GetLocation(), name));
        await Request.Body.CopyToAsync(stream);
        return Ok();
    }
}