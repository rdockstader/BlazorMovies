using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Pages
{
    public partial class Counter
    {
        [Inject] SingletonService singleton { get; set; }
        [Inject] TransientService transient { get; set; }
        private int currentCount = 0;
        private List<Movie> movies;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            movies = new List<Movie>()
        {
            new Movie() { Title = "Inception", ReleaseDate = new DateTime(2019, 7, 2)}
        };
        }

        private void IncrementCount()
        {
            currentCount++;
            transient.Value = currentCount;
            singleton.Value = currentCount;
        }
    }
}
