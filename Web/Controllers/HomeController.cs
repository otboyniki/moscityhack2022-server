using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Handlers.Home.Queries;

namespace Web.Controllers;

[Route("/")]
[AllowAnonymous]
[ApiController]
[Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
public class HomeController : ControllerBase
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator) =>
        _mediator = mediator;

    /// <summary>
    ///     Метод, который показывает, что сервак живой и даже отвечает
    /// </summary>
    [HttpGet]
    public Task<object> Index([FromQuery] IndexRequest request, CancellationToken cancellationToken) =>
        _mediator.Send(request, cancellationToken);
}