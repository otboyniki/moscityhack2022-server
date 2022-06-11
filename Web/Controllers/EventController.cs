using System.Net;
using System.Net.Mime;
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
[ApiController, Authorize]
[Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
public class EventController : ControllerBase
{
    private readonly DataContext _dbContext;

    public EventController(DataContext dbContext) =>
        _dbContext = dbContext;

    #region Events

    [HttpGet]
    public async Task<ICollection<EventDto>> ListEvents(CancellationToken cancellationToken) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt)
                        .Select(EventDto.Projection)
                        .ToListAsync(cancellationToken);

    [HttpPost]
    [Authorize(Roles = nameof(OrganizerUser))]
    public async Task<Entity> CreateEvent([FromBody] UpdateEventRequest request,
                                          CancellationToken cancellationToken)
    {
        var evt = new Event
        {
            CompanyId = User.FindFirst(nameof(Company))
                            ?.Value
                            .Let(Guid.Parse)
                        ?? throw new RestException("Кажется тебе сюда нельзя, дружок", HttpStatusCode.Forbidden),

            PreviewId = request.PreviewId,
            Title = request.Title,
            Description = request.Description,
            Terms = request.Terms,
            Kind = request.Kind,

            Locations = request.Locations
                               .Select(x => x.Address)
                               .ToList(),

            Recruitment = request.Recruitment,
            Meeting = request.Meeting,
            MeetingNote = request.MeetingNote,
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
                        .Select(EventDto.Projection)
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
        evt.Title = request.Title;
        evt.Description = request.Description;
        evt.Terms = request.Terms;
        evt.Kind = request.Kind;

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

        _dbContext.Remove(spec);
        await _dbContext.SaveChangesAsync(cancellationToken);
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
}