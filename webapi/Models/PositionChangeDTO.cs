using System;
namespace WebApi.Models
{
    public class PositionChangeDTO
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual int? PositionId { get; set; }
    }
}