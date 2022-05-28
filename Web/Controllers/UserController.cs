using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Handlers.User.Commands;

namespace Web.Controllers;

[Route("user")]
public class UserController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) => _mediator = mediator;

    [HttpPost, Route("login")]
    public async Task Login([FromBody] LoginRequest request, CancellationToken cancellationToken) =>
        await _mediator.Send(request, cancellationToken);

    [HttpPost, Route("register")]
    public async Task Register([FromBody] RegisterRequest request, CancellationToken cancellationToken) =>
        await _mediator.Send(request, cancellationToken);

    [HttpPost, Route("logout")]
    public async Task Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken) =>
        await _mediator.Send(request, cancellationToken);

    [Authorize]
    [HttpGet, Route("is-authorized")]
    public async Task IsAuthorized(CancellationToken cancellationToken) { }

    [Authorize(Roles = "admin")]
    [HttpGet, Route("is-admin")]
    public async Task IsAdmin(CancellationToken cancellationToken) { }
}