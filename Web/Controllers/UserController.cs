using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Exceptions;
using Web.Extensions;
using Web.ViewModels;
using Web.ViewModels.Events;
using Web.ViewModels.User;

namespace Web.Controllers;

[Route("user")]
[ApiController, Authorize]
public class UserController : ControllerBase
{
    private readonly DataContext _dbContext;

    public UserController(DataContext dbContext) =>
        _dbContext = dbContext;

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

        var profileType = user switch
        {
            OrganizerUser => ProfileType.Organizer,
            VolunteerUser => ProfileType.Volunteer,
            _ => throw new ArgumentOutOfRangeException(),
        };

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
            ProfileType = profileType,
        };
    }

    [HttpPost, Route("profile")]
    public async Task SaveProfile(ProfileRequest request,
                                  CancellationToken cancellationToken,
                                  [FromServices] DataContext dataContext)
    {
        var userId = User.GetUserId();
        var user = dataContext.Users
                              .Include(x => x.Communications)
                              .Include(x => x.UserActivities)
                              .ThenInclude(x => x.Activity)
                              .First(x => x.Id == userId);

        var avatar = await dataContext.Files
                                      .FirstOrDefaultAsync(x => x.Id == request.AvatarId, cancellationToken)
                     ?? throw new RestException("???????? ???? ????????????", HttpStatusCode.NotFound);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Patronymic = request.Patronymic;
        user.Birthday = request.Birthday;
        user.Gender = request.Gender;
        user.Address = request.Location?.Address;
        user.SocialNetworks = request.SocialNetworks;
        user.Languages = request.Languages;
        user.Education = request.Education;
        user.AvatarId = avatar.Id;

        ChangeCommunication(user.Communications, CommunicationType.Email, request.Email);
        ChangeCommunication(user.Communications, CommunicationType.Phone, request.Phone);

        var userInterests = user.UserActivities.Select(x => x.ActivityId).ToArray();
        var toAdd = request.ActivityIds.Except(userInterests).ToList();
        var toRemove = userInterests.Except(request.ActivityIds).ToList();
        toAdd.ForEach(x => user.UserActivities.Add(new UserActivity { ActivityId = x }));
        toRemove.ForEach(x => user.UserActivities.Remove(user.UserActivities.First(y => y.ActivityId == x)));

        await dataContext.SaveChangesAsync(cancellationToken);
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

    [HttpGet]
    [Route("near-events")]
    public async Task<ICollection<EventDto>> ListNearEvents(CancellationToken cancellationToken) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .Where(e => e.Specializations
                                     .SelectMany(s => s.Participants)
                                     .Any(x => x.VolunteerId == User.GetUserId()))
                        .OrderBy(x => x.Meeting.Since)
                        .Select(EventDto.ConditionalProjection(
                                    User.GetUserId()!.Value,
                                    s => s.Participants
                                          .Any(x => x.VolunteerId == User.GetUserId())))
                        .ToListAsync(cancellationToken);

    [HttpGet]
    [Route("visited-events")]
    public async Task<ICollection<EventDto>> ListVisitedEvents(CancellationToken cancellationToken) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .Where(e => e.Specializations
                                     .SelectMany(s => s.Participants)
                                     .Any(x => x.VolunteerId == User.GetUserId() &&
                                               x.IsVisited))
                        .OrderByDescending(x => x.Meeting.Until)
                        .Select(EventDto.ConditionalProjection(
                                    User.GetUserId()!.Value,
                                    s => s.Participants
                                          .Any(x => x.VolunteerId == User.GetUserId() &&
                                                    x.IsVisited)))
                        .ToListAsync(cancellationToken);

    [HttpGet]
    [Route("event-reviews")]
    public async Task<ICollection<ReviewDto>> ListEventReviews(CancellationToken cancellationToken) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .SelectMany(x => x.EventReviews)
                        .Where(x => x.UserId == User.GetUserId())
                        .OrderByDescending(x => x.CreatedAt)
                        .Select(ReviewDto.Projection(User.GetUserId()!.Value))
                        .ToListAsync(cancellationToken);
}