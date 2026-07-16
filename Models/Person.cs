using System.ComponentModel.DataAnnotations;

namespace Pozdravlyator.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [StringLength(100)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите фамилию")]
        [StringLength(100)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите дату рождения")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [StringLength(300)]
        [Display(Name = "Заметка (должность, отдел, как познакомились и т.п.)")]
        public string? Note { get; set; }

        [Display(Name = "Фото")]
        public string? PhotoPath { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public int UpcomingAge
        {
            get
            {
                var today = DateTime.Today;
                var next = NextOccurrence(today);
                return next.Year - BirthDate.Year;
            }
        }

        public DateTime NextOccurrence(DateTime today)
        {
            var candidate = new DateTime(today.Year, BirthDate.Month, BirthDate.Day);
            if (candidate < today.Date)
                candidate = candidate.AddYears(1);
            return candidate;
        }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool IsToday => BirthDate.Month == DateTime.Today.Month && BirthDate.Day == DateTime.Today.Day;
    }
}