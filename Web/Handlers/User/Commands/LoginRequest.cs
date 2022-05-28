using MediatR;
using Microsoft.AspNetCore.Identity;
using Web.Exceptions;
using Web.Exceptions.Models;

namespace Web.Handlers.User.Commands;

public class LoginRequest : IRequest
{
    public string Username { get; set; }

    public string Password { get; set; }
}

public class LoginHandler : IRequestHandler<LoginRequest>
{
    private readonly UserManager<Data.Entities.User> _userManager;
    private readonly SignInManager<Data.Entities.User> _signInManager;

    public LoginHandler(UserManager<Data.Entities.User> userManager,
                        SignInManager<Data.Entities.User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Unit> Handle(LoginRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            throw new UnauthorizedException(UnauthorizedError.UsernameOrPasswordIsInvalid);
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedException(UnauthorizedError.UsernameOrPasswordIsInvalid);
        }

        return Unit.Value;
    }
}