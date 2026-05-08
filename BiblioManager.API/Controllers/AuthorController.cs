using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BiblioManager.API.DTOs.Request;
using BiblioManager.API.DTOs.Response;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Services;

namespace BiblioManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorController> _logger;

    public AuthorController(
        IAuthorService authorService,
        IMapper mapper,
        ILogger<AuthorController> logger)
    {
        _authorService = authorService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorResponseDTO>>> GetAll()
    {
        var authors = await _authorService.GetAllAsync();
        var authorsDto = _mapper.Map<IEnumerable<AuthorResponseDTO>>(authors);
        return Ok(authorsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorResponseDTO>> GetById(int id)
    {
        var author = await _authorService.GetByIdAsync(id);

        if (author == null)
            return NotFound(new { message = $"Autor con ID {id} no encontrado" });

        var authorDto = _mapper.Map<AuthorResponseDTO>(author);
        return Ok(authorDto);
    }

    [HttpPost] 
    public async Task<ActionResult<AuthorResponseDTO>> Create(AuthorRequestDTO dto)
    {
        try
        {
            var author = _mapper.Map<Author>(dto);
            var createdAuthor = await _authorService.CreateAsync(author);
            var responseDto = _mapper.Map<AuthorResponseDTO>(createdAuthor);

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.Id },
                responseDto);
        }
        catch (InvalidOperationException ex) 
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, AuthorRequestDTO dto)
    {
        try
        {
            var author = _mapper.Map<Author>(dto);
            await _authorService.UpdateAsync(id, author);
            return NoContent();
        }
        catch (KeyNotFoundException ex) 
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) 
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _authorService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) 
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
