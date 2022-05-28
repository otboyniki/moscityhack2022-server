using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Web.Handlers.User.Commands;

public class LogoutRequest : IRequest { }

public class LogoutHandler : IRequestHandler<LogoutRequest>
{
    private readonly SignInManager<Data.Entities.User> _signInManager;

    public LogoutHandler(SignInManager<Data.Entities.User> signInManager) => _signInManager = signInManager;

    public async Task<Unit> Handle(LogoutRequest request, CancellationToken ct)
    {
        await _signInManager.SignOutAsync();
        return Unit.Value;
    }
}