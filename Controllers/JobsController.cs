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
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: jobs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Job.ToListAsync());
        }
        // GET: jobs/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        // POST: jobs/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Job.Where(j => j.JobQuestion.Contains(SearchPhrase)).ToListAsync());
        }
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

        // GET: jobs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // POST: jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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

        private bool jobExists(int id)
        {
            return _context.Job.Any(e => e.Id == id);
        }
    }
}
