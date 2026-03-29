using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Display(Name = "Początek sesji")]
        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana")]
        [DataType(DataType.DateTime, ErrorMessage = "Nieprawidłowy format daty")]
        public DateTime Start { get; set; }

        [Display(Name = "Koniec sesji")]
        [Required(ErrorMessage = "Data zakończenia jest wymagana")]
        [DataType(DataType.DateTime, ErrorMessage = "Nieprawidłowy format daty")]
        public DateTime End { get; set; }

        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }
    }
}