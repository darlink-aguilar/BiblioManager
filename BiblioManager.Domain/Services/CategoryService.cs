using Microsoft.Extensions.Logging;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Repositories;
using BiblioManager.Domain.Interfaces.Services;

namespace BiblioManager.Domain.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all Categories");
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving Category with ID: {CategoryId}", id);
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            _logger.LogWarning("Category with ID {CategoryId} not found", id);

        return category;
    }

    public async Task<Category> CreateAsync(Category category)
    {
        // Regla de negocio 
        // Buscamos si ya existe una categoría con el mismo nombre
        var existingCategory = await _categoryRepository.GetByNameAsync(category.Name);
        if (existingCategory != null)
        {
            _logger.LogWarning("Attempted to create duplicate Category: {Name}", category.Name);
            throw new InvalidOperationException(
                $"Ya existe una categoría registrada como '{category.Name}'");
        }

        _logger.LogInformation("Creating Category: {Name}", category.Name);
        return await _categoryRepository.CreateAsync(category);
    }

    public async Task UpdateAsync(int id, Category category)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
        {
            _logger.LogWarning("Category with ID {CategoryId} not found for update", id);
            throw new KeyNotFoundException(
                $"No se encontró la categoría con ID {id}");
        }

        // Solo lanzamos error si la categoría encontrada tiene un id diferente al que estamos editando
        var repeatedCategory = await _categoryRepository.GetByNameAsync(category.Name);
        if (repeatedCategory != null && repeatedCategory.Id != id)
        {
            _logger.LogWarning("Attempted to create duplicate Category: {Name}", category.Name);
            throw new InvalidOperationException(
                $"Ya existe una categoría registrada como '{category.Name}'");
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;

        _logger.LogInformation("Updating Category with ID: {CategoryId}", id);
        await _categoryRepository.UpdateAsync(existingCategory);
    }

    public async Task DeleteAsync(int id)
    {
        // Verificamos existencia
        var exists = await _categoryRepository.ExistsAsync(id);
        if (!exists)
        {
            _logger.LogWarning("Category with ID {CategoryId} not found for deletion", id);
            throw new KeyNotFoundException($"No se encontró la categoría con ID {id}");
        }

        // VALIDACION: No borrar si tiene libros
        //var hasBooks = await _categoryRepository.HasBooksAsync(id);
        //if (hasBooks)
        //{
        //    _logger.LogWarning("Attempted to delete Category {CategoryId} that has books associated", id);
        //    throw new InvalidOperationException("No se puede eliminar la categoría porque tiene libros asociados.");
        //}
       
        _logger.LogInformation("Deleting Category with ID: {CategoryId}", id);
        await _categoryRepository.DeleteAsync(id);
    }
}