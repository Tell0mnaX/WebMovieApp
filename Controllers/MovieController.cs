using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebMovieApplication.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly MovieContext _context;

    public MovieController(MovieContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovies() =>
        await _context.Movies.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        return movie == null ? NotFound() : Ok(movie);
    }

    [HttpGet("search/{keyword}")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByKeyword(string keyword)
    {
        var movies = await _context.Movies.Where(m => m.Title.ToLower().Contains(keyword.ToLower())).ToListAsync();
        return movies.Count == 0 ? NotFound() : Ok(movies);
    }

    [HttpGet("genre/{genre}")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByGenre(string genre)
    {
        var movies = await _context.Movies.Where(m => m.Genre.ToLower() == genre.ToLower()).ToListAsync();
        return movies.Count == 0 ? NotFound() : Ok(movies);
    }

    [HttpGet("sorted-by-year")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesSortedByYear()
    {
        var movies = await _context.Movies.OrderByDescending(m => m.ReleaseYear).ToListAsync();
        return movies.Count == 0 ? NotFound() : Ok(movies);
    }

    [HttpGet("latest")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetLastAddedMovies()
    {
        var movies = await _context.Movies.OrderByDescending(m => m.Id).Take(3).ToListAsync();
        return movies.Count == 0 ? NotFound() : Ok(movies);
    }

    [HttpGet("best-movies")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetBestMovies()
    {
        var movies = await _context.Movies.OrderByDescending(m => m.Rating).Take(5).ToListAsync();
        return movies.Count == 0 ? NotFound() : Ok(movies);
    }

    [HttpGet("Genre/count")]
    public async Task<ActionResult<IEnumerable<string>>> GroupMoviesByGenre()
    {
        var movies = await _context.Movies
        .GroupBy(groupm => groupm.Genre)
        .Where(groupm => groupm.Count()>1)
        .OrderByDescending(groupm => groupm.Count())
        .Select(groupm => $"{groupm.Key} : {groupm.Count()} film(s)")
        .ToListAsync();

        return movies.Count == 0 ? NotFound() : Ok(movies);
    }



    [HttpPut("update/{id}")]
    public async Task<ActionResult<Movie>> UpdateMovie(int id, Movie movie)
    {
        var mv = await _context.Movies.FindAsync(id);
        if(mv==null) return NotFound($"l'id {id} est introuvable");
        
        if(mv.Title!=null)mv.Title = movie.Title;
        if(mv.Director!=null)mv.Director = movie.Director;
        if(mv.ReleaseYear!=null)mv.ReleaseYear = movie.ReleaseYear;
        if(mv.Genre!=null)mv.Genre = movie.Genre;
        if(mv.Rating!=null)mv.Rating = movie.Rating;

        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMovie), new {id = movie.Id}, movie);
    }

    [HttpPost]
    public async Task<ActionResult<Movie>> AddMovie(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMovie), new {id = movie.Id}, movie);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> RemoveMovie(int id)
    {
        var movieToDelete = await _context.Movies.FindAsync(id);
        if(movieToDelete==null) return NotFound($"Le film avec l'id {id} que vous cherchez n'existe pas...");
        _context.Movies.Remove(movieToDelete);
        await _context.SaveChangesAsync();
        return Ok($"Le film {movieToDelete.Title} a été supprimé");
    }

    [HttpDelete("by-title/{title}")]
    public async Task<ActionResult<string>> RemoveMovieByTitle(string title)
    {
        var movie = await _context.Movies
        .FirstOrDefaultAsync(m => m.Title.ToLower() == title.ToLower());


        if(movie==null){return NotFound($"Le film {title} que vous cherchez n'existe pas...");}
        
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return Ok($"Le film \"{movie.Title}\" a été supprimé.");
    }
}