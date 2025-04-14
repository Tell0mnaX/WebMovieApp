using WebMovieApplication.Models;

public static class DataSeeder
{
    public static void SeedDatabase(MovieContext context)
    {
        if (context.Movies.Any()) return; // Ne pas dupliquer les données

        var movies = new List<Movie>
        {
            new Movie { Title = "Inception", Director = "Christopher Nolan", ReleaseYear = 2010, Genre = "Science Fiction", Rating = 9.5 },
            new Movie { Title = "The Dark Knight", Director = "Christopher Nolan", ReleaseYear = 2008, Genre = "Action", Rating = 9.5 },
            new Movie { Title = "Parasite", Director = "Bong Joon-ho", ReleaseYear = 2019, Genre = "Thriller", Rating = 9.7 },
            new Movie { Title = "Interstellar", Director = "Christopher Nolan", ReleaseYear = 2014, Genre = "Science Fiction", Rating = 9.2 },
            new Movie { Title = "La La Land", Director = "Damien Chazelle", ReleaseYear = 2016, Genre = "Romance" , Rating = 8.7},
            new Movie { Title = "Joker", Director = "Todd Phillips", ReleaseYear = 2019, Genre = "Drama", Rating = 9.1 },
            new Movie { Title = "Dune", Director = "Denis Villeneuve", ReleaseYear = 2021, Genre = "Science Fiction", Rating = 8.6 },
            new Movie { Title = "Whiplash", Director = "Damien Chazelle", ReleaseYear = 2014, Genre = "Drama" , Rating = 7.2},
            new Movie { Title = "The Godfather", Director = "Francis Ford Coppola", ReleaseYear = 1972, Genre = "Crime" , Rating = 9.0},
            new Movie { Title = "Titanic", Director = "James Cameron", ReleaseYear = 1997, Genre = "Romance", Rating = 7.8 }
        };

        context.Movies.AddRange(movies);
        context.SaveChanges();
    }
}
