using Microsoft.AspNetCore.Mvc;
using DisneyAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using DisneyAPI.ViewModels.CharacterViewModel;
using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using DisneyAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route(template:"api/[controller]")]
    [Authorize]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IMovieOrSerieRepository _movieOrSerieRepository;
        private readonly IMapper _mapper;
        public CharacterController(ICharacterRepository characterRepository, IMovieOrSerieRepository movieOrSerieRepository, IMapper mapper)
        {
            _characterRepository = characterRepository;
            _movieOrSerieRepository = movieOrSerieRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("characters")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                var characters = _characterRepository.GetAllCharacters();
                if (characters == null) return NoContent();
                var charactersVM = new List<CharacterListGetViewModel>();
                charactersVM = _mapper.Map<List<Character>, List<CharacterListGetViewModel>>(characters);

                return Ok(charactersVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Error: {ex.Message}");
            }
            
            
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {            
            try
            {
                var character = _characterRepository.GetCharacter(id);
                if (character == null) return BadRequest("El personaje no existe");

                var characterVM = _mapper.Map<Character, CharacterGetResponseViewModel>(character);
              
                return Ok(characterVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
            
        }
        [HttpGet]
        [Route("obtenerPersonajes")]
        public IActionResult Get([FromQuery] CharacterGetRequestViewModel viewModel)
        {
            try
            {
                var characters = _characterRepository.GetAllCharacters();
                if (!characters.Any()) return NoContent();

                if (viewModel.IdMovie != 0)
                {
                    characters = characters.Where(x => x.MovieOrSeries.FirstOrDefault(x => x.Id == viewModel.IdMovie) != null).ToList();

                }
                if (!string.IsNullOrEmpty(viewModel.Name))
                {
                    characters = characters.Where(x => x.Name == viewModel.Name).ToList();
                }
                if (viewModel.Age > 0)
                {
                    characters = characters.Where(x => x.Age == viewModel.Age).ToList();
                }

                var charactersVM = new List<CharacterGetResponseViewModel>();
                charactersVM = _mapper.Map<List<Character>, List<CharacterGetResponseViewModel>>(characters);
                
                return Ok(charactersVM.OrderBy(x => x.Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }

            
        }
        [HttpGet]
        [Route("InfoCompletaPersonajes")]
        public IActionResult GetAllCharacterInfo()
        {
            try
            {
                var characters = _characterRepository.GetAllCharacters();
                if (characters == null) return NoContent();
                var charactersVM = new List<CharacterGetResponseViewModel>();
                charactersVM = _mapper.Map<List<Character>, List<CharacterGetResponseViewModel>>(characters);

                return Ok(charactersVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
        }
        [HttpPost]
        public IActionResult Post(CharacterPostRequestViewModel characterVM)
        {
            try
            {
                Character character = _mapper.Map<CharacterPostRequestViewModel, Character>(characterVM);
                if (characterVM.MovieOrSeriesId.Any())
                {
                    if (character.MovieOrSeries == null) 
                    {
                        character.MovieOrSeries = new List<MovieOrSerie>();
                    }
                    var movieOrSerieList = _movieOrSerieRepository.GetAllEntities();
                    character.MovieOrSeries = movieOrSerieList.Where(x => x.Id == characterVM.MovieOrSeriesId.FirstOrDefault(y => y == x.Id)).ToList();
                    
                }

                return Ok(_characterRepository.Add(character));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
        }
        [HttpPut]
        public IActionResult Put(CharacterPutViewModel characterVM)
        {
            
            try
            {
                var originalCharacter = _characterRepository.GetCharacter(characterVM.Id);
                if (originalCharacter == null) return BadRequest("El personaje no existe");

                _mapper.Map<CharacterPutViewModel, Character>(characterVM, originalCharacter);
              
                if (characterVM.MovieOrSeriesId.Any())
                {
                    var movieOrSerieList = _movieOrSerieRepository.GetAllEntities();

                    originalCharacter.MovieOrSeries = movieOrSerieList.Where(x => x.Id == characterVM.MovieOrSeriesId.FirstOrDefault(y => y == x.Id)).ToList();
                    
                }
                _characterRepository.Update(originalCharacter);
                return Ok(originalCharacter);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult Delete(int id)
        {
            
            try
            {
                var originalCharacter = _characterRepository.GetCharacter(id);
                if (originalCharacter == null) return BadRequest("El personaje no existe");

                _characterRepository.Delete(originalCharacter.Id);
                return Ok("Eliminado");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
           
        }
       
        
    }
}
