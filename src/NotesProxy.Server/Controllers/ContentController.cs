using System.Text;
using Microsoft.AspNetCore.Mvc;
using NotesProxy.Manager;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers;

[ApiController]
[Route("api/content")]
public class ContentController : Controller
{
    private readonly IConfig _config;

    public ContentController(IFiles files, IConfig config)
    {
        _config = config;
    }

    [HttpGet("{name}")]
    [SwaggerOperation(Summary = "Gets note contents", Description = "Gets note contents")]
    public ActionResult GetNoteContent(string name)
    {
        if (!System.IO.File.Exists(Path.Combine(_config.GetLocation(), name)))
            return NotFound("Note not found.");
        
        return PhysicalFile(Path.Combine(_config.GetLocation(), name), "text/plain");
    }

    [HttpPut("{name}")]
    [SwaggerOperation(Summary = "Updates note contents", Description = "Updates note contents")]
    public async Task<ActionResult> UpdateNoteContent(string name)
    {
        if (!System.IO.File.Exists(Path.Combine(_config.GetLocation(), name)))
            return NotFound("Note not found.");

        await using var stream = System.IO.File.OpenWrite(Path.Combine(_config.GetLocation(), name));
        await Request.Body.CopyToAsync(stream);
        return Ok();
    }
}