
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
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
        public float Salary { get; set; }

        public bool Resigned { get; set; }
        // References
        public virtual Position Position { get; set; } = null!;

        [InverseProperty("User")]
        public List<PositionChange> PositionChanges { get; set; } = null!;

    }
}