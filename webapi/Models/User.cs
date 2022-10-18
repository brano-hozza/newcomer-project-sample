
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.DTOs;

namespace WebApi.Models
{
    /**
    * User entity model
    */
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string? Address { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public float Salary { get; set; }

        public DateTime? ResignedDate { get; set; }
        // References
        public Position Position { get; set; }
        public int PositionId { get; set; }
        [InverseProperty("User")]
        public List<PositionChange> PositionChanges { get; set; } = new List<PositionChange>();

        public User() { }
        public User(Position position, UserDTO dto)
        {
            this.Position = position;
            this.PositionId = position.Id;
            this.Name = dto.Name;
            this.Surname = dto.Surname;
            this.BirthDate = dto.BirthDate;
            this.StartDate = dto.StartDate;
            this.Salary = dto.Salary;
        }
    }
}