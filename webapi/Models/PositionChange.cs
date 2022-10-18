
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models
{
    /**
    * Position change entity model
    */
    public class PositionChange
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        [Key, ForeignKey("FK_User")]
        public User User { get; set; } = null!;
        [Key, ForeignKey("FK_Position")]
        public Position? Position { get; set; }
    }
}