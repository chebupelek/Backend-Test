using MCC.TestTask.App.Features.Authors.Dto;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace MCC.TestTask.App.Features.Authors;

[Route("api/author")]
public class AuthorController : ControllerBase
{
    private readonly AuthorService _authorService;

    public AuthorController(AuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<AuthorDto>>> GetAuthors()
    {
        return await _authorService.GetAuthorsAsync().ToActionResult();
    }
}