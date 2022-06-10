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
using Web.Data.Enums;
using Web.Exceptions;

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
    public async Task<Guid> FastRegistration(string? email,
                                             string? phone,
                                             CancellationToken cancellationToken,
                                             [FromServices] DataContext dataContext)
    {
        if (email == null && phone == null)
        {
            throw new RestException($"{nameof(email)} и {nameof(phone)} не заполнены", HttpStatusCode.BadRequest);
        }

        if (email != null && phone != null)
        {
            throw new RestException($"Должен быть заполнен один из {nameof(email)} и {nameof(phone)}", HttpStatusCode.BadRequest);
        }

        var communicationType = GetCommunicationType(email, phone);
        var value = communicationType switch
        {
            CommunicationType.Email => email,
            CommunicationType.Phone => phone,
            _ => throw new ArgumentOutOfRangeException(),
        };
        var selector = (Expression<Func<Communication, bool>>)(x => x.Type == communicationType && x.Value == value);
        var verification = new Verification
        {
            Code = UniversalCode,
        };
        var communication = await dataContext.Communications.FirstOrDefaultAsync(selector, cancellationToken) ?? new Communication
        {
            Value = value!,
            Type = communicationType,
            Verifications = new[]
            {
                verification,
            },
        };

        var user = new User
        {
            Communications = new[] { communication },
        };
        dataContext.Users.Add(user);

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

    private CommunicationType GetCommunicationType(string? email,
                                                   string? phone) =>
        email != null
            ? CommunicationType.Email
            : CommunicationType.Phone;
}