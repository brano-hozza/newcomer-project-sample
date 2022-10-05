
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models
{
    public class PositionChange
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        [Key, ForeignKey("FK_User")]
        public virtual User User { get; set; } = null!;
        [Required]
        [Key, ForeignKey("FK_Position")]
        public virtual Position Position { get; set; } = null!;
    }
}