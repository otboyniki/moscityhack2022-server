using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Web.Exceptions;

namespace Web.Handlers.User.Commands;

public class RegisterRequest : IRequest
{
    public string Username { get; set; }

    public string Password { get; set; }
}

public class RegisterHandler : IRequestHandler<RegisterRequest>
{
    private readonly UserManager<Data.Entities.User> _userManager;
    private readonly SignInManager<Data.Entities.User> _signInManager;

    public RegisterHandler(UserManager<Data.Entities.User> userManager,
                           SignInManager<Data.Entities.User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Unit> Handle(RegisterRequest request, CancellationToken ct)
    {
        var user = new Data.Entities.User { UserName = request.Username };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var identityError = result.Errors.First();
            throw new RestException(identityError.Description, identityError.Code, HttpStatusCode.UnprocessableEntity);
        }

        await _signInManager.SignInAsync(user, true);

        return Unit.Value;
    }
}