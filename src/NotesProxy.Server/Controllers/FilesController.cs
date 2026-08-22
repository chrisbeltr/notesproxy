using System.ComponentModel;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NotesProxy.Manager;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : Controller
{
    private readonly IFiles _files;
    private readonly IConfig _config;

    public FilesController(IFiles files, IConfig config)
    {
        _files = files;
        _config = config;
    }

    [HttpPost("{name}")]
    [SwaggerOperation(Summary = "Creates a new note", Description = "Creates a new note")]
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

    [HttpPut("{name}")]
    [SwaggerOperation(Summary = "Moves a note", Description = "Moves a note")]
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
    [SwaggerOperation(Summary = "Deletes a note", Description = "Deletes a note")]
    public ActionResult DeleteNoteFile(string name)
    {
        _files.DeleteNote(name);
        return Ok();
    }
}