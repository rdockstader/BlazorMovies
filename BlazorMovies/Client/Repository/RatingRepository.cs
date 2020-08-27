using BlazorMovies.Client.Helpers;
using BlazorMovies.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Repository
{
    public class RatingRepository: IRatingRepository
    {
        private readonly IHttpService httpService;
        private readonly string baseURL = "api/rating";

        public RatingRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }
        public async Task Vote(MovieRating rating)
        {
            var response = await httpService.Post($"{baseURL}/", rating);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
