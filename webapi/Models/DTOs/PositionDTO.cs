using WebApi.Models;

namespace WebApi.DTOs
{
    /**
    * Position DTO for easier API manipulation
    */
    public class PositionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public PositionDTO() { }

        public PositionDTO(Position position)
        {
            this.Name = position.Name;
        }
    }
}