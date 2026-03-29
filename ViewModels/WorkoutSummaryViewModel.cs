using System.ComponentModel.DataAnnotations;

namespace BeFit.ViewModels;

public class WorkoutSummaryViewModel
{
    [Display(Name = "Id ćwiczenia")]
    public int Id { get; set; }

    [Display(Name = "Nazwa ćwiczenia")]
    public string? ExerciseName { get; set; }

    [Display(Name = "Łączna liczba sesji")]
    public int TotalSessionCount { get; set; }

    [Display(Name = "Łączna liczba powtórzeń (powtórzenia × serie)")]
    public int TotalReps { get; set; }

    [Display(Name = "Średnia waga")]
    public double AverageWeight { get; set; }

    [Display(Name = "Maksymalna waga")]
    public int PeakWeight { get; set; }
}