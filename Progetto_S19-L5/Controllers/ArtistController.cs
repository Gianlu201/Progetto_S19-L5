﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Progetto_S19_L5.DTOs.Artist;
using Progetto_S19_L5.DTOs.Event;
using Progetto_S19_L5.Models;
using Progetto_S19_L5.Services;

namespace Progetto_S19_L5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ArtistService _artistService;

        public ArtistController(ArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist([FromBody] CreateArtistRequestDto newArtist)
        {
            try
            {
                var artist = new Artist()
                {
                    FirstName = newArtist.FirstName,
                    LastName = newArtist.LastName,
                    Genre = newArtist.Genre,
                    Biography = newArtist.Biography,
                };

                var result = await _artistService.CreateArtistAsync(artist);

                return result
                    ? Ok(new CreateArtistResponse() { Message = "Artist created successfully!" })
                    : BadRequest(new CreateArtistResponse() { Message = "Something went wrong!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _artistService.GetArtistsAsync();

            if (result == null)
            {
                return BadRequest(
                    new GetAllArtistsResponse()
                    {
                        Message = "Something went wrong!",
                        Artists = null,
                    }
                );
            }

            List<ArtistDto> artistsList = result
                .Select(r => new ArtistDto()
                {
                    ArtistId = r.ArtistId,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Genre = r.Genre,
                    Biography = r.Biography,
                    Events = r
                        .Events?.Select(e => new EventDto()
                        {
                            Eventid = e.Eventid,
                            Title = e.Title,
                            Date = e.Date,
                            Place = e.Place,
                            ArtistId = e.ArtistId,
                        })
                        .ToList(),
                })
                .ToList();

            return Ok(
                new GetAllArtistsResponse() { Message = "Artists found!", Artists = artistsList }
            );
        }

        [HttpGet("{artistId}")]
        public async Task<IActionResult> GetArtist(string artistId)
        {
            try
            {
                var artist = await _artistService.GetArtistByIdAsync(artistId);

                if (artist == null)
                {
                    return BadRequest(
                        new GetArtistResponse() { Message = "Something went wrong!", Artist = null }
                    );
                }

                var artistFound = new ArtistDto()
                {
                    ArtistId = artist.ArtistId,
                    FirstName = artist.FirstName,
                    LastName = artist.LastName,
                    Genre = artist.Genre,
                    Biography = artist.Biography,
                    Events = artist
                        .Events?.Select(e => new EventDto()
                        {
                            Eventid = e.Eventid,
                            Title = e.Title,
                            Date = e.Date,
                            Place = e.Place,
                            ArtistId = e.ArtistId,
                        })
                        .ToList(),
                };

                return Ok(
                    new GetArtistResponse() { Message = "Artist found!", Artist = artistFound }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditArtist(
            [FromQuery] string artistId,
            [FromBody] EditArtistRequestDto editArtist
        )
        {
            try
            {
                var result = await _artistService.EditArtistByIdAsync(artistId, editArtist);

                return result
                    ? Ok(new EditArtistResponse() { Message = "Artist modified successfully!" })
                    : BadRequest(new EditArtistResponse() { Message = "Something went wrong!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _artistService.DeleteArtistByIdAsync(id);

            return result
                ? Ok(new DeleteArtistResponse() { Message = "Artist deleted successfully" })
                : BadRequest(new DeleteArtistResponse() { Message = "Something went wrong!" });
        }
    }
}
