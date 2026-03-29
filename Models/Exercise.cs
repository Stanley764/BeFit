using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models
{
    public class Exercise
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser? User { get; set; }

        [Display(Name = "Ciężar")]
        [Required(ErrorMessage = "Podaj ciężar")]
        [Range(0, 1000, ErrorMessage = "Ciężar musi być w zakresie 0 - 1000")]
        public int Weight { get; set; }

        [Display(Name = "Serie")]
        [Required(ErrorMessage = "Podaj liczbę serii")]
        [Range(0, 1000, ErrorMessage = "Liczba serii musi być w zakresie 0 - 1000")]
        public int NumOfSeries { get; set; }

        [Display(Name = "Powtórzenia")]
        [Required(ErrorMessage = "Podaj liczbę powtórzeń")]
        [Range(0, 1000, ErrorMessage = "Liczba powtórzeń musi być w zakresie 0 - 1000")]
        public int NumOfReps { get; set; }

        [Display(Name = "Rodzaj ćwiczenia")]
        public int ExerciseTypeId { get; set; }

        [Display(Name = "Rodzaj ćwiczenia")]
        public virtual ExerciseType? ExerciseType { get; set; }

        [Display(Name = "Sesja")]
        public int SessionId { get; set; }

        [Display(Name = "Sesja")]
        public virtual Session? Session { get; set; }
    }
}