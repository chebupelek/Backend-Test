using FluentResults;
using MCC.TestTask.App.Features.Comments;
using MCC.TestTask.App.Features.Communities;
using MCC.TestTask.App.Features.Communities.Dto;
using MCC.TestTask.App.Features.Posts;
using MCC.TestTask.App.Features.Posts.Dto;
using MCC.TestTask.App.Features.Tags;
using MCC.TestTask.App.Features.Users;
using MCC.TestTask.App.Features.Users.Dto;
using MCC.TestTask.Domain;
using MCC.TestTask.Persistance;
using Microsoft.EntityFrameworkCore;

namespace MCC.TestTask.App.Services.Seeding;

public class SeedingService(
    BlogDbContext dbContext,
    UserService userService,
    PostService postService,
    CommunityService communityService,
    CommentService commentService,
    TagService tagService)
{
    public async Task<Result> SeedDatabase()
    {
        if (dbContext.Users.Any())
             return Result.Ok();

        if (!dbContext.Users.Any(u => u.Email == "scalanis.farcan@mcc.blog.com"))
            await userService.RegisterUser(new UserRegisterModel()
                { Email = "scalanis.farcan@mcc.blog.com", Password = "123456", FullName = "Scalanis Farcan" });
        var sfUser = dbContext.Users.Single(u => u.Email == "scalanis.farcan@mcc.blog.com");

        if (!dbContext.Users.Any(u => u.Email == "ardreth.cairie@mcc.blog.com"))
            await userService.RegisterUser(new UserRegisterModel()
                { Email = "ardreth.cairie@mcc.blog.com", Password = "123456", FullName = "Ardreth Cairie" });
        var acUser = dbContext.Users.Single(u => u.Email == "ardreth.cairie@mcc.blog.com");

        if (!dbContext.Users.Any(u => u.Email == "tilverel.tragella@mcc.blog.com"))
            await userService.RegisterUser(new UserRegisterModel()
                { Email = "tilverel.tragella@mcc.blog.com", Password = "123456", FullName = "Filverel Tragella" });
        var ttUser = dbContext.Users.Single(u => u.Email == "tilverel.tragella@mcc.blog.com");

        if (!dbContext.Communities.Any(c => c.Name == "AC/DC"))
            await communityService.CreateCommunityAsync(acUser.Id,
                new CommunityCreateModel()
                    { Name = "AC/DC", CommunityType = CommunityType.Public, Description = "Ardreth Cairie fan club" });
        var acCommunity = dbContext.Communities.Include(c => c.Administrators).Include(c => c.Subscribers).Single(c => c.Name == "AC/DC");
        
        if(acCommunity.Administrators.All(s => s.Id != sfUser.Id))
            await communityService.UpdateCommunityAdminsList(acUser.Id, acCommunity.Id, [sfUser.Id]);
        if(acCommunity.Subscribers.All(s => s.Id != ttUser.Id))
            await communityService.SubscribeUserToCommunityAsync(acCommunity.Id, ttUser.Id);
        
        var tagIds = new List<Guid>();
        
        if(!dbContext.Tags.Any())
            for (var i = 0; i < 100; i++)
            {
                var tag = await tagService.CreateTag("Tag #" + i);
                tagIds.Add(tag.Value);
            }

        if (dbContext.Posts.Any()) 
            return Result.Ok();
        
        for (var i = 0; i < 2000; i++)
        {
            var post = await postService.CreatePostAsync(acUser.Id, acCommunity.Id, new CreatePostModel()
            {
                Title = "Post #" + i,
                Description = GetRandomText(),
                ReadingTime = i % 15,
                Tags = tagIds.Skip(i % 50).Take(Random.Shared.Next(1, 20)).ToList()
            });

            if (Random.Shared.NextDouble() < .3)
                await postService.LikePostAsync(post.Value, ttUser.Id);
        }
        

        return Result.Ok();
    }

    private const string _loremIpsum =
        "Nulla in pulvinar magna. Nulla ac erat lectus. Curabitur porttitor laoreet quam, et tempor felis malesuada sed. Nullam interdum orci nec erat mattis imperdiet. Donec maximus, tortor placerat tempor interdum, leo ex sagittis odio, non mollis enim justo sed nisl. Nam luctus lacus sit amet arcu varius sagittis. Proin sed tempus justo, quis porttitor dolor. Praesent vitae malesuada ligula. Vivamus sem quam, volutpat quis cursus sit amet, malesuada ac magna. Donec vel velit sit amet leo sodales fermentum. Curabitur ut pulvinar nisi.\n\nNam interdum ligula lectus. Nam fermentum eleifend vehicula. Quisque venenatis convallis nisi quis condimentum. Integer elementum egestas velit id posuere. Fusce ornare ex purus, condimentum pellentesque ligula faucibus in. Maecenas ut pretium nisl, sit amet iaculis justo. Donec sagittis venenatis lorem in convallis. Aliquam vel ex aliquam, tempus arcu id, sodales velit. Sed ullamcorper blandit vestibulum. Vivamus nec nulla in sapien imperdiet blandit. Cras fermentum varius ipsum, nec rhoncus tortor placerat in. Sed mattis condimentum magna, non tempus arcu iaculis id. Sed aliquam est a finibus ornare.\n\nNulla consectetur pulvinar tortor eget efficitur. Vivamus vel eros nisi. Interdum et malesuada fames ac ante ipsum primis in faucibus. Nunc eu augue lorem. Praesent dignissim porttitor dolor non fringilla. Suspendisse euismod sapien eu dui convallis, a rutrum nisl volutpat. Fusce viverra nulla et ex molestie, ut lacinia magna tincidunt. Duis vitae pharetra dui, in porta metus. Aliquam vitae magna vitae dui maximus interdum ut at nisl. Nulla ut vulputate ante. Maecenas vehicula nisi non eros tincidunt vulputate. Sed aliquet massa vel hendrerit cursus.\n\nProin sed eros id dolor cursus fermentum. Ut id tortor vestibulum, ullamcorper turpis nec, finibus tortor. Maecenas porta lacus et lorem posuere elementum. Quisque tristique pharetra facilisis. Nullam congue vitae odio pharetra faucibus. Maecenas tempor in ex lacinia condimentum. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras tristique ipsum turpis, at fermentum diam viverra ut. Sed sed purus in dui vestibulum dapibus at non metus. Sed egestas mauris ut enim iaculis viverra. Nunc elit metus, laoreet sed nisi et, accumsan varius est.\n\nCras id fringilla diam, in malesuada enim. Integer lorem enim, aliquam id ex eu, rutrum ullamcorper velit. Praesent pretium pharetra congue. Sed a massa auctor, sollicitudin urna eu, mattis lectus. Fusce tristique molestie interdum. Vivamus aliquam at eros vitae euismod. Curabitur in lacus tempor, euismod odio non, porta lacus. ";
    
    private static string GetRandomText()
    {
        var words = _loremIpsum.Split(' ');
        var len = new Random().Next(10, words.Length - 1);
        var maxSkip = words.Length - len;
        return string.Join(" ", words.Skip(Random.Shared.Next(0, maxSkip)).Take(len));
    }
}