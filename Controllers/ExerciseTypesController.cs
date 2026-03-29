using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BeFit.Data;
using BeFit.Models;

namespace BeFit.Controllers
{
    public class ExerciseTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExerciseTypesController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index() =>
            View(await _db.ExerciseType.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var type = await _db.ExerciseType.FirstOrDefaultAsync(m => m.Id == id);
            return type == null ? NotFound() : View(type);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name")] ExerciseType exerciseType)
        {
            if (!ModelState.IsValid) return View(exerciseType);

            _db.Add(exerciseType);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var type = await _db.ExerciseType.FindAsync(id);
            return type == null ? NotFound() : View(type);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ExerciseType exerciseType)
        {
            if (id != exerciseType.Id) return NotFound();
            if (!ModelState.IsValid) return View(exerciseType);

            try
            {
                _db.Update(exerciseType);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseTypeExists(exerciseType.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var type = await _db.ExerciseType.FirstOrDefaultAsync(m => m.Id == id);
            return type == null ? NotFound() : View(type);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var type = await _db.ExerciseType.FindAsync(id);
            if (type != null) _db.ExerciseType.Remove(type);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseTypeExists(int id) => _db.ExerciseType.Any(e => e.Id == id);
    }
}