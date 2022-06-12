using System.Net;
using System.Net.Mime;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Data.Entities;
using Web.Exceptions;
using Web.Extensions;
using Web.ViewModels;
using Web.ViewModels.Events;

namespace Web.Controllers;

[Route("events")]
[ApiController]
[Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
public class EventController : ControllerBase
{
    private readonly DataContext _dbContext;

    public EventController(DataContext dbContext) =>
        _dbContext = dbContext;

    #region Events

    [HttpGet]
    public async Task<Page<EventDto>> ListEvents([FromQuery] ListEventsRequest request,
                                                 CancellationToken cancellationToken) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .Where(x => request.StringLocation == null ||
                                    x.Locations
                                     .Any(l => EF.Functions.ILike(l.StringLocation!, $"%{request.StringLocation}%")))
                        .Where(x => request.Activities == null ||
                                    request.Activities.Contains(x.ActivityId))
                        .Where(x => !request.Since.HasValue ||
                                    request.Since <= x.Meeting.Since)
                        .Where(x => !request.Until.HasValue ||
                                    x.Meeting.Until <= request.Until)
                        .Where(x => !request.IsArchived.HasValue ||
                                    (request.IsArchived.Value && x.Meeting.Until < DateTime.UtcNow) ||
                                    (!request.IsArchived.Value && DateTime.UtcNow <= x.Meeting.Since))
                        .Where(e => !request.IsOnline.HasValue ||
                                    e.Specializations
                                     .Any(x => x.IsOnline == request.IsOnline))
                        .Where(e => !request.FromAge.HasValue ||
                                    e.Specializations
                                     .Any(x => x.Ages == null ||
                                               request.FromAge.Value <= x.Ages!.From))
                        .Where(e => !request.ToAge.HasValue ||
                                    e.Specializations
                                     .Any(x => x.Ages == null ||
                                               x.Ages!.To <= request.ToAge.Value))
                        .OrderByDescending(x => x.CreatedAt)
                        .Select(EventDto.ConditionalProjection(User.GetUserId(), _ => true))
                        .PaginateAsync(request, cancellationToken);

    [HttpPost]
    [Authorize(Roles = nameof(OrganizerUser))]
    public async Task<Entity> CreateEvent([FromBody] CreateEventRequest request,
                                          CancellationToken cancellationToken)
    {
        if (request.Specialization == null)
        {
            throw new RestException("Событие должно иметь одну вакансию", HttpStatusCode.UnprocessableEntity);
        }

        var evt = new Event
        {
            CompanyId = User.FindFirst(nameof(Company))
                            ?.Value
                            .Let(Guid.Parse)
                        ?? throw new RestException("Кажется тебе сюда нельзя, дружок", HttpStatusCode.Forbidden),

            PreviewId = request.PreviewId,
            ActivityId = request.ActivityId,
            Title = request.Title,
            Description = request.Description,
            Terms = request.Terms,

            Locations = request.Locations
                               .Select(x => x.Address)
                               .ToList(),

            Recruitment = request.Recruitment,
            Meeting = request.Meeting,
            MeetingNote = request.MeetingNote,

            Specializations = new List<EventSpecialization>
            {
                new()
                {
                    Title = request.Specialization.Title,
                    Requirements = request.Specialization.Requirements,
                    Description = request.Specialization.Description,

                    IsOnline = request.Specialization.IsOnline,
                    Ages = request.Specialization.Ages,

                    MinVolunteersNumber = request.Specialization.MinVolunteersNumber,
                    MaxVolunteersNumber = request.Specialization.MaxVolunteersNumber,
                    IsRegisteredVolunteersNeeded = request.Specialization.IsRegisteredVolunteersNeeded
                }
            }
        };

        _dbContext.Add(evt);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Entity.From(evt);
    }

    [HttpGet]
    [Route("{eventId:guid}")]
    public async Task<EventDto> ReadEvent([FromRoute] Guid eventId,
                                          CancellationToken cancellationToken) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .Where(x => x.Id == eventId)
                        .Select(EventDto.ConditionalProjection(User.GetUserId(), _ => true))
                        .FirstOrDefaultAsync(cancellationToken)
        ?? throw new RestException("Мероприятие не найдено", HttpStatusCode.NotFound);

    [HttpPatch]
    [Route("{eventId:guid}")]
    [Authorize(Roles = nameof(OrganizerUser))]
    public async Task UpdateEvent([FromRoute] Guid eventId,
                                  [FromBody] UpdateEventRequest request,
                                  CancellationToken cancellationToken)
    {
        var evt = await _dbContext.Events
                                  .Where(x => x.Id == eventId)
                                  .FirstOrDefaultAsync(cancellationToken)
                  ?? throw new RestException("Мероприятие не найдено", HttpStatusCode.NotFound);

        evt.PreviewId = request.PreviewId;
        evt.ActivityId = request.ActivityId;
        evt.Title = request.Title;
        evt.Description = request.Description;
        evt.Terms = request.Terms;

        evt.Locations = request.Locations
                               .Select(x => x.Address)
                               .ToList();

        evt.Recruitment = request.Recruitment;
        evt.Meeting = request.Meeting;
        evt.MeetingNote = request.MeetingNote;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpDelete]
    [Route("{eventId:guid}")]
    [Authorize(Roles = nameof(OrganizerUser))]
    public async Task DeleteEvent([FromRoute] Guid eventId,
                                  CancellationToken cancellationToken)
    {
        var evt = await _dbContext.Events
                                  .Where(x => x.Id == eventId)
                                  .FirstOrDefaultAsync(cancellationToken)
                  ?? throw new RestException("Мероприятие не найдено", HttpStatusCode.NotFound);

        _dbContext.Remove(evt);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Specializations

    [HttpPost]
    [Route("{eventId:guid}/specializations")]
    [Authorize(Roles = nameof(OrganizerUser))]
    public async Task<Entity> CreateEventSpecialization([FromRoute] Guid eventId,
                                                        [FromBody] UpdateEventSpecializationRequest request,
                                                        CancellationToken cancellationToken)
    {
        var spec = new EventSpecialization
        {
            EventId = eventId,

            Title = request.Title,
            Requirements = request.Requirements,
            Description = request.Description,

            IsOnline = request.IsOnline,
            Ages = request.Ages,

            MinVolunteersNumber = request.MinVolunteersNumber,
            MaxVolunteersNumber = request.MaxVolunteersNumber,
            IsRegisteredVolunteersNeeded = request.IsRegisteredVolunteersNeeded,
        };

        _dbContext.Add(spec);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Entity.From(spec);
    }

    [HttpPatch]
    [Route("{eventId:guid}/specializations/{specializationId:guid}")]
    [Authorize(Roles = nameof(OrganizerUser))]
    public async Task UpdateEventSpecialization([FromRoute] Guid eventId,
                                                [FromRoute] Guid specializationId,
                                                [FromBody] UpdateEventSpecializationRequest request,
                                                CancellationToken cancellationToken)
    {
        var spec = await _dbContext.Events
                                   .Where(x => x.Id == eventId)
                                   .SelectMany(x => x.Specializations)
                                   .Where(x => x.Id == specializationId)
                                   .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new RestException("Специализация не найдена", HttpStatusCode.NotFound);

        spec.Title = request.Title;
        spec.Requirements = request.Requirements;
        spec.Description = request.Description;

        spec.IsOnline = request.IsOnline;
        spec.Ages = request.Ages;

        spec.MinVolunteersNumber = request.MinVolunteersNumber;
        spec.MaxVolunteersNumber = request.MaxVolunteersNumber;
        spec.IsRegisteredVolunteersNeeded = request.IsRegisteredVolunteersNeeded;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpDelete]
    [Route("{eventId:guid}/specializations/{specializationId:guid}")]
    [Authorize(Roles = nameof(OrganizerUser))]
    public async Task DeleteEventSpecialization([FromRoute] Guid eventId,
                                                [FromRoute] Guid specializationId,
                                                CancellationToken cancellationToken)
    {
        var spec = await _dbContext.Events
                                   .Where(x => x.Id == eventId)
                                   .SelectMany(x => x.Specializations)
                                   .Where(x => x.Id == specializationId)
                                   .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new RestException("Специализация не найдена", HttpStatusCode.NotFound);

        var count = await _dbContext.Events
                                    .AsNoTracking()
                                    .Where(x => x.Id == eventId)
                                    .SelectMany(x => x.Specializations)
                                    .CountAsync(cancellationToken);

        if (count == 1)
        {
            throw new RestException("Событие лолжно иметь как минимум одну вакансию", HttpStatusCode.Conflict);
        }

        _dbContext.Remove(spec);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpGet]
    [Route("/{eventId:guid}/specializations/{specializationId:guid}/qr/invite")]
    public async Task<IActionResult> CreateQrForInvite([FromRoute] Guid eventId,
                                                       [FromRoute] Guid specializationId,
                                                       CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!.Value;
        var user = (OrganizerUser)await _dbContext.Users
                                                  .Include(x => ((OrganizerUser)x).Company)
                                                  .FirstAsync(x => x.Id == userId, cancellationToken);

        var specialization = await _dbContext.Events
                                             .Where(x => x.Id == eventId)
                                             .SelectMany(x => x.Specializations)
                                             .Include(x => x.Event)
                                             .ThenInclude(x => x.Company)
                                             .FirstOrDefaultAsync(x => x.Id == specializationId, cancellationToken)
                             ?? throw new RestException("Мероприятие не найдено", HttpStatusCode.NotFound);

        if (user.CompanyId != specialization.Event.CompanyId)
        {
            throw new RestException("Не твоя организация", HttpStatusCode.Forbidden);
        }

        var url = $"otboyniki-moscityhack2022.ru/quick-registration?eventId={eventId}&specializationId={specializationId}";
        return Redirect($"http://qrcoder.ru/code/?{UrlEncoder.Default.Encode(url!)}&10&3");
    }

    [HttpGet]
    [Route("{eventId:guid}/specializations/{specializationId:guid}/qr")]
    [Authorize(Roles = nameof(VolunteerUser))]
    public async Task<IActionResult> ReadEventSpecializationQr([FromRoute] Guid eventId,
                                                               [FromRoute] Guid specializationId,
                                                               CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!.Value;
        var spec = await _dbContext.Events
                                   .AsNoTracking()
                                   .Where(x => x.Id == eventId)
                                   .SelectMany(x => x.Specializations)
                                   .Where(x => x.Id == specializationId)
                                   .Select(x => new { x.Id, x.EventId })
                                   .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new RestException("Мероприятие не найдено", HttpStatusCode.NotFound);

        var url = Url.Action(
            action: nameof(MarkEventSpecializationVolunteerVisited),
            values: new
            {
                spec.EventId,
                SpecializationId = spec.Id,
                VolunteerId = userId
            },
            controller: null,
            protocol: null,
            host: "api.otboyniki-moscityhack2022.ru");

        return Redirect($"http://qrcoder.ru/code/?{UrlEncoder.Default.Encode(url!)}&10&3");
    }

    [HttpPost]
    [Route("{eventId:guid}/specializations/{specializationId:guid}/join")]
    [Authorize(Roles = nameof(VolunteerUser))]
    public async Task JoinEventSpecialization([FromRoute] Guid eventId,
                                              [FromRoute] Guid specializationId,
                                              CancellationToken cancellationToken)
    {
        var spec = await _dbContext.Events
                                   .Where(x => x.Id == eventId)
                                   .SelectMany(x => x.Specializations)
                                   .Where(x => x.Id == specializationId)
                                   .Include(x => x.Event)
                                   .Include(x => x.Participants)
                                   .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new RestException("Специализация не найдена", HttpStatusCode.NotFound);

        if (spec.Event.Recruitment is not null)
        {
            if (DateTime.UtcNow < spec.Event.Recruitment.Since)
            {
                throw new RestException("Набор ещё не начался", HttpStatusCode.UnprocessableEntity);
            }

            if (spec.Event.Recruitment.Until < DateTime.UtcNow)
            {
                throw new RestException("Набор уже закончился", HttpStatusCode.UnprocessableEntity);
            }
        }

        if (spec.Participants.Any(x => x.VolunteerId == User.GetUserId()!))
        {
            throw new RestException("Этот волонёр уже присоединился", HttpStatusCode.Conflict);
        }

        spec.Participants.Add(new Participation
        {
            VolunteerId = User.GetUserId()!.Value,
            IsMember = spec.Participants.Count < spec.MaxVolunteersNumber
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpPost]
    [Route("{eventId:guid}/specializations/{specializationId:guid}/confirm")]
    [Authorize(Roles = nameof(VolunteerUser))]
    public async Task ConfirmEventSpecialization([FromRoute] Guid eventId,
                                                 [FromRoute] Guid specializationId,
                                                 CancellationToken cancellationToken)
    {
        var participation = await _dbContext.Events
                                            .Where(x => x.Id == eventId)
                                            .SelectMany(x => x.Specializations)
                                            .Where(x => x.Id == specializationId)
                                            .SelectMany(x => x.Participants)
                                            .Where(x => x.VolunteerId == User.GetUserId())
                                            .FirstOrDefaultAsync(cancellationToken)
                            ?? throw new RestException("Запись об участии не найдена", HttpStatusCode.NotFound);

        participation.IsConfirmed = true;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpGet, ApiExplorerSettings(IgnoreApi = true)]
    [Route("{eventId:guid}/specializations/{specializationId:guid}/mark-visited")]
    [Authorize(Roles = nameof(OrganizerUser))]
    [Produces(MediaTypeNames.Text.Plain)]
    public async Task<IActionResult> MarkEventSpecializationVolunteerVisited([FromRoute] Guid eventId,
                                                                             [FromRoute] Guid specializationId,
                                                                             [FromQuery] Guid volunteerId,
                                                                             CancellationToken cancellationToken)
    {
        var participation = await _dbContext.Events
                                            .Where(x => x.Id == eventId)
                                            .SelectMany(x => x.Specializations)
                                            .Where(x => x.Id == specializationId)
                                            .SelectMany(x => x.Participants)
                                            .Where(x => x.VolunteerId == volunteerId)
                                            .FirstOrDefaultAsync(cancellationToken);

        if (participation is null)
        {
            return Ok("Запись об участии не найдена");
        }

        participation.IsVisited = true;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok("Участии волонтёра в мероприятии подтверждено");
    }

    [HttpPost]
    [Route("{eventId:guid}/specializations/{specializationId:guid}/leave")]
    [Authorize(Roles = nameof(VolunteerUser))]
    public async Task LeaveEventSpecialization([FromRoute] Guid eventId,
                                               [FromRoute] Guid specializationId,
                                               CancellationToken cancellationToken)
    {
        var spec = await _dbContext.Events
                                   .Where(x => x.Id == eventId)
                                   .SelectMany(x => x.Specializations)
                                   .Where(x => x.Id == specializationId)
                                   .Include(x => x.Participants)
                                   .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new RestException("Специализация не найдена", HttpStatusCode.NotFound);

        var participation = spec.Participants
                                .FirstOrDefault(x => x.VolunteerId == User.GetUserId());

        if (participation != null)
        {
            _dbContext.Remove(participation);

            if (participation.IsMember)
            {
                var oldest = spec.Participants
                                 .Where(x => !x.IsMember &&
                                             x.Id != participation.Id)
                                 .MinBy(x => x.CreatedAt);

                if (oldest is not null)
                {
                    oldest.IsMember = true;
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    #endregion

    #region Reviews

    [HttpGet]
    [Route("{eventId:guid}/reviews")]
    public async Task<Page<ReviewDto>> ListEventReviews([FromRoute] Guid eventId,
                                                        [FromQuery] ListEventsRequest request,
                                                        CancellationToken cancellationToken) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .Where(x => x.Id == eventId)
                        .SelectMany(x => x.EventReviews)
                        .OrderByDescending(x => x.CreatedAt)
                        .Select(ReviewDto.Projection(User.GetUserId()!.Value))
                        .PaginateAsync(request, cancellationToken);

    [HttpPost]
    [Route("{eventId:guid}/reviews")]
    [Authorize(Roles = nameof(VolunteerUser))]
    public async Task<Entity> CreateEventReview([FromRoute] Guid eventId,
                                                [FromBody] UpdateReviewRequest request,
                                                CancellationToken cancellationToken)
    {
        var review = new EventReview
        {
            EventId = eventId,
            UserId = User.GetUserId()!.Value,

            Text = request.Text,
            CompanyRate = request.CompanyRate,
            GoalComplianceRate = request.GoalComplianceRate
        };

        _dbContext.Add(review);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Entity.From(review);
    }

    [HttpGet]
    [Route("{eventId:guid}/reviews/{reviewId:guid}")]
    public async Task<ReviewDto> ReadEventReview([FromRoute] Guid eventId,
                                                 [FromRoute] Guid reviewId,
                                                 CancellationToken cancellationToken) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .Where(x => x.Id == eventId)
                        .SelectMany(x => x.EventReviews)
                        .Where(x => x.Id == reviewId)
                        .Select(ReviewDto.Projection(User.GetUserId()!.Value))
                        .FirstOrDefaultAsync(cancellationToken)
        ?? throw new RestException("Отзыв не найден", HttpStatusCode.NotFound);

    [HttpPatch]
    [Route("{eventId:guid}/reviews/{reviewId:guid}")]
    [Authorize(Roles = nameof(VolunteerUser))]
    public async Task UpdateEventReview([FromRoute] Guid eventId,
                                        [FromRoute] Guid reviewId,
                                        [FromBody] UpdateReviewRequest request,
                                        CancellationToken cancellationToken)
    {
        var review = await _dbContext.Events
                                     .Where(x => x.Id == eventId)
                                     .SelectMany(x => x.EventReviews)
                                     .Where(x => x.Id == reviewId)
                                     .FirstOrDefaultAsync(cancellationToken)
                     ?? throw new RestException("Отзыв не найден", HttpStatusCode.NotFound);

        review.Text = request.Text;
        review.CompanyRate = request.CompanyRate;
        review.GoalComplianceRate = request.GoalComplianceRate;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpDelete]
    [Route("{eventId:guid}/reviews/{reviewId:guid}")]
    [Authorize(Roles = nameof(VolunteerUser))]
    public async Task DeleteEventReview([FromRoute] Guid eventId,
                                        [FromRoute] Guid reviewId,
                                        CancellationToken cancellationToken)
    {
        var review = await _dbContext.Events
                                     .Where(x => x.Id == eventId)
                                     .SelectMany(x => x.EventReviews)
                                     .Where(x => x.Id == reviewId)
                                     .FirstOrDefaultAsync(cancellationToken)
                     ?? throw new RestException("Отзыв не найден", HttpStatusCode.NotFound);

        _dbContext.Remove(review);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpPost]
    [Route("{eventId:guid}/reviews/{reviewId:guid}/like")]
    [Authorize(Roles = nameof(VolunteerUser))]
    public async Task LikeEventReview([FromRoute] Guid eventId,
                                      [FromRoute] Guid reviewId,
                                      CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!.Value;
        var review = await _dbContext.Events
                                     .Where(x => x.Id == eventId)
                                     .SelectMany(x => x.EventReviews)
                                     .Where(x => x.Id == reviewId)
                                     .Include(r => r.ReviewScores
                                                    .Where(x => x.UserId == userId))
                                     .FirstOrDefaultAsync(cancellationToken)
                     ?? throw new RestException("Отзыв не найден", HttpStatusCode.NotFound);

        var myScore = review.ReviewScores
                            .FirstOrDefault(x => x.UserId == userId);

        switch (myScore)
        {
            case null:
                review.ReviewScores.Add(new ReviewScore
                {
                    UserId = userId,
                    ReviewId = reviewId,
                    Positive = true
                });
                break;

            case { Positive: false }:
                myScore.Positive = true;
                break;

            case { Positive: true }:
                _dbContext.Remove(myScore);
                break;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    [HttpPost]
    [Route("{eventId:guid}/reviews/{reviewId:guid}/dislike")]
    [Authorize(Roles = nameof(VolunteerUser))]
    public async Task DislikeEventReview([FromRoute] Guid eventId,
                                         [FromRoute] Guid reviewId,
                                         CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!.Value;
        var review = await _dbContext.Events
                                     .Where(x => x.Id == eventId)
                                     .SelectMany(x => x.EventReviews)
                                     .Where(x => x.Id == reviewId)
                                     .Include(r => r.ReviewScores
                                                    .Where(x => x.UserId == userId))
                                     .FirstOrDefaultAsync(cancellationToken)
                     ?? throw new RestException("Отзыв не найден", HttpStatusCode.NotFound);

        var myScore = review.ReviewScores
                            .FirstOrDefault(x => x.UserId == userId);

        switch (myScore)
        {
            case null:
                review.ReviewScores.Add(new ReviewScore
                {
                    UserId = userId,
                    ReviewId = reviewId,
                    Positive = false
                });
                break;

            case { Positive: false }:
                _dbContext.Remove(myScore);
                break;

            case { Positive: true }:
                myScore.Positive = false;
                break;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Participants

    [HttpGet]
    [Route("{eventId:guid}/participants")]
    [Authorize(Roles = nameof(OrganizerUser))]
    public async Task<object> ListParticipants([FromRoute] Guid eventId,
                                               CancellationToken cancellationToken) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .Where(x => x.Id == eventId)
                        .SelectMany(x => x.Specializations)
                        .SelectMany(x => x.Participants)
                        .GroupBy(x => new { x.EventSpecializationId })
                        .Select(g => new
                        {
                            g.Key.EventSpecializationId,
                            Volunteers = g.Select(x => new
                                          {
                                              ParticipantId = x.Id,

                                              x.IsConfirmed,
                                              x.IsVisited,
                                              x.IsMember,

                                              Volunteer = new
                                              {
                                                  VolunteerId = x.Volunteer.Id,
                                                  x.Volunteer.FirstName,
                                                  x.Volunteer.LastName,
                                                  x.Volunteer.Patronymic,

                                                  Communications = x.Volunteer
                                                                    .Communications
                                                                    .Select(c => new
                                                                    {
                                                                        c.Type,
                                                                        c.Value
                                                                    })
                                                                    .ToList()
                                              }
                                          })
                                          .ToList()
                        })
                        .ToDictionaryAsync(
                            x => x.EventSpecializationId,
                            x => x.Volunteers,
                            cancellationToken);

    [HttpPost]
    [Route("{eventId:guid}/participants/{participantId:guid}/mark-visited")]
    [Authorize(Roles = nameof(OrganizerUser))]
    public async Task MarkParticipantVisited([FromRoute] Guid eventId,
                                             [FromRoute] Guid participantId,
                                             [FromBody] bool isVisited,
                                             CancellationToken cancellationToken)
    {
        var participation = await _dbContext.Events
                                            .Where(x => x.Id == eventId)
                                            .SelectMany(x => x.Specializations)
                                            .SelectMany(x => x.Participants)
                                            .Where(x => x.Id == participantId)
                                            .FirstOrDefaultAsync(cancellationToken)
                            ?? throw new RestException("Запись об участии не найдена", HttpStatusCode.NotFound);

        participation.IsVisited = isVisited;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion
}