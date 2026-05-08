using BiblioManager.DataAccess.Repositories;
using BiblioManager.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using BiblioManager.DataAccess.Context;
using BiblioManager.Domain.Interfaces.Services;
using BiblioManager.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Entity Framework Core ──
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();


// ── Services ──
builder.Services.AddScoped<IAuthorService, AuthorService>();

// ── AutoMapper ──
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// ── Controllers ──
builder.Services.AddControllers();

// ── Swagger ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Middleware Pipeline ──
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();