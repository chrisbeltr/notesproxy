using System.Text;
using Microsoft.AspNetCore.Mvc;
using NotesProxy.Manager;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers;

[ApiController]
[Route("api/notes")]
public class NotesController : Controller
{
    private readonly INotes _notes;
    private readonly IConfig _config;

    public NotesController(INotes notes, IConfig config)
    {
        _notes = notes;
        _config = config;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all notes", Description = "Gets all notes")]
    public ActionResult<List<Note>> GetNotes([FromQuery] string? category)
    {
        var notes = _notes.QueryDatabase(category);
        return notes;
    }

    [HttpGet("{name}")]
    [SwaggerOperation(Summary = "Gets a note", Description = "Gets a note")]
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
    [SwaggerOperation(Summary = "Creates a new note", Description = "Creates a new note")]
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
    [SwaggerOperation(Summary = "Updates a note", Description = "Updates a note")]
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

    [HttpDelete("{name}")]
    [SwaggerOperation(Summary = "Deletes a note", Description = "Deletes a note")]
    public ActionResult DeleteNote(string name)
    {
        _notes.DeleteNote(name);
        return Ok();
    }
}