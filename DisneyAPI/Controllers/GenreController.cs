using Microsoft.AspNetCore.Mvc;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using DisneyAPI.ViewModels.GenreViewModel;
using AutoMapper;

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
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genderRepository, IMovieOrSerieRepository movieOrSerieRepository, IMapper mapper)
        {
            _genderRepository = genderRepository;
            _movieOrSerieRepository = movieOrSerieRepository;
            _mapper = mapper;
        }

        // GET: api/<GenderController>
        [HttpGet]
        [Route("genres")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                var genres = _genderRepository.GetAllGenders();
                if (genres == null) return NoContent();
                var genresVM = new List<GenreGetResponseViewModel>();
                genresVM = _mapper.Map<List<Genre>, List<GenreGetResponseViewModel>>(genres);

                return Ok(genresVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
        }

        // POST api/<GenderController>
        [HttpPost]
        public IActionResult Post(GenrePostRequestViewModel genreVM)
        {
            try
            {
                var genre = _mapper.Map<GenrePostRequestViewModel, Genre>(genreVM); 
                if (genreVM.MovieOrSeriesId.Any())
                {
                    var movieOrSerieList = _movieOrSerieRepository.GetAllMoviesOrSeries();
                    genre.MovieOrSeries = movieOrSerieList.Where(x => x.Id == genreVM.MovieOrSeriesId.FirstOrDefault(y => y == x.Id)).ToList();
                }
                return Ok(_genderRepository.Add(genre));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // PUT api/<GenderController>/5
        [HttpPut]
        public IActionResult Put( GenrePutRequestViewModel genreVM)
        {
            var originalGenre = _genderRepository.GetGender(genreVM.Id);
            if (originalGenre == null) return BadRequest("La pelicula o serie no existe");
            try
            {
                _mapper.Map<GenrePutRequestViewModel, Genre>(genreVM, originalGenre);

                if (genreVM.MovieOrSeriesId.Any())
                {
                    var movieOrSerieList = _movieOrSerieRepository.GetAllMoviesOrSeries();
                    originalGenre.MovieOrSeries = movieOrSerieList.Where(x => x.Id == genreVM.MovieOrSeriesId.FirstOrDefault(y => y == x.Id)).ToList();
                }
                return Ok(_genderRepository.Update(originalGenre));
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
