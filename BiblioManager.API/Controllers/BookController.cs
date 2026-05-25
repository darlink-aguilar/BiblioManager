using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BiblioManager.API.DTOs.Request;
using BiblioManager.API.DTOs.Response;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Services;

namespace BiblioManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;

    public BookController(
        IBookService bookService,
        IMapper mapper)
    {
        _bookService = bookService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<BookResponseDTO>>(books));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponseDTO>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book == null)
            return NotFound(new { message = $"Libro con ID {id} no encontrado" });

        return Ok(_mapper.Map<BookResponseDTO>(book));
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDTO>> Create(BookRequestDTO dto)
    {
        try
        {
            var book = _mapper.Map<Book>(dto);
            var created = await _bookService.CreateAsync(book);
            var responseDto = _mapper.Map<BookResponseDTO>(created);

            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, BookRequestDTO dto)
    {
        try
        {
            var book = _mapper.Map<Book>(dto);
            await _bookService.UpdateAsync(id, book);
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
            await _bookService.DeleteAsync(id);
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


    [HttpPost("{id}/authors")]
    public async Task<ActionResult> AddAuthor(int id, RegisterAuthorDTO dto)
    {
        try
        {
            await _bookService.AddAuthorToBookAsync(id, dto.AuthorId);
            return Ok(new { message = "Autor asociado al libro exitosamente" });
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


    [HttpDelete("{id}/authors/{authorId}")]
    public async Task<ActionResult> RemoveAuthor(int id, int authorId)
    {
        try
        {
            await _bookService.RemoveAuthorFromBookAsync(id, authorId);
            return NoContent();
        }
        catch (KeyNotFoundException ex) 
        { 
            return NotFound(new { message = ex.Message }); 
        }
    }


    [HttpGet("{id}/authors")]
    public async Task<ActionResult<IEnumerable<AuthorResponseDTO>>> GetAuthors(int id)
    {
        try
        {
            var authors = await _bookService.GetAuthorsByBookAsync(id);
            return Ok(_mapper.Map<IEnumerable<AuthorResponseDTO>>(authors));
        }
        catch (KeyNotFoundException ex) 
        { 
            return NotFound(new { message = ex.Message }); 
        }
    }
}