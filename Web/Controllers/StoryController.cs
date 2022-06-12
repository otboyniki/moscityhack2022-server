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
    [HttpGet]
    public async Task<StoryItemsResponse> GetStoryItems([FromQuery] StoryItemsRequest request,
                                                        [FromServices] DataContext dataContext,
                                                        CancellationToken cancellationToken)
    {
        StoryItemsValidate(request);
        var filteredStoryItems = GetFilteredStoryItems(request, dataContext);

        var response = new StoryItemsResponse
        {
            Items = await filteredStoryItems.Select(StoryDto.Projection).ToArrayAsync(cancellationToken),
        };

        return response;
    }

    [HttpGet, Route("/{storyId:guid}")]
    public async Task<StoryDetailResponse> DetailStory([FromRoute] Guid storyId,
                                                       [FromServices] DataContext dataContext,
                                                       CancellationToken cancellationToken)
    {
        var story = dataContext.Stories
                               .Include(x => ((CompanyStory)x).Company)
                               .Include(x => x.Comments)
                               .ThenInclude(x => x.ReviewScores)
                               .Include(x => x.Comments)
                               .ThenInclude(x => x.User)
                               .ThenInclude(x => x.Avatar)
                               .Include(x => x.StoryScores)
                               .Include(x => x.StoryViews)
                               .FirstOrDefault(x => x.Id == storyId);
        if (story == null)
        {
            throw new RestException("История не найдена", HttpStatusCode.NotFound);
        }

        var userId = User.GetUserId()!.Value;
        if (story.StoryViews.All(x => x.UserId != userId))
        {
            story.StoryViews.Add(new StoryView { UserId = userId });
            await dataContext.SaveChangesAsync(cancellationToken);
        }

        return new StoryDetailResponse
        {
            FullName = story.FullName,
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

    [HttpPost, Authorize(Roles = nameof(OrganizerUser))]
    public async Task<StoryNewResponse> CreateStory(StoryNewRequest request,
                                                    [FromServices] DataContext dataContext,
                                                    CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var user = await dataContext.Users
                                    .Include(x => ((OrganizerUser)x).Company)
                                    .FirstAsync(x => x.Id == userId, cancellationToken);

        Story story;
        switch (user)
        {
            case OrganizerUser organizerUser:
                var companyStory = CreateStory<CompanyStory>(request);
                companyStory.CompanyId = organizerUser.Company.Id;
                story = companyStory;
                break;
            case VolunteerUser volunteerUser:
                var userStory = CreateStory<CompanyStory>(request);
                story = userStory;
                break;
            default:
                throw new ArgumentException("Неопознанная роль пользователя");
        }

        await dataContext.Stories.AddAsync(story, cancellationToken);
        await dataContext.SaveChangesAsync(cancellationToken);

        return new StoryNewResponse();
    }

    [HttpPost, Route("/{storyId:guid}/comment")]
    public async Task<CreateCommentResponse> CreateComment([FromRoute] Guid storyId,
                                                           [FromBody] CreateCommentRequest request,
                                                           [FromServices] DataContext dataContext,
                                                           CancellationToken cancellationToken)
    {
        var story = dataContext.Stories
                               .Include(x => x.Comments)
                               .FirstOrDefault(x => x.Id == storyId);
        if (story == null)
        {
            throw new RestException("История не найдена", HttpStatusCode.NotFound);
        }

        var userId = User.GetUserId()!.Value;
        story.Comments.Add(new Comment
        {
            Text = request.Text,
            UserId = userId,
        });

        await dataContext.SaveChangesAsync(cancellationToken);
        return new CreateCommentResponse();
    }

    [HttpPost, Route("/{storyId:guid}/comment/{commentId:guid}/like")]
    public async Task LikeComment([FromRoute] Guid storyId,
                                  [FromRoute] Guid commentId,
                                  [FromServices] DataContext dataContext,
                                  CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!.Value;
        var comment = dataContext.Comments
                                 .Include(x => x.ReviewScores)
                                 .FirstOrDefault(x => x.Id == commentId)
                      ?? throw new RestException("Комментарий не найдена", HttpStatusCode.NotFound);

        var reviewScore = comment.ReviewScores.FirstOrDefault(x => x.UserId == userId);
        switch (reviewScore)
        {
            case null:
                comment.ReviewScores.Add(new ReviewScore
                {
                    ReviewId = commentId,
                    UserId = userId,
                    Positive = true,
                });
                break;
            case { Positive: false }:
                reviewScore.Positive = true;
                break;
            case { Positive: true }:
                comment.ReviewScores.Remove(reviewScore);
                break;
        }

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    [HttpPost, Route("/{storyId:guid}/comment/{commentId:guid}/dislike")]
    public async Task DislikeComment([FromRoute] Guid storyId,
                                     [FromRoute] Guid commentId,
                                     [FromServices] DataContext dataContext,
                                     CancellationToken cancellationToken)
    {
        var userId = User.GetUserId()!.Value;
        var comment = dataContext.Comments
                                 .Include(x => x.ReviewScores)
                                 .FirstOrDefault(x => x.Id == commentId)
                      ?? throw new RestException("Комментарий не найдена", HttpStatusCode.NotFound);

        var reviewScore = comment.ReviewScores.FirstOrDefault(x => x.UserId == userId);
        switch (reviewScore)
        {
            case null:
                comment.ReviewScores.Add(new ReviewScore
                {
                    ReviewId = commentId,
                    UserId = userId,
                    Positive = false,
                });
                break;
            case { Positive: true }:
                reviewScore.Positive = false;
                break;
            case { Positive: false }:
                comment.ReviewScores.Remove(reviewScore);
                break;
        }

        await dataContext.SaveChangesAsync(cancellationToken);
    }

    private TStory CreateStory<TStory>(StoryNewRequest request)
        where TStory : Story, new()
    {
        return new TStory
        {
            Title = request.Title,
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            Format = request.Format,
            StoryActivities = request.ActivityIds.Select(x => new StoryActivity { ActivityId = x }).ToArray(),
            PreviewId = request.PreviewId,
        };
    }

    private static IQueryable<Story> GetFilteredStoryItems(StoryItemsRequest request,
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

        if (request.Filters.ActivityIds.Any())
        {
            storyQuery = storyQuery.Where(x => x.StoryActivities.Any(y => request.Filters.ActivityIds.Contains(y.ActivityId)));
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