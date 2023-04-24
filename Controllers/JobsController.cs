using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoLast.Data;
using ToDoLast.Models;
using Microsoft.AspNetCore.Authorization;
using ToDoLast.Data;

namespace ToDoLast.Controllers
{
    /// <summary>
    /// Kontroler obsługujący widoki dla obiektów typu Job.
    /// </summary>
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Wyświetla listę wszystkich obiektów typu Job.
        /// </summary>
        /// <returns>Widok z listą wszystkich obiektów typu Job.</returns>
        // GET: jobs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Job.ToListAsync());
        }
        /// <summary>
        /// Wyświetla formularz wyszukiwania obiektów typu Job.
        /// </summary>
        /// <returns>Widok z formularzem wyszukiwania obiektów typu Job.</returns>
        // GET: jobs/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        /// <summary>
        /// Wyświetla wyniki wyszukiwania obiektów typu Job na podstawie podanej frazy.
        /// </summary>
        /// <param name="SearchPhrase">Fraza do wyszukania.</param>
        /// <returns>Widok z listą obiektów typu Job spełniających kryteria wyszukiwania.</returns>
        // POST: jobs/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Job.Where(j => j.JobQuestion.Contains(SearchPhrase)).ToListAsync());
        }
        /// <summary>
        /// Wyświetla szczegóły obiektu typu Job o podanym id.
        /// </summary>
        /// <param name="id">Id obiektu typu Job.</param>
        /// <returns>Widok z szczegółami obiektu typu Job.</returns>
        // GET: jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }
        /// <summary>
        /// Wyświetla formularz tworzenia nowego obiektu typu Job.
        /// </summary>
        /// <returns>Widok z formularzem tworzenia nowego obiektu typu Job.</returns>
        // GET: jobs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Tworzy nowy obiekt typu Job na podstawie danych z formularza.
        /// </summary>
        /// <param name="job">Obiekt typu Job z danymi z formularza.</param>
        /// <returns>Przekierowanie do widoku z listą wszystkich obiektów typu Job.</returns>
        // POST: jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Job job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(job);
        }
        /// <summary>
        /// Wyświetla formularz do edycji istniejącego zlecenia.
        /// </summary>
        /// <param name="id">Identyfikator zlecenia do edycji.</param>
        /// <returns>Widok zawierający formularz do edycji istniejącego zlecenia.</returns>
        // GET: jobs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        // POST: jobs/Edit/5
        /// <summary>
        /// Aktualizuje istniejące zlecenie na podstawie danych przesłanych z formularza.
        /// </summary>
        /// <param name="id">Identyfikator zlecenia do aktualizacji.</param>
        /// <param name="job">Dane zaktualizowanego zlecenia.</param>
        /// <returns>Przekierowanie na stronę z listą zleceń.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!jobExists(job.Id))
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
            return View(job);
        }
        /// <summary>
        /// Wyświetla formularz potwierdzenia usunięcia istniejącego zlecenia.
        /// </summary>
        /// <param name="id">Identyfikator zlecenia do usunięcia.</param>
        /// <returns>Widok zawierający formularz potwierdzenia usunięcia istniejącego zlecenia.</returns>
        // GET: jobs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }
        /// </summary>
        /// <param name="id">Identyfikator zlecenia do usunięcia.</param>
        /// <returns>Przekierowanie na stronę z listą zleceń po wykonaniu operacji usuwania.</returns>
        // POST: jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Sprawdza, czy encja 'Job' jest niepusta
            if (_context.Job == null)
            {
                return Problem("Entity set 'ApplicationDbContext.job'  is null.");
            }
            var job = await _context.Job.FindAsync(id);
            if (job != null)
            {
                _context.Job.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Metoda pomocnicza do sprawdzenia, czy zlecenie o danym id istnieje w bazie danych
        private bool jobExists(int id)
        {
            return _context.Job.Any(e => e.Id == id);
        }
    }
}
