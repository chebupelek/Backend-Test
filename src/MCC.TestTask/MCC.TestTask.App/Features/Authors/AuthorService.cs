using MCC.TestTask.App.Features.Authors.Dto;
using MCC.TestTask.Persistance;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace MCC.TestTask.App.Features.Authors;

public class AuthorService
{
    private readonly BlogDbContext _blogDbContext;

    public AuthorService(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task<Result<List<AuthorDto>>> GetAuthorsAsync()
    {
        return await _blogDbContext.Users.Select(u => new AuthorDto
            {
                FullName = u.FullName,
                Birthdate = u.BirthDate,
                Gender = u.Gender,
                Created = u.CreateTime,
                Likes = _blogDbContext.Posts.Count(p => p.LikedBy.Any(lb => lb.Id == u.Id)),
                Posts = _blogDbContext.Posts.Count(p => p.AuthorId == u.Id)
            })
            .Where(a => a.Posts > 0)
            .OrderBy(a => a.FullName)
            .ToListAsync();
    }
}