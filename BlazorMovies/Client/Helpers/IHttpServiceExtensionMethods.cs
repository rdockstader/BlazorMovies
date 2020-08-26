using BlazorMovies.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Client.Helpers
{
    public static class IHttpServiceExtensionMethods
    {
        public static async Task<T> GetHelper<T>(this IHttpService httpService, string url)
        {
            var response = await httpService.Get<T>(url);

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            Console.WriteLine("Request was successful.");
            return response.Response;
        }

        public static async Task<PaginatedResponse<T>> GetHelper<T>(this IHttpService httpService, string url, PaginationDTO paginationDTO)
        {
            string newURL = "";
            if(url.Contains("?"))
            {
                newURL = $"{url}&page={paginationDTO.Page}&recordsPerPage={paginationDTO.RecordsPerPage}";
            }
            else
            {
                newURL = $"{url}?page={paginationDTO.Page}&recordsPerPage={paginationDTO.RecordsPerPage}";
            }
            var httpResponse = await httpService.Get<T>(newURL);
            var totalAmoutPages = int.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());
            var paginatedresponse = new PaginatedResponse<T>()
            {
                Response = httpResponse.Response,
                TotalAmountPages = totalAmoutPages
            };

            return paginatedresponse;
        }
    }
}
