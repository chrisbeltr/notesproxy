using System.Text;
using Microsoft.AspNetCore.Mvc;
using NotesProxy.Manager;
using Swashbuckle.AspNetCore.Annotations;

namespace Server.Controllers;

[ApiController]
[Route("api/info")]
public class InformationController : Controller
{
    private readonly INotes _notes;

    public InformationController(INotes notes, IConfig config)
    {
        _notes = notes;
    }

    [HttpGet("schema")]
    [SwaggerOperation(Summary = "Gets a list of schema names", Description = "Gets a list of schema names")]
    public ActionResult<IEnumerable<string>> GetSchema()
    {
        var schema = _notes.GetSchema();
        return schema.ToList();
    }

    [HttpGet("category")]
    [SwaggerOperation(Summary = "Gets a list of category names", Description = "Gets a list of category names")]
    public ActionResult<IEnumerable<string>> GetCategory()
    {
        var categories = _notes.GetCategories();
        return categories;
    }
}