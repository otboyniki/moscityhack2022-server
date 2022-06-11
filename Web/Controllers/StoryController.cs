using System.Linq.Expressions;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Data.Entities;
using Web.Exceptions;
using Web.Extensions;
using Web.ViewModels.Story;

namespace Web.Controllers;

[Route("stories")]
[ApiController, Authorize]
public class StoryController : ControllerBase
{
    [HttpGet, Route("")]
    public async Task<StoryItemsResponse> GetStoryItems([FromQuery] StoryItemsRequest request,
                                                        [FromServices] DataContext dataContext,
                                                        CancellationToken cancellationToken)
    {
        StoryItemsValidate(request);
        var filteredStoryItems = await GetFilteredStoryItemsAsync(request, dataContext);

        var response = new StoryItemsResponse();
        if (request.Filters.Skip == 0)
        {
            response.BigItem = await filteredStoryItems.Select(GetStoryItemSelector()).FirstOrDefaultAsync(cancellationToken);
            response.Items = await filteredStoryItems.Skip(1).Select(GetStoryItemSelector()).ToArrayAsync(cancellationToken);
        }
        else
        {
            response.Items = await filteredStoryItems.Select(GetStoryItemSelector()).ToArrayAsync(cancellationToken);
        }

        return response;
    }

    [HttpGet, Route("{id}")]
    public async Task<StoryDetailResponse> DetailStory([FromRoute] Guid id,
                                                       [FromServices] DataContext dataContext,
                                                       CancellationToken cancellationToken)
    {
        var story = dataContext.Stories
                               .Include(x => x.Company)
                               .Include(x => x.Comments)
                               .ThenInclude(x => x.ReviewScores)
                               .Include(x => x.Comments)
                               .ThenInclude(x => x.User)
                               .ThenInclude(x => x.Avatar)
                               .Include(x => x.StoryScores)
                               .Include(x => x.StoryViews)
                               .FirstOrDefault(x => x.Id == id);
        if (story == null)
        {
            throw new RestException("История не найдена", HttpStatusCode.NotFound);
        }

        var userId = HttpContext.User.GetUserId()!.Value;
        if (story.StoryViews.All(x => x.UserId != userId))
        {
            story.StoryViews.Add(new StoryView { UserId = userId });
            await dataContext.SaveChangesAsync(cancellationToken);
        }

        return new StoryDetailResponse
        {
            CompanyName = story.Company.Title,
            Date = story.CreatedAt,
            Score = story.StoryScores.Count,
            IsPositiveScore = story.StoryScores.FirstOrDefault(x => x.UserId == userId)?.Positive,
            Title = story.Title,
            Description = story.Description,
            Comments = story.Comments.Select(x => new StoryCommentItem
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
    public async Task<StoryNewResponse> AddStory(StoryNewRequest request,
                                                 [FromServices] DataContext dataContext,
                                                 CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.GetUserId();
        var user = (OrganizerUser)await dataContext.Users
                                                   .Include(x => ((OrganizerUser)x).Company)
                                                   .FirstAsync(x => x.Id == userId, cancellationToken);

        var story = new Story
        {
            CompanyId = user.Company.Id,
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            Format = request.Format,
            StoryActivities = request.ActivityIds.Select(x => new StoryActivity { ActivityId = x }).ToArray(),
            PreviewId = request.PreviewId,
        };

        await dataContext.Stories.AddAsync(story, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);

        return new StoryNewResponse();
    }

    private static Expression<Func<Story, StoryItem>> GetStoryItemSelector()
    {
        return story => new StoryItem
        {
            Id = story.Id,
            CompanyName = story.Company.Title,
            Title = story.Title,
            ShortDescription = story.ShortDescription,
            Score = story.StoryScores.Sum(x => x.Positive ? 1 : -1),
            CommentsCount = story.Comments.Count,
            ViewsCount = story.StoryViews.Count,
            PreviewId = story.PreviewId,
            Format = story.Format,
            Activities = story.StoryActivities.Select(x => new StoryItemsActivityItem
            {
                Title = x.Activity.Title,
                IconId = x.Activity.Icon == null ? null : x.Activity.Icon.Id,
            }).ToArray(),
            Date = story.CreatedAt,
        };
    }

    private static async Task<IQueryable<Story>> GetFilteredStoryItemsAsync(StoryItemsRequest request,
                                                                            DataContext dataContext)
    {
        var storyQuery = dataContext.Stories.AsQueryable();
        var selector = (Expression<Func<Story, int>>)(request.Sort.Value switch
        {
            StorySortValue.Score => x => x.StoryScores.Count,
            StorySortValue.Views => x => x.StoryViews.Count,
            _ => throw new ArgumentOutOfRangeException(),
        });
        storyQuery = (request.Sort.IsAscending
            ? storyQuery.OrderBy(selector)
            : storyQuery.OrderByDescending(selector)).AsQueryable();

        if (request.Filters.FromDate != null)
        {
            storyQuery = storyQuery.Where(x => x.CreatedAt >= request.Filters.FromDate);
        }

        if (request.Filters.ToDate != null)
        {
            storyQuery = storyQuery.Where(x => x.CreatedAt <= request.Filters.ToDate);
        }

        if (request.Filters.FromScore != null)
        {
            storyQuery = storyQuery.Where(x => x.StoryScores.Count >= request.Filters.FromScore);
        }

        if (request.Filters.ToScore != null)
        {
            storyQuery = storyQuery.Where(x => x.StoryScores.Count <= request.Filters.ToScore);
        }

        if (request.Filters.InterestIds.Any())
        {
            storyQuery = storyQuery.Where(x => x.StoryActivities.Any(y => request.Filters.InterestIds.Contains(y.ActivityId)));
        }

        storyQuery = storyQuery.Skip(request.Filters.Skip)
                               .Take(request.Filters.Take);

        return storyQuery;
    }

    private void StoryItemsValidate(StoryItemsRequest request)
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