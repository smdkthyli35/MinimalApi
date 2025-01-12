using Microsoft.EntityFrameworkCore;
using MinimalApi.Contexts;
using MinimalApi.Models;
using MinimalApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("BookDb");
});

builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

app.MapPost("books", async (Book book, IBookService bookService, CancellationToken cancellationToken) =>
{
    bool result = await bookService.CreateAsync(book, cancellationToken);
    if (!result)
        return Results.BadRequest("Something went wrong!");

    return Results.Ok(result);
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
