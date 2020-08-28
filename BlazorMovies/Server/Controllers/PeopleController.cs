using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using BlazorMovies.Server.Helpers;
using BlazorMovies.Shared.DTOs;
using BlazorMovies.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        private readonly IMapper mapper;

        public PeopleController(ApplicationDbContext context, IFileStorageService fileStorageService, IMapper mapper)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.People.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDTO.RecordsPerPage);

            return await queryable.Paginate(paginationDTO).ToListAsync();
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);

            if(person == null) { return NotFound(); }

            return person;
        }

        [HttpGet("details/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DetailsPersonDTO>> GetPersonDetails(int id)
        {
            var person = await context.People.Where(x => x.Id == id)
                .Include(pers => pers.MoviesActors).ThenInclude(ma => ma.Movie)
                .FirstOrDefaultAsync();

            if (person == null) { return NotFound(); }

            var model = new DetailsPersonDTO();
            model.Person = person;
            model.Appearances = person.MoviesActors.Select(x => x.Movie).OrderByDescending(x => x.ReleaseDate).ToList();

            return model;

        }
        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            if(string.IsNullOrWhiteSpace(searchText)) { return new List<Person>(); }
            return await context.People.Where(x => x.Name.Contains(searchText))
                .Take(5)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            if(!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                person.Picture = await fileStorageService.SaveFile(personPicture, "jpg", "people");
            }
            context.Add(person);
            await context.SaveChangesAsync();
            return person.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            var personDB = await context.People.FirstOrDefaultAsync(pers => pers.Id == person.Id);

            if (personDB == null) { return NotFound(); }

            personDB = mapper.Map(person, personDB);

            if(!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                personDB.Picture = await fileStorageService.EditFile(personPicture, "jpg", "people", personDB.Picture);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            context.Remove(person);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
