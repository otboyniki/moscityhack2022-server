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
using Web.ViewModels.Auth;

namespace Web.Controllers;

[Route("auth")]
[ApiController, AllowAnonymous]
[Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
public class AuthController : ControllerBase
{
    private const string UniversalCode = "7373";

    [HttpPost, Route("/sign-in")]
    public async Task<AuthResponse> SignIn(SignInRequest request,
                                           [FromServices] DataContext dataContext,
                                           CancellationToken cancellationToken)
    {
        var selector = (Expression<Func<Communication, bool>>)(x => x.Type == request.Communication.Type && x.Value == request.Communication.Value);
        var communication = await dataContext.Communications
                                             .Include(x => x.Verifications)
                                             .FirstOrDefaultAsync(selector, cancellationToken);
        if (communication == null)
        {
            throw new RestException("Такой пользователь не существует", HttpStatusCode.NotFound);
        }

        var verification = AddVerification(communication, UniversalCode);

        await dataContext.SaveChangesAsync(cancellationToken);
        return new AuthResponse(verification.Id);
    }

    [HttpPost, Route("/fast-registration")]
    public async Task<AuthResponse> FastRegistration(FastRegistrationRequest request,
                                                     CancellationToken cancellationToken,
                                                     [FromServices] DataContext dataContext)
    {
        var selector = (Expression<Func<Communication, bool>>)(x => x.Type == request.Communication.Type && x.Value == request.Communication.Value);

        var communication = await dataContext.Communications
                                             .Include(x => x.User)
                                             .Include(x => x.Verifications)
                                             .FirstOrDefaultAsync(selector, cancellationToken) ?? new Communication
        {
            Value = request.Communication.Value,
            Type = request.Communication.Type,
        };
        var verification = AddVerification(communication, UniversalCode);

        if (communication.User == null)
        {
            var user = new VolunteerUser
            {
                FirstName = request.FirstName,
                Communications = new[] { communication },
            };
            dataContext.Users.Add(user);
        }

        await dataContext.SaveChangesAsync(cancellationToken);
        return new AuthResponse(verification.Id);
    }

    [HttpPost, Route("/fast-registration/confirm")]
    public async Task ConfirmFastRegistration(ConfirmFastRegistrationRequest request,
                                              CancellationToken cancellationToken,
                                              [FromServices] DataContext dataContext)
    {
        var verification = await dataContext.Verifications
                                            .Include(x => x.Communication)
                                            .ThenInclude(x => x.User)
                                            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (verification == null)
        {
            throw new RestException("Неизвестно кого авторизовывать", HttpStatusCode.NotFound);
        }

        if (verification.Code != request.Code)
        {
            throw new RestException("Введенный код неверный", HttpStatusCode.BadRequest);
        }

        var claims = new List<Claim> { new(ClaimsIdentity.DefaultNameClaimType, verification.Communication.User!.Id.ToString()) };
        var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                                                ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    [HttpPost, Route("/registration/volunteer")]
    public async Task<AuthResponse> VolunteerUserRegistration(VolunteerUserRegistrationRequest request,
                                                              CancellationToken cancellationToken,
                                                              [FromServices] DataContext dataContext)
    {
        await CheckUserAsync(request.Email, dataContext, cancellationToken);

        var (user, verification) = CreateUser<VolunteerUser>(UniversalCode, CommunicationType.Email, request.Email);
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        dataContext.Users.Add(user);

        await dataContext.SaveChangesAsync(cancellationToken);
        return new AuthResponse(verification.Id);
    }

    [HttpPost, Route("/registration/organizer")]
    public async Task<AuthResponse> OrganizerUserRegistration(OrganizerUserRegistrationRequest request,
                                                              CancellationToken cancellationToken,
                                                              [FromServices] DataContext dataContext)
    {
        await CheckUserAsync(request.Email, dataContext, cancellationToken);

        var company = await dataContext.Companies
                                       .FirstOrDefaultAsync(x => x.Title == request.CompanyName, cancellationToken) ??
                      new Company
                      {
                          Title = request.CompanyName,
                      };

        var (user, verification) = CreateUser<OrganizerUser>(UniversalCode, CommunicationType.Email, request.Email);
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Company = company;
        dataContext.Users.Add(user);

        await dataContext.SaveChangesAsync(cancellationToken);
        return new AuthResponse(verification.Id);
    }

    private Verification AddVerification(Communication communication, string code)
    {
        var verification = new Verification { Code = code };
        communication.Verifications.Add(verification);

        return verification;
    }

    private async Task CheckUserAsync(string email, DataContext dataContext, CancellationToken cancellationToken)
    {
        var user = await dataContext.Users
                                    .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (user != null)
        {
            throw new RestException("Такой пользователь уже существует", HttpStatusCode.BadRequest);
        }
    }

    private (TUser, Verification) CreateUser<TUser>(string code,
                                                    CommunicationType communicationType,
                                                    string communicationValue)
        where TUser : User, new()
    {
        var communication = new Communication
        {
            Value = communicationValue,
            Type = communicationType,
        };
        var verification = AddVerification(communication, code);
        var user = new TUser
        {
            Communications = new[] { communication },
        };

        return (user, verification);
    }
}