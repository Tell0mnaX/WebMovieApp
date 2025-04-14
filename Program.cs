using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MovieContext>(option => 
        option.UseSqlite("Data source =movie.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
    DataSeeder.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async (MovieContext context) =>
{
    var movies = await context.Movies.ToListAsync();
    return Results.Ok(movies);
});


app.MapControllers();

app.Run();
