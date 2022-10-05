namespace WebApi.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Address { get; set; } = null!;
        public string BirthDate { get; set; } = "";
        public string StartDate { get; set; } = "";
        public float Salary { get; set; }
        public int Position { get; set; }
        public bool Resigned { get; set; }
    }
}