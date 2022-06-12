using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.ViewModels.Events;

namespace Web.Controllers;

[Route("main")]
[ApiController, AllowAnonymous]
[Consumes(MediaTypeNames.Application.Json), Produces(MediaTypeNames.Application.Json)]
public class MainController : ControllerBase
{
    private readonly DataContext _dbContext;

    public MainController(DataContext dbContext) =>
        _dbContext = dbContext;

    [HttpGet]
    [Route("events-reviews")]
    public async Task<ICollection<ReviewDto>> ListEventReviews(int limit = 10,
                                                               CancellationToken cancellationToken = default) =>
        await _dbContext.Events
                        .AsNoTracking()
                        .SelectMany(x => x.EventReviews)
                        .OrderByDescending(r => r.ReviewScores.Count(x => x.Positive))
                        .ThenByDescending(x => x.CreatedAt)
                        .Take(limit)
                        .Select(ReviewDto.Projection(Guid.Empty))
                        .ToListAsync(cancellationToken);
}