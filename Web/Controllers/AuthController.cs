using System.Linq.Expressions;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Data.Entities;
using Web.Exceptions;
using Web.ViewModels.Auth;

namespace Web.Controllers;

[Route("auth")]
[ApiController, AllowAnonymous]
[Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
public class AuthController : ControllerBase
{
    private const string UniversalCode = "7373";

    [HttpPost]
    public Task SignIn(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    [HttpPost, Route("/fast-registration")]
    public async Task<Guid> FastRegistration(FastRegistrationRequest request,
                                             CancellationToken cancellationToken,
                                             [FromServices] DataContext dataContext)
    {
        var selector = (Expression<Func<Communication, bool>>)(x => x.Type == request.Type && x.Value == request.Value);
        var verification = new Verification
        {
            Code = UniversalCode,
        };
        var communication = await dataContext.Communications
                                             .Include(x => x.User)
                                             .FirstOrDefaultAsync(selector, cancellationToken) ?? new Communication
        {
            Value = request.Value,
            Type = request.Type,
            Verifications = new[] { verification },
        };

        if (communication.User == null)
        {
            var user = new VolunteerUser
            {
                FirstName = request.Name,
                Communications = new[] { communication },
            };
            dataContext.Users.Add(user);
        }

        await dataContext.SaveChangesAsync(cancellationToken);
        return verification.Id;
    }

    [HttpPost, Route("/fast-registration/confirm")]
    public async Task ConfirmFastRegistration(Guid id,
                                              string code,
                                              CancellationToken cancellationToken,
                                              [FromServices] DataContext dataContext)
    {
        var verification = await dataContext.Verifications
                                            .Include(x => x.Communication)
                                            .ThenInclude(x => x.User)
                                            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (verification == null)
        {
            throw new RestException("Неизвестно кого авторизовывать", HttpStatusCode.NotFound);
        }

        if (verification.Code != code)
        {
            throw new RestException("Введенный код неверный", HttpStatusCode.Forbidden);
        }

        var claims = new List<Claim> { new(ClaimsIdentity.DefaultNameClaimType, verification.Communication.User.Id.ToString()) };
        var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                                                ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        await dataContext.SaveChangesAsync(cancellationToken);
    }
}