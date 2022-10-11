using System;
namespace WebApi.Models
{
    /**
    * Position change DTO for easier API manipulation
    */
    public class PositionChangeDTO
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual int? PositionId { get; set; }
    }
}