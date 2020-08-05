using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BlazorMovies.Client.Shared.MainLayout;

namespace BlazorMovies.Client.Pages
{
    public partial class Counter
    {
        [Inject] SingletonService singleton { get; set; }
        [Inject] TransientService transient { get; set; }
        [Inject] IJSRuntime js { get; set; }

        [CascadingParameter] public AppState appState { get; set; }

        private int currentCount = 0;
        private static int currentStaticCount = 0;
        private List<Movie> movies;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            movies = new List<Movie>()
        {
            new Movie() { Title = "Inception", ReleaseDate = new DateTime(2019, 7, 2)}
        };
        }
        [JSInvokable]
        public async Task IncrementCount()
        {
            currentCount++;
            transient.Value = currentCount;
            singleton.Value = currentCount;
            currentStaticCount++;
            await js.InvokeVoidAsync("dotnetStaticInvocation");
        }

        private async Task IncrementCountJavaScript()
        {
            await js.InvokeVoidAsync("dotnetInstanceInvocation", DotNetObjectReference.Create(this));
        }
        [JSInvokable]
        public static Task<int> GetCurrentCount()
        {
            return Task.FromResult(currentStaticCount);
        }
    }
}
