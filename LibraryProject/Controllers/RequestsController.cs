using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibraryProject.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Requests.Include(r => r.Book).Include(r => r.Receiver).Include(r => r.Requester);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Book)
                .Include(r => r.Receiver)
                .Include(r => r.Requester)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["RequesterId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Dinleyici")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,BookId,ReceiverId")] Request request)
       {
            if (ModelState.IsValid)
            {
                request.CreatedDate = DateTime.Now;

                var userEmail = User.Identity.Name;
                var user = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

                request.RequesterId = user.Id;

                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("UserProfile", "Profile", new { id = request.ReceiverId });
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", request.BookId);
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", request.ReceiverId);
            ViewData["RequesterId"] = new SelectList(_context.Users, "Id", "Id", request.RequesterId);
            return RedirectToAction("UserProfile", "Profile", new { id = request.ReceiverId });
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", request.BookId);
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", request.ReceiverId);
            ViewData["RequesterId"] = new SelectList(_context.Users, "Id", "Id", request.RequesterId);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,CreatedDate,BookId,RequesterId,ReceiverId")] Request request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", request.BookId);
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", request.ReceiverId);
            ViewData["RequesterId"] = new SelectList(_context.Users, "Id", "Id", request.RequesterId);
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Book)
                .Include(r => r.Receiver)
                .Include(r => r.Requester)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            var userEmail = User.Identity.Name;
            var user = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

            if (request.RequesterId != user.Id)
            {
                return Forbid();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile", "Profile");
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
