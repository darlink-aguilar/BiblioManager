using Microsoft.Extensions.Logging;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Repositories;
using BiblioManager.Domain.Interfaces.Services;

namespace BiblioManager.Domain.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookAuthorRepository _bookAuthorRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ILogger<BookService> _logger;

    public BookService(
        IBookRepository bookRepository,
        IBookAuthorRepository bookAuthorRepository,
        IAuthorRepository authorRepository,
        ILogger<BookService> logger)
    {
        _bookRepository = bookRepository;
        _bookAuthorRepository = bookAuthorRepository;
        _authorRepository = authorRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all books");
        return await _bookRepository.GetAllAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving book with ID: {BookId}", id);
        return await _bookRepository.GetByIdAsync(id);

        // PROBAR 
    }

    public async Task<Book> CreateAsync(Book book)
    {
        // validaciones
        // 1. Validar ISBN único
        var existingBook = await _bookRepository.GetByIsbnAsync(book.Isbn);
        if (existingBook != null)
        {
            throw new InvalidOperationException($"Ya existe un libro registrado con el ISBN '{book.Isbn}'");
        }

        _logger.LogInformation("Creating book: {BookTitle}", book.Title);
        return await _bookRepository.CreateAsync(book);
    }

    public async Task UpdateAsync(int id, Book book)
    {
        var existing = await _bookRepository.GetByIdAsync(id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"No se encontró el libro con ID {id}");
        }

        // Validar que si cambia el ISBN
        var repeatedIsbn = await _bookRepository.GetByIsbnAsync(book.Isbn);
        if (repeatedIsbn != null && repeatedIsbn.Id != id)
        {
            throw new InvalidOperationException($"El ISBN '{book.Isbn}' ya pertenece a otro libro.");
        }

        existing.Isbn = book.Isbn;
        existing.Title = book.Title;
        existing.Synopsis = book.Synopsis;
        existing.PublicationDate = book.PublicationDate;

        _logger.LogInformation("Updating book with ID: {BookId}", id);
        await _bookRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _bookRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el libro con ID {id}");

        _logger.LogInformation("Deleting book with ID: {BookId}", id);
        await _bookRepository.DeleteAsync(id);
    }

    public async Task AddAuthorToBookAsync(int bookId, int authorId)
    {
        // VALIDACIONES
        // 1. Validar que el libro existe
        var bookExists = await _bookRepository.ExistsAsync(bookId);
        if (!bookExists)
            throw new KeyNotFoundException($"No se encontró el libro con ID {bookId}");

        // 2. Validar que el autor existe
        var authorExists = await _authorRepository.ExistsAsync(authorId);
        if (!authorExists)
            throw new KeyNotFoundException($"No se encontró el autor con ID {authorId}");

        // 3. Validar que no esté ya asociado
        var existingAssociation = await _bookAuthorRepository.GetByBookAndAuthorAsync(bookId, authorId);
        if (existingAssociation != null)
        {
            throw new InvalidOperationException("Este autor ya está asociado a este libro");
        }

        var bookAuthor = new BookAuthor
        {
            BookId = bookId,
            AuthorId = authorId,
            CreatedAt = DateTime.UtcNow
        };

        _logger.LogInformation("Associating author {AuthorId} to book {BookId}", authorId, bookId);
        await _bookAuthorRepository.CreateAsync(bookAuthor);
    }

    public async Task RemoveAuthorFromBookAsync(int bookId, int authorId)
    {
        // Validar que exista este registro
        var association = await _bookAuthorRepository.GetByBookAndAuthorAsync(bookId, authorId);
        if (association == null)
        {
            throw new KeyNotFoundException("No existe ninguna relación entre el autor y el libro especificados");
        }

        _logger.LogInformation("Removing association between author {AuthorId} and book {BookId}", authorId, bookId);
        await _bookAuthorRepository.DeleteAsync(association.Id);
    }

    public async Task<IEnumerable<Author>> GetAuthorsByBookAsync(int bookId)
    {
        var bookExists = await _bookRepository.ExistsAsync(bookId);
        if (!bookExists)
            throw new KeyNotFoundException($"No se encontró el libro con ID {bookId}");

        var bookAuthors = await _bookAuthorRepository.GetByBookAsync(bookId);

        return bookAuthors.Select(ba => ba.Author);
    }
}