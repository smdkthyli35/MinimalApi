using Microsoft.EntityFrameworkCore;
using MinimalApi.Contexts;
using MinimalApi.Models;

namespace MinimalApi.Services;

public sealed class BookService(ApplicationDbContext context) : IBookService
{
    public async Task<bool> CreateAsync(Book book, CancellationToken cancellationToken = default)
    {
        await context.Books.AddAsync(book, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(string isbn, CancellationToken cancellationToken = default)
    {
        Book? book = await context.Books.FindAsync(isbn, cancellationToken);
        if (book is null) return false;

        context.Books.Remove(book);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Books.ToListAsync(cancellationToken);
    }

    public async Task<Book> GetByIsbnAsync(string isbn, CancellationToken cancellationToken = default)
    {
        Book? book = await context.Books.FindAsync(isbn, cancellationToken);
        if (book is null) throw new KeyNotFoundException();
        return book;
    }

    public async Task<IEnumerable<Book>> SearchByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await context.Books.Where(b => b.Title.Contains(title)).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        context.Update(book);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
