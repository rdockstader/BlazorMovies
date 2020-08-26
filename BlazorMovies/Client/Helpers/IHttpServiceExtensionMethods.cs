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
    }
}
