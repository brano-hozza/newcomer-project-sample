
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models
{
    /**
    * User entity model
    */
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Surname { get; set; } = "";

        public string? Address { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public float Salary { get; set; }

        public DateTime? ResignedDate { get; set; }
        // References
        public virtual Position Position { get; set; } = null!;

        [InverseProperty("User")]
        public virtual List<PositionChange> PositionChanges { get; set; } = new List<PositionChange>();
    }
}