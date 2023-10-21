using System.Diagnostics;
using Application.Configs;
using Application.Utils.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Backend.Controllers;

public class MetaController : BaseApiController {
    public MetaController(IMediator _mediator, BackendApplicationConfig _configuration) : base(_mediator, _configuration) { }
    [HttpGet("/info")]
    public ActionResult<string> Info() {
        var assembly = typeof(Program).Assembly;

        var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
        var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

        return Ok($"Version: {version}, Last Updated: {lastUpdate}");
    }
}