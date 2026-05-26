using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BiblioManager.API.DTOs.Request;
using BiblioManager.API.DTOs.Response;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Services;

namespace BiblioManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(
        ICategoryService categoryService,
        IMapper mapper,
        ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        var categoriesDto = _mapper.Map<IEnumerable<CategoryResponseDTO>>(categories);
        return Ok(categoriesDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponseDTO>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound(new { message = $"Categoría con ID {id} no encontrada" });
        }

        var categoryDto = _mapper.Map<CategoryResponseDTO>(category);
        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDTO>> Create(CategoryRequestDTO dto)
    {
        try
        {
            var category = _mapper.Map<Category>(dto);
            var createdCategory = await _categoryService.CreateAsync(category);
            var responseDto = _mapper.Map<CategoryResponseDTO>(createdCategory);

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
    public async Task<ActionResult> Update(int id, CategoryRequestDTO dto)
    {
        try
        {
            var category = _mapper.Map<Category>(dto);
            await _categoryService.UpdateAsync(id, category);
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
            _logger.LogInformation("HTTP DELETE: Attempting to delete category with ID {Id}", id);
            await _categoryService.DeleteAsync(id);
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
}