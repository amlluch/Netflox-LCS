using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Netflox.Models;
using Netflox.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace Netflox.Controllers
{
    public class MoviesController : Controller
    {
        private readonly Context _context;

        public string ImageUrl { get; private set; }

        public MoviesController(Context context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            var movies = _context.Movies.OrderBy(x => x.Name).Skip((pagina - 1) * cantidadRegistrosPorPagina).Take(cantidadRegistrosPorPagina).ToList();
            var totalDeRegistros = _context.Movies.Count();
            var modelo = new ViewMovie
            {
                Movies = movies,
                PaginaActual = pagina,
                TotalDeRegistros = totalDeRegistros,
                RegistrosPorPagina = cantidadRegistrosPorPagina
            };

            ViewData["partial"] = modelo;
            if (modelo.TotalDeRegistros > cantidadRegistrosPorPagina) {
                ViewBag.pie = true;
            } else { ViewBag.pie = false; }
            

            return View(modelo.Movies.ToList());

        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,Name,Director,PicName,Pic")] Movie movie, IFormFile Imagen, IFormFile Video)
        {
            if (ModelState.IsValid)
            {
                if (Imagen != null)
                {
                        var fichero = Path.GetFileName(Imagen.FileName);
                        movie.PicName = fichero;
                    var ftpserver = new UploadFile("uploadimages", "proyecto3", "ftp://anonvpn.net/");
                    byte[] FileToDB = await ftpserver.sendfileAsync(fichero, Imagen);
                    if (FileToDB != null) { movie.Pc = FileToDB; }
                }

                if (Video != null)
                {
                    var nombrefichero = Path.GetFileName(Video.FileName);
                    movie.VideoName = nombrefichero;
                    var ftpserver = new UploadFile("uploadvideos", "proyecto3", "ftp://anonvpn.net/");
                    byte[] FileToDB = await ftpserver.sendfileAsync(nombrefichero, Video);
                    if (FileToDB != null) { movie.Vd = FileToDB; }
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            //         var actores = _context.Actors.OrderBy(x => x.ActorName).Select(x =>  new { x.ActorId, x.ActorName }).ToList();
            //          ViewBag.ActorId = new SelectList(actores, nameof(Actor.ActorId), nameof(Actor.ActorName));
            //            ViewBag.ActorId = _context.Actors.OrderBy(x => x.ActorName).ToList();

            ViewBag.ActorId = (from actores in _context.Actors
                           
                        where actores.MoviesLink.All(c => c.MovieId != id)
                        
                        select actores).ToList();
                 
            
            var movie = await _context.Movies.FindAsync(id);
            Actor actor = await _context.Actors.FirstOrDefaultAsync(x =>x.ActorId == 4);



            if (movie == null)
            {
                return NotFound();
            }
            else
            {
                _context.Movies.Attach(movie);
                _context.Actors.Attach(actor);
                //              actor.Movies.Add(movie);

                //              movie.ActorsLink.Add(new MovieActor(actor));

                await _context.SaveChangesAsync();
            }
            return View(movie);
        }

        //Play: Movies/Play/5
        public async Task<IActionResult> Play(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }



        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(include:"MovieId,Name,Director,Pic,PicName,Video,VideoName")] Movie movie, IFormFile Imagen, IFormFile Video)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(movie).State = EntityState.Modified;
                if (Imagen != null)
                {
                    var fichero = Path.GetFileName(Imagen.FileName);
                    movie.PicName = fichero;
                    var ftpserver = new UploadFile("uploadimages", "proyecto3", "ftp://anonvpn.net/");
                    byte[] FileToDB = await ftpserver.sendfileAsync(fichero, Imagen);
                    if (FileToDB != null) { movie.Pc = FileToDB; }
                } else
                {
                    _context.Entry(movie).Property(x => x.PicName).IsModified = false;
                    _context.Entry(movie).Property( x => x.Pc).IsModified = false;
;
                   
                }

                if (Video != null)
                {
                    var nombrefichero = Path.GetFileName(Video.FileName);
                    movie.VideoName = nombrefichero;
                    var ftpserver = new UploadFile("uploadvideos", "proyecto3", "ftp://anonvpn.net/");
                    byte[] FileToDB = await ftpserver.sendfileAsync(nombrefichero, Video);
    //                if (FileToDB != null) { movie.Vd = FileToDB; }
                } else
                {
                    _context.Entry(movie).Property(x => x.VideoName).IsModified = false;
                    _context.Entry(movie).Property(x => x.Vd).IsModified = false;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

 
        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
