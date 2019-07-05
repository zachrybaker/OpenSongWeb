using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenSongWeb.Data;
using OpenSongWeb.Managers;
using Microsoft.Extensions.Logging;
using OpenSongWeb.Models;

namespace OpenSongWeb.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : Controller
    {
        private readonly IOSSongManager _songManager;

        public SongsController(IOSSongManager songManager)
        {
            _songManager = songManager;
        }

        /// <summary>
        /// list of songs, optionally by search criteria
        /// </summary>
        /// <param name="songSearchParameters"></param>
        /// <returns>List<OSSong> or error</returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> All([FromQuery]SongSearchParameters songSearchParameters = null)
        {
            SongFilterParameter songFilterParameter = null;
            if (songSearchParameters != null && !string.IsNullOrEmpty(songSearchParameters.Text))
            {
                songFilterParameter = new SongFilterParameter
                {
                    Author = songSearchParameters.type == SongSearchType.All || songSearchParameters.type == SongSearchType.Author ? songSearchParameters.Text : null,
                    Lyrics = songSearchParameters.type == SongSearchType.All || songSearchParameters.type == SongSearchType.Lyrics ? songSearchParameters.Text : null,
                    Title = songSearchParameters.type == SongSearchType.All || songSearchParameters.type == SongSearchType.Title ? songSearchParameters.Text : null,
                    Themes = songSearchParameters.type == SongSearchType.All || songSearchParameters.type == SongSearchType.Tags ? songSearchParameters.Text : null,
                    SearchCriteriaBuildType = SearchCriteriaBuildType.Any
                };
            }

            var result = await _songManager.All(songFilterParameter);

            return result.Match<IActionResult>(vm =>
                {
                    return Ok(vm);
                },
                (error) =>
                {
                    return BadRequest(new ErrorViewModel(error, HttpContext));
                }
            );
        }

        
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Song(int id, [FromQuery]string title = null)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var result = await _songManager.Get(id);
            OSSong song = result.LeftOrDefault();

            if (song != null)
            {
                return Ok(song);
            }

            if (result.RightOrDefault() != null)
            {
                return BadRequest(new ErrorViewModel(result.RightOrDefault(), HttpContext));
            }

            return NotFound();  
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Author,Copyright,Key,Presentation,Content,Capo,HymnNumber,CCLINumber,Themes,Filename,CreatedDateUTC,LastUpdatedDateUTC,CreatedByID")] OSSong oSSong)
        {
            return NotFound();
            //if (ModelState.IsValid)
            //{
            //    oSSong.ID = Guid.NewGuid();
            //    _context.Add(oSSong);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CreatedByID"] = new SelectList(_context.Users, "Id", "Id", oSSong.CreatedByID);
            //return View(oSSong);
            
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            return NotFound();
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var oSSong = await _context.Songs.FindAsync(id);
            //if (oSSong == null)
            //{
            //    return NotFound();
            //}
            //ViewData["CreatedByID"] = new SelectList(_context.Users, "Id", "Id", oSSong.CreatedByID);
            //return View(oSSong);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Title,Author,Copyright,Key,Presentation,Content,Capo,HymnNumber,CCLINumber,Themes,Filename,CreatedDateUTC,LastUpdatedDateUTC,CreatedByID")] OSSong oSSong)
        {
            return NotFound();
            //if (id != oSSong.ID)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(oSSong);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!OSSongExists(oSSong.ID))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CreatedByID"] = new SelectList(_context.Users, "Id", "Id", oSSong.CreatedByID);
            //return View(oSSong);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            return NotFound();
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var oSSong = await _context.OSSongs
            //    .Include(o => o.CreatedBy)
            //    .FirstOrDefaultAsync(m => m.ID == id);
            //if (oSSong == null)
            //{
            //    return NotFound();
            //}

            //return View(oSSong);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            return NotFound();
            //var oSSong = await _context.OSSongs.FindAsync(id);
            //_context.OSSongs.Remove(oSSong);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OSSongExists(int id)
        {
            return await _songManager.Exists(id);
        }
    }
}
