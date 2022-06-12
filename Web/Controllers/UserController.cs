using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Extensions;
using Web.Services;
using Web.ViewModels;
using Web.ViewModels.User;

namespace Web.Controllers;

[Route("user")]
[ApiController, Authorize]
public class UserController : ControllerBase
{
    [HttpGet, Route("profile")]
    public async Task<ProfileResponse> GetProfile(CancellationToken cancellationToken,
                                                  [FromServices] DataContext dataContext)
    {
        var userId = Guid.Parse(User.Identity!.Name!);
        var user = await dataContext.Users
                                    .Include(x => x.Avatar)
                                    .Include(x => x.Communications)
                                    .Include(x => x.UserActivities)
                                    .ThenInclude(x => x.Activity)
                                    .FirstAsync(x => x.Id == userId, cancellationToken);

        var allInterests = await dataContext.Activities.ToArrayAsync(cancellationToken);
        var userInterestIds = user.UserActivities.Select(x => x.Activity.Id).ToArray();

        return new ProfileResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
            Birthday = user.Birthday,
            Email = user.Communications.FirstOrDefault(x => x.Type == CommunicationType.Email)?.Value,
            Phone = user.Communications.FirstOrDefault(x => x.Type == CommunicationType.Phone)?.Value,
            AvatarId = user.Avatar?.Id,
            Gender = user.Gender,
            Location = user.Address == null ? null : AddressDto.Projection.Compile()(user.Address),
            SocialNetworks = user.SocialNetworks,
            Languages = user.Languages,
            Education = user.Education,
            Interests = allInterests.Select(x => new InterestModel
            {
                Id = x.Id,
                Title = x.Title,
                Enable = userInterestIds.Contains(x.Id),
            }).ToArray(),
        };
    }

    [HttpPost, Route("profile")]
    public async Task SaveProfile(ProfileRequest model,
                                  CancellationToken cancellationToken,
                                  [FromServices] DataContext dataContext)
    {
        var userId = User.GetUserId();
        var user = dataContext.Users
                              .Include(x => x.Communications)
                              .Include(x => x.UserActivities)
                              .ThenInclude(x => x.Activity)
                              .First(x => x.Id == userId);

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Patronymic = model.Patronymic;
        user.Birthday = model.Birthday;
        user.Gender = model.Gender;
        user.Address = model.Location?.Address;
        user.SocialNetworks = model.SocialNetworks;
        user.Languages = model.Languages;
        user.Education = model.Education;

        ChangeCommunication(user.Communications, CommunicationType.Email, model.Email);
        ChangeCommunication(user.Communications, CommunicationType.Phone, model.Phone);

        var userInterests = user.UserActivities.Select(x => x.ActivityId).ToArray();
        var toAdd = model.ActivityIds.Except(userInterests).ToList();
        var toRemove = userInterests.Except(model.ActivityIds).ToList();
        toAdd.ForEach(x => user.UserActivities.Add(new UserActivity { ActivityId = x }));
        toRemove.ForEach(x => user.UserActivities.Remove(user.UserActivities.First(y => y.ActivityId == x)));

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    [Authorize]
    [HttpPut("avatar")]
    public async Task<object> ChangeAvatar([FromForm] ChangeAvatarRequest request,
                                           [FromServices] DataContext dataContext,
                                           CancellationToken cancellationToken)
    {
        var fileService = new FileService();

        var userId = User.GetUserId();
        var user = await dataContext.Users
                                    .Include(x => x.Avatar)
                                    .SingleAsync(x => x.Id == userId, cancellationToken);

        var userFile = await fileService.CreateFileAsync("users", request.File, cancellationToken);
        if (user.AvatarId != null)
        {
            fileService.DeleteFile(user.Avatar!);
            dataContext.Files.Remove(user.Avatar!);
        }

        user.Avatar = userFile;
        await dataContext.SaveChangesAsync(cancellationToken);

        return new
        {
            Path = $"file/{user.Avatar.Path}",
        };
    }

    private void ChangeCommunication(ICollection<Communication> communications,
                                     CommunicationType communicationType,
                                     string? communicationValue)
    {
        var emailCommunication = communications.FirstOrDefault(x => x.Type == communicationType);
        if (emailCommunication == null)
        {
            if (communicationValue != null)
            {
                communications.Add(new Communication
                {
                    Type = communicationType,
                    Value = communicationValue,
                });
            }
        }
        else
        {
            if (communicationValue != null)
            {
                emailCommunication.Value = communicationValue;
            }
            else
            {
                communications.Remove(emailCommunication);
            }
        }
    }
}