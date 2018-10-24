using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Netflox.Models;
using Netflox.ViewModels;
using static System.Net.WebRequestMethods;

namespace Netflox.Controllers
{
    public class ActorsController : Controller
    {
        private readonly Context _context;

        public ActorsController(Context context)
        {
            _context = context;
        }

        // GET: Actors
        public IActionResult Index(int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;
            var actores = _context.Actors.OrderBy(x => x.ActorName).Skip((pagina - 1) * cantidadRegistrosPorPagina).Take(cantidadRegistrosPorPagina).ToList();
            var totalDeRegistros = _context.Actors.Count();
            var modelo = new ViewActor
            {
                Actors = actores,
                PaginaActual = pagina,
                TotalDeRegistros = totalDeRegistros,
                RegistrosPorPagina = cantidadRegistrosPorPagina
            };

            ViewData["partial"] = modelo;
            if (modelo.TotalDeRegistros > cantidadRegistrosPorPagina)
            {
                ViewBag.pie = true;
            }
            else { ViewBag.pie = false; }

            return View(modelo.Actors.ToList());

  //          return View(await _context.Actors.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,ActorName,Birthdate,PicName")] Actor actor, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                if (Imagen != null)
                {
                    var fichero = Path.GetFileName(Imagen.FileName);
                    actor.PicName = fichero;
                    var ftpserver = new UploadFile("uploadimages", "proyecto3", "ftp://anonvpn.net/");
                    byte[] FileToDB = await ftpserver.sendfileAsync(fichero, Imagen);
                    if (FileToDB != null) { actor.Pc = FileToDB; }
                }
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,ActorName,Birthdate,PicName")] Actor actor, IFormFile Imagen)
        {
            if (id != actor.ActorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(actor).State = EntityState.Modified;
                if (Imagen != null)
                {
                    var fichero = Path.GetFileName(Imagen.FileName);
                    actor.PicName = fichero;
                    var ftpserver = new UploadFile("uploadimages", "proyecto3", "ftp://anonvpn.net/" );
                    byte [] FileToDB = await ftpserver.sendfileAsync(fichero, Imagen);
                    if (FileToDB != null) { actor.Pc = FileToDB; }
  
                }
                else
                {

                    _context.Entry(actor).Property(x => x.Pc).IsModified = false;
                    _context.Entry(actor).Property(x => x.PicName).IsModified = false;

                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.ActorId))
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
            return View(actor);
        }

        private Task UploadFile(string v)
        {
            throw new NotImplementedException();
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.ActorId == id);
        }
    }
}
