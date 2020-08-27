using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BlazorMovies.Client.Helpers;
using Tewr.Blazor.FileReader;
using BlazorMovies.Client.Repository;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorMovies.Client.Auth;

namespace BlazorMovies.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            ConfigureServices(builder.Services);
            
            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions(); // Authorization system
            services.AddTransient<IRepository, RepositoryInMemory>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IMoviesRepository, MoviesRepository>();
            services.AddFileReaderService(Options => Options.InitializeOnFirstCall = true);

            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, DummyAuthenitcationStateProvider>();
        }
    }
}
