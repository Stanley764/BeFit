using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa ćwiczenia")]
        [Required(ErrorMessage = "Nazwa ćwiczenia jest wymagana")]
        [MaxLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [MinLength(2, ErrorMessage = "Minimalnie 2 znaki")]
        public string Name { get; set; }
    }
}