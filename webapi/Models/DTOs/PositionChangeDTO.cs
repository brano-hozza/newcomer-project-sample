using System;
namespace WebApi.DTOs
{
    /**
    * Position change DTO for easier API manipulation
    */
    public class PositionChangeDTO
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PositionId { get; set; }

        public PositionChangeDTO() { }
    }
}