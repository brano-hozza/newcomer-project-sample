using System;
namespace WebApi.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartDate { get; set; }
        public float Salary { get; set; }
        public int Position { get; set; }
        public DateTime? ResignedDate { get; set; }
    }
}