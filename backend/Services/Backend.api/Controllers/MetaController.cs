using System.Diagnostics;
using Application.Configs;
using Application.Utils.Service;
using Infrastructure.Services.Email;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Backend.api.Controllers;

public class MetaController : BaseApiController {
    private readonly IMailService _mailService;
    private readonly ITemplateService _templateService;
    public MetaController(IMediator _mediator, BackendApplicationConfig _configuration, IMailService mailService, ITemplateService templateService) : base(_mediator,
        _configuration) {
        _mailService = mailService;
        _templateService = templateService;
    }
    [HttpGet("/maintenance")]
    public IActionResult Maintenance() {
        return Ok(new {
            Maintenance = IsMaintenanceMode()
        });
    }

    [HttpGet("/info")]
    public ActionResult<string> Info() {
        var assembly = typeof(Program).Assembly;

        var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
        var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

        return Ok($"Version: {version}, Last Updated: {lastUpdate}");
    }
}