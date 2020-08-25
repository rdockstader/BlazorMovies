using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorMovies.Shared.Entities
{
    public class MoviesActors
    {
        public int PersonId { get; set; }
        public int MovieId { get; set; }
        public Person Person;
        public Movie Movie;
        public string Character { get; set; }
        public int Order { get; set; }
    }
}
