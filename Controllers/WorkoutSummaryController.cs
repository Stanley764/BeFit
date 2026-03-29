using BeFit.Data;
using BeFit.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class WorkoutSummaryController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public WorkoutSummaryController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var uid = _userManager.GetUserId(User);
            var cutoff = DateTime.Now.AddDays(-28);

            var exercises = await _db.Exercise
                .Where(e => e.UserId == uid && e.Session.Start >= cutoff)
                .Include(e => e.Session)
                .Include(e => e.ExerciseType)
                .ToListAsync();

            var summary = exercises
                .GroupBy(e => new { e.ExerciseTypeId, e.ExerciseType?.Name })
                .Select(BuildSummary)
                .ToList();

            return View(summary);
        }

        private static WorkoutSummaryViewModel BuildSummary(IGrouping<dynamic, Models.Exercise> g)
        {
            return new WorkoutSummaryViewModel
            {
                Id = g.Key.ExerciseTypeId,
                ExerciseName = g.Key.Name,
                TotalSessionCount = g.Count(),
                TotalReps = g.Sum(e => e.NumOfReps * e.NumOfSeries),
                AverageWeight = Math.Round(g.Average(e => e.Weight), 2),
                PeakWeight = g.Max(e => e.Weight),
            };
        }
    }
}