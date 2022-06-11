using System.Linq.Expressions;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Data.Entities;
using Web.Exceptions;
using Web.Extensions;
using Web.ViewModels.History;

namespace Web.Controllers;

[Route("histories")]
[ApiController, Authorize]
public class HistoryController : ControllerBase
{
    [HttpGet, Route("")]
    public async Task<HistoryItemsResponse> GetHistoryItems([FromQuery] HistoryItemsRequest request,
                                                            [FromServices] DataContext dataContext,
                                                            CancellationToken cancellationToken)
    {
        HistoryItemsValidate(request);
        var filteredHistoryItems = await GetFilteredHistoryItemsAsync(request, dataContext);

        var response = new HistoryItemsResponse();
        if (request.Filters.Skip == 0)
        {
            response.BigItem = await filteredHistoryItems.Select(GetHistoryItemSelector()).FirstOrDefaultAsync(cancellationToken);
            response.Items = await filteredHistoryItems.Skip(1).Select(GetHistoryItemSelector()).ToArrayAsync(cancellationToken);
        }
        else
        {
            response.Items = await filteredHistoryItems.Select(GetHistoryItemSelector()).ToArrayAsync(cancellationToken);
        }

        return response;
    }

    [HttpGet, Route("{id}")]
    public async Task<HistoryDetailResponse> DetailHistory([FromRoute] Guid id,
                                                           [FromServices] DataContext dataContext,
                                                           CancellationToken cancellationToken)
    {
        var history = dataContext.Histories
                                 .Include(x => x.Company)
                                 .Include(x => x.Comments)
                                 .ThenInclude(x => x.ReviewScores)
                                 .Include(x => x.Comments)
                                 .ThenInclude(x => x.User)
                                 .ThenInclude(x => x.Avatar)
                                 .Include(x => x.HistoryScores)
                                 .Include(x => x.HistoryViews)
                                 .FirstOrDefault(x => x.Id == id);
        if (history == null)
        {
            throw new RestException("История не найдена", HttpStatusCode.NotFound);
        }

        var userId = HttpContext.User.GetUserId()!.Value;
        if (history.HistoryViews.All(x => x.UserId != userId))
        {
            history.HistoryViews.Add(new HistoryView { UserId = userId });
            await dataContext.SaveChangesAsync(cancellationToken);
        }

        return new HistoryDetailResponse
        {
            CompanyName = history.Company.Title,
            Date = history.CreatedAt,
            Score = history.HistoryScores.Count,
            IsPositiveScore = history.HistoryScores.FirstOrDefault(x => x.UserId == userId)?.Positive,
            Title = history.Title,
            Description = history.Description,
            Comments = history.Comments.Select(x => new HistoryCommentItem
            {
                FullName = $"{x.User.FirstName} {x.User.LastName}",
                Date = x.CreatedAt,
                Text = x.Text,
                Score = x.ReviewScores.Sum(y => y.Positive ? 1 : -1),
                IsPositiveScore = x.ReviewScores.FirstOrDefault(y => y.Review.UserId == userId)?.Positive,
                AvatarPath = x.User.Avatar == null ? null : $"file/{x.User.Avatar.Path}",
            }).ToArray(),
        };
    }

    [HttpPost, Route(""), Authorize(Roles = nameof(OrganizerUser))]
    public async Task<HistoryNewResponse> AddHistory(HistoryNewRequest request,
                                                     [FromServices] DataContext dataContext,
                                                     CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.GetUserId();
        var user = (OrganizerUser)await dataContext.Users
                                                   .Include(x => ((OrganizerUser)x).Company)
                                                   .FirstAsync(x => x.Id == userId, cancellationToken);

        var history = new History
        {
            CompanyId = user.Company.Id,
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            Format = request.Format,
            HistoryActivities = request.ActivityIds.Select(x => new HistoryActivity { ActivityId = x }).ToArray(),
            PreviewId = request.PreviewId,
        };

        await dataContext.Histories.AddAsync(history, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);

        return new HistoryNewResponse();
    }

    private static Expression<Func<History, HistoryItem>> GetHistoryItemSelector()
    {
        return history => new HistoryItem
        {
            Id = history.Id,
            CompanyName = history.Company.Title,
            Title = history.Title,
            ShortDescription = history.ShortDescription,
            Score = history.HistoryScores.Sum(x => x.Positive ? 1 : -1),
            CommentsCount = history.Comments.Count,
            ViewsCount = history.HistoryViews.Count,
            PreviewId = history.PreviewId,
            Format = history.Format,
            Activities = history.HistoryActivities.Select(x => new HistoryItemsActivityItem
            {
                Title = x.Activity.Title,
                IconId = x.Activity.Icon == null ? null : x.Activity.Icon.Id,
            }).ToArray(),
            Date = history.CreatedAt,
        };
    }

    private static async Task<IQueryable<History>> GetFilteredHistoryItemsAsync(HistoryItemsRequest request,
                                                                                DataContext dataContext)
    {
        var historyQuery = dataContext.Histories.AsQueryable();
        var selector = (Expression<Func<History, int>>)(request.Sort.Value switch
        {
            HistorySortValue.Score => x => x.HistoryScores.Count,
            HistorySortValue.Views => x => x.HistoryViews.Count,
            _ => throw new ArgumentOutOfRangeException(),
        });
        historyQuery = (request.Sort.IsAscending
            ? historyQuery.OrderBy(selector)
            : historyQuery.OrderByDescending(selector)).AsQueryable();

        if (request.Filters.FromDate != null)
        {
            historyQuery = historyQuery.Where(x => x.CreatedAt >= request.Filters.FromDate);
        }

        if (request.Filters.ToDate != null)
        {
            historyQuery = historyQuery.Where(x => x.CreatedAt <= request.Filters.ToDate);
        }

        if (request.Filters.FromScore != null)
        {
            historyQuery = historyQuery.Where(x => x.HistoryScores.Count >= request.Filters.FromScore);
        }

        if (request.Filters.ToScore != null)
        {
            historyQuery = historyQuery.Where(x => x.HistoryScores.Count <= request.Filters.ToScore);
        }

        if (request.Filters.InterestIds.Any())
        {
            historyQuery = historyQuery.Where(x => x.HistoryActivities.Any(y => request.Filters.InterestIds.Contains(y.ActivityId)));
        }

        historyQuery = historyQuery.Skip(request.Filters.Skip)
                                   .Take(request.Filters.Take);

        return historyQuery;
    }

    private void HistoryItemsValidate(HistoryItemsRequest request)
    {
        switch (request.Filters.Take)
        {
            case < 0:
                throw new RestException("Запрошено отрицательно количество записей", HttpStatusCode.UnprocessableEntity);
            case > 100:
                throw new RestException("Запрошено слишком записей", HttpStatusCode.UnprocessableEntity);
        }

        if (request.Filters.Skip < 0)
        {
            throw new RestException("Невозможно пропустить отрицательное количество записей", HttpStatusCode.UnprocessableEntity);
        }
    }
}