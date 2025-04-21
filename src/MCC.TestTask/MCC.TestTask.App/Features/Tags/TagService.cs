using MCC.TestTask.App.Features.Tags.Dto;
using MCC.TestTask.Domain;
using MCC.TestTask.Persistance;
using MCC.TestTask.Infrastructure;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace MCC.TestTask.App.Features.Tags;

public class TagService
{
    private readonly BlogDbContext _blogDbContext;

    public TagService(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task<Result<List<TagDto>>> GetAllTagsAsync()
    {
        return await _blogDbContext.Tags
            .Select(t => t.ToDto())
            .ToListAsync();
    }

    public async Task<Result<Guid>> CreateTag(string name)
    {
        if (await _blogDbContext.Tags.AnyAsync(t => t.Name == name))
            return CustomErrors.ValidationError("Tag already exists");

        var tag = new Tag
        {
            CreateTime = DateTime.UtcNow,
            Name = name
        };

        _blogDbContext.Tags.Add(tag);
        await _blogDbContext.SaveChangesAsync();
        return tag.Id;
    }
}