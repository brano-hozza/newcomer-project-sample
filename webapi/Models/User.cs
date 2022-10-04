
using System.ComponentModel.DataAnnotations;
namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Surname { get; set; } = "";

        public string Address { get; set; } = null!;

        [Required]
        public string BirthDate { get; set; } = "";

        [Required]
        public string StartDate { get; set; } = "";

        [Required]
        public int PositionId { get; set; }
        public Position Position { get; set; } = null!;

        [Required]
        public float Salary { get; set; }
    }
}