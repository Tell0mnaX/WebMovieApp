namespace WebMovieApplication.Models
{
    public class Movie
    {
        public int Id {set; get;}

        public required string Title {set; get;}

        public required string Director {set; get;}

        public int ReleaseYear {set; get;}

        public required string Genre {set; get;}

        public double Rating { get; set; }
    }
    
}