﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Helpers
{
    public class InAppStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public InAppStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
        }
        public Task DeleteFile(string fileRoute, string containerName)
        {
            if(fileRoute != null)
            {
                var fileName = Path.GetFileName(fileRoute);
                if(fileName != null)
                {
                    string fileDirectory = Path.Combine(env.WebRootPath, containerName, fileName);
                    if (File.Exists(fileDirectory))
                    {
                        File.Delete(fileDirectory);
                    }
                }
                
            }
            

            return Task.FromResult(0);
        }

        public async Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute)
        {
            if (!string.IsNullOrEmpty(fileRoute))
            {
                await DeleteFile(fileRoute, containerName);
            }

            return await SaveFile(content, extension, containerName);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string containerName)
        {
            var fileName = $"{Guid.NewGuid()}.{extension}";
            string folder = Path.Combine(env.WebRootPath, containerName);

            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string savingPath = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(savingPath, content);

            var request = httpContextAccessor.HttpContext.Request;

            var currentUrl = $"{request.Scheme}://{request.Host}";
            var pathForDatabase = Path.Combine(currentUrl, containerName, fileName);

            return pathForDatabase;
        }
    }
}
