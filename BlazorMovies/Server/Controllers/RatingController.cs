using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public RatingController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Rate(MovieRating movieRating)
        {
            var user = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var userId = user.Id;

            var currentRating = await context.MovieRatings
                .FirstOrDefaultAsync(x => x.MovieId == movieRating.MovieId && x.UserId == userId);

            if(currentRating == null)
            {
                movieRating.UserId = userId;
                context.Add(movieRating);
            }
            else
            {
                currentRating.Rate = movieRating.Rate;
            }
            movieRating.RatingDate = DateTime.Today;
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
