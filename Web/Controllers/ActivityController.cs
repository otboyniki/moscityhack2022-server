using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Data.Entities;
using Web.Exceptions;
using Web.ViewModels.Activities;

namespace Web.Controllers;

[Route("activities")]
[ApiController, AllowAnonymous]
public class ActivityController : ControllerBase
{
    [HttpGet, Route("")]
    public async Task<GetActivitiesResponse> GetActivities([FromQuery] GetActivitiesRequest request,
                                                           [FromServices] DataContext dataContext,
                                                           CancellationToken cancellationToken)
    {
        return new GetActivitiesResponse
        {
            ActivityItems = await dataContext.Activities.Select(x => GetActivityItem(x)).ToArrayAsync(cancellationToken),
        };
    }

    [HttpGet, Route("{id}")]
    public async Task<ActivityItem> GetActivity([FromRoute] Guid id,
                                                [FromServices] DataContext dataContext,
                                                CancellationToken cancellationToken)
    {
        var activity = await dataContext.Activities.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (activity == null)
        {
            throw new RestException("Вид деятельности не найден", HttpStatusCode.NotFound);
        }

        return GetActivityItem(activity);
    }

    private static ActivityItem GetActivityItem(Activity activity) =>
        new()
        {
            Id = activity.Id,
            Title = activity.Title,
            IconPath = activity.Icon == null ? null : $"file/{activity.Icon.Path}",
        };
}