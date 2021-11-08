using Microsoft.AspNetCore.Mvc;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DisneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genderRepository;
        private readonly IMovieOrSerieRepository _movieOrSerieRepository;

        public GenreController(IGenreRepository genderRepository, IMovieOrSerieRepository movieOrSerieRepository)
        {
            _genderRepository = genderRepository;
            _movieOrSerieRepository = movieOrSerieRepository;
        }

        // GET: api/<GenderController>
        [HttpGet]
        [Route("genres")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                var genders = _genderRepository.GetAllGenders();
                if (genders == null) return NoContent();
                var gendersVM = new List<GenreGetResponseViewModel>();
                foreach (var item in genders)
                {
                    gendersVM.Add(new GenreGetResponseViewModel
                    {
                        Id = item.Id,
                        Image = item.Image,
                        Name = item.Name
                    });
                }
                return Ok(gendersVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
        }

        // POST api/<GenderController>
        [HttpPost]
        public IActionResult Post(GenrePostRequestViewModel genderVM)
        {
            try
            {
                Genre gender = new Genre
                {
                    Id = genderVM.Id,
                    Image = genderVM.Image,
                    Name = genderVM.Name
                 
                };
                if (genderVM.MovieOrSeriesId.Any())
                {
                    var movieOrSeriesList = _movieOrSerieRepository.GetAllMoviesOrSeries();
                    foreach (var item in genderVM.MovieOrSeriesId)
                    {
                        var element = movieOrSeriesList.FirstOrDefault(x => x.Id == item);
                        if (element != null)
                        {
                            gender.MovieOrSeries.Add(element);

                        }
                    }
                }
                return Ok(_genderRepository.Add(gender));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // PUT api/<GenderController>/5
        [HttpPut]
        public IActionResult Put( GenrePutRequestViewModel genderVM)
        {
            var originalGender = _genderRepository.GetGender(genderVM.Id);
            if (originalGender == null) return BadRequest("La pelicula o serie no existe");
            try
            {
                originalGender.Image = genderVM.Image;
                originalGender.Name = genderVM.Name;

                if (genderVM.MovieOrSeriesId.Any())
                {
                    var movieOrSerieList = _movieOrSerieRepository.GetAllMoviesOrSeries();
                    foreach (var item in genderVM.MovieOrSeriesId)
                    {
                        var element = movieOrSerieList.FirstOrDefault(x => x.Id == item);
                        if (element != null)
                        {
                            originalGender.MovieOrSeries.Add(element);

                        }
                    }
                }
                return Ok(_genderRepository.Update(originalGender));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // DELETE api/<GenderController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult Delete(int id)
        {
            
            try
            {
                var originalGender = _genderRepository.GetGender(id);
                if (originalGender == null) return BadRequest("El genero no existe");

                _genderRepository.Delete(originalGender.Id);
                return Ok(" Eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
