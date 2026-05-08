using Microsoft.Extensions.Logging;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Repositories;
using BiblioManager.Domain.Interfaces.Services;

namespace BiblioManager.Domain.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(IAuthorRepository authorRepository, ILogger<AuthorService> logger)
    {
        _authorRepository = authorRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Author>> GetAllAsync() 
    {
        _logger.LogInformation("Retrieving all Authors");
        return await _authorRepository.GetAllAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving Author with ID: {AuthorId}", id);
        var author = await _authorRepository.GetByIdAsync(id);

        if (author == null)
            _logger.LogWarning("Author with ID {AuthorId} not found", id);

        return author;
    }

    public async Task<Author> CreateAsync(Author author)
    {
        // Regla de negocio 
        // Buscamos si ya existe un autor con el mismo nombre Y apellido
        var existingAuthor = await _authorRepository.GetByNameAndLastNameAsync(author.FirstName, author.LastName);

        if (existingAuthor != null)
        {
            _logger.LogWarning("Attempted to create duplicate author: {FirstName} {LastName}", author.FirstName, author.LastName);
            throw new InvalidOperationException(
                $"Ya existe un autor registrado como '{author.FirstName} {author.LastName}'");
        }

        _logger.LogInformation("Creating author: {FirstName} {LastName}", author.FirstName, author.LastName);
        return await _authorRepository.CreateAsync(author);
    }

    public async Task UpdateAsync(int id, Author author)
    {
        var existingAuthor = await _authorRepository.GetByIdAsync(id);
        if (existingAuthor == null)
        {
            _logger.LogWarning("Author with ID {AuthorId} not found for update", id);
            throw new KeyNotFoundException(
                $"No se encontró el autor con ID {id}");
        }

        // Solo lanzamos error si el autor encontrado tiene un id diferente al que estamos editando
        var repeatedAuthor = await _authorRepository.GetByNameAndLastNameAsync(author.FirstName, author.LastName);

        if (repeatedAuthor != null && repeatedAuthor.Id != id)
        {
            _logger.LogWarning("Attempted to create duplicate author: {FirstName} {LastName}", author.FirstName, author.LastName);
            throw new InvalidOperationException(
                $"Ya existe un autor registrado como '{author.FirstName} {author.LastName}'");
        }

        existingAuthor.FirstName = author.FirstName;
        existingAuthor.LastName = author.LastName;
        existingAuthor.Nationality = author.Nationality;
        existingAuthor.BirthDate = author.BirthDate;

        _logger.LogInformation("Updating Author with ID: {AuthorId}", id);
        await _authorRepository.UpdateAsync(existingAuthor);
    }

    public async Task DeleteAsync(int id)
    {
        // Verificamos existencia del autor 
        var exists = await _authorRepository.ExistsAsync(id);
        if (!exists)
        {
            _logger.LogWarning("Author with ID {AuthorId} not found for deletion", id);
            throw new KeyNotFoundException(
                $"No se encontró el autor con ID {id}");
        }

        _logger.LogInformation("Deleting Author with ID: {AuthorId}", id);
        await _authorRepository.DeleteAsync(id);
    }
}