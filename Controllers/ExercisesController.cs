using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BeFit.Data;
using BeFit.Models;

namespace BeFit.Controllers
{
    [Authorize]
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public ExercisesController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var uid = _userManager.GetUserId(User);
            var list = await _db.Exercise
                .Where(e => e.UserId == uid)
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var exercise = await _db.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercise == null) return NotFound();
            if (exercise.UserId != _userManager.GetUserId(User)) return Forbid();

            return View(exercise);
        }

        public IActionResult Create()
        {
            var uid = _userManager.GetUserId(User);
            ViewData["ExerciseTypeId"] = new SelectList(_db.ExerciseType, "Id", "Name");
            ViewData["SessionId"] = new SelectList(_db.Session.Where(s => s.UserId == uid), "Id", "Start");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Weight,NumOfSeries,NumOfReps,ExerciseTypeId,SessionId")] Exercise exercise)
        {
            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
            {
                ViewData["ExerciseTypeId"] = new SelectList(_db.ExerciseType, "Id", "Id", exercise.ExerciseTypeId);
                ViewData["SessionId"] = new SelectList(_db.Session, "Id", "Id", exercise.SessionId);
                return View(exercise);
            }

            var uid = _userManager.GetUserId(User)!;
            exercise.UserId = uid;

            var session = await _db.Session.FindAsync(exercise.SessionId);
            if (session?.UserId != uid) return Forbid();

            _db.Add(exercise);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var exercise = await _db.Exercise.FindAsync(id);
            if (exercise == null) return NotFound();
            if (exercise.UserId != _userManager.GetUserId(User)) return Forbid();

            var uid = _userManager.GetUserId(User);
            ViewData["ExerciseTypeId"] = new SelectList(_db.ExerciseType, "Id", "Id", exercise.ExerciseTypeId);
            ViewData["SessionId"] = new SelectList(_db.Session.Where(s => s.UserId == uid), "Id", "Id", exercise.SessionId);
            return View(exercise);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Weight,NumOfSeries,NumOfReps,ExerciseTypeId,SessionId")] Exercise exercise)
        {
            if (id != exercise.Id) return NotFound();
            if (exercise.UserId != _userManager.GetUserId(User)) return Forbid();

            if (!ModelState.IsValid)
            {
                ViewData["ExerciseTypeId"] = new SelectList(_db.ExerciseType, "Id", "Id", exercise.ExerciseTypeId);
                ViewData["SessionId"] = new SelectList(_db.Session, "Id", "Id", exercise.SessionId);
                return View(exercise);
            }

            try
            {
                _db.Update(exercise);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(exercise.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var exercise = await _db.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercise == null) return NotFound();
            if (exercise.UserId != _userManager.GetUserId(User)) return Forbid();

            return View(exercise);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _db.Exercise.FindAsync(id);
            if (exercise?.UserId != _userManager.GetUserId(User)) return Forbid();

            if (exercise != null) _db.Exercise.Remove(exercise);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id) => _db.Exercise.Any(e => e.Id == id);
    }
}