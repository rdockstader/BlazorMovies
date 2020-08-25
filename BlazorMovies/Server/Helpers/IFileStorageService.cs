using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Helpers
{
    public interface IFileStorageService
    {
        public Task DeleteFile(string fileRoute, string containerName);
        public Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute);
        public Task<string> SaveFile(byte[] content, string extension, string containerName);
    }
}
