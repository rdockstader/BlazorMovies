using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class MoviesRepository: IMoviesRepository
    {
        private readonly IHttpService httpService;
        private string url = "api/movies";

        public MoviesRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }
        
        public async Task<IndexPageDTO> GetIndexPageDTO()
        {
            return await httpService.GetHelper<IndexPageDTO>(url);
        }

        public async Task<MovieUpdateDTO> GetMovieForUpdate(int id)
        {
            var requestURL = $"{url}/update/{id}";
            return await httpService.GetHelper<MovieUpdateDTO>(requestURL);
        }
        public async Task<DetailsMovieDTO> GetDetailsMovieDTO(int id)
        {
            var requestURL = $"{url}/{id}";
            return await httpService.GetHelper<DetailsMovieDTO>(requestURL);
        }

        public async Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDTO)
        {
            var responseHTTP = await httpService.Post<FilterMoviesDTO, List<Movie>>($"{url}/filter", filterMoviesDTO);
            var totalAmoutPages = int.Parse(responseHTTP.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<List<Movie>>()
            {
                Response = responseHTTP.Response,
                TotalAmountPages = totalAmoutPages
            };

            return paginatedResponse;
        }

        public async Task<int> CreateMovie(Movie movie)
        {
            var response = await httpService.Post<Movie, int>(url, movie);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

            return response.Response;
        }
        public async Task UpdateMovie(Movie movie)
        {
            var response = await httpService.Put(url, movie);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task DeleteMovie(int Id)
        {
            var response = await httpService.Delete($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
