using Microsoft.AspNetCore.Mvc;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DisneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Route("movies")]
        public IActionResult Get()
        {
            var genders = _genderRepository.GetAllGenders();
            if (genders == null) return BadRequest("No se ha agregado contenido");
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
                Console.WriteLine(ex.ToString());
                return BadRequest($"Error: {ex.Message}");

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
                Console.WriteLine(ex.ToString());
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // DELETE api/<GenderController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var originalGender = _genderRepository.GetGender(id);
            if (originalGender == null) return BadRequest("El genero no existe");
            try
            {
                _genderRepository.Delete(originalGender.Id);
                return Ok(" Eliminado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
