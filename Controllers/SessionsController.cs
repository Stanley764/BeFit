using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BeFit.Data;
using BeFit.Models;

namespace BeFit.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public SessionsController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var uid = _userManager.GetUserId(User);
            var list = await _db.Session.Where(s => s.UserId == uid).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var session = await _db.Session.FirstOrDefaultAsync(s => s.Id == id);
            if (session == null) return NotFound();
            if (session.UserId != _userManager.GetUserId(User)) return Forbid();

            return View(session);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Start,End")] Session session)
        {
            if (!ModelState.IsValid) return View(session);

            session.UserId = _userManager.GetUserId(User);
            _db.Add(session);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var session = await _db.Session.FindAsync(id);
            if (session == null) return NotFound();
            if (session.UserId != _userManager.GetUserId(User)) return Forbid();

            return View(session);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,End")] Session session)
        {
            if (id != session.Id) return NotFound();
            if (session.UserId != _userManager.GetUserId(User)) return Forbid();
            if (!ModelState.IsValid) return View(session);

            try
            {
                _db.Update(session);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(session.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var session = await _db.Session.FirstOrDefaultAsync(s => s.Id == id);
            if (session == null) return NotFound();
            if (session.UserId != _userManager.GetUserId(User)) return Forbid();

            return View(session);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _db.Session.FindAsync(id);
            if (session?.UserId != _userManager.GetUserId(User)) return Forbid();

            if (session != null) _db.Session.Remove(session);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id) => _db.Session.Any(s => s.Id == id);
    }
}