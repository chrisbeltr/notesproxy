using System.Text;
using Microsoft.AspNetCore.Mvc;
using NotesProxy.Manager;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers;

[ApiController]
[Route("api/schema")]
public class SchemaController : Controller
{
    private readonly INotes _notes;

    public SchemaController(INotes notes, IConfig config)
    {
        _notes = notes;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets a list of schema names", Description = "Gets a list of schema names")]
    public ActionResult<IEnumerable<string>> GetSchema()
    {
        var schema = _notes.GetSchema();
        return schema.ToList();
    }
}