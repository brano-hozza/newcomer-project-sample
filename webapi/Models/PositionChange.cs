
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models
{
    public class PositionChange
    {
        public int Id { get; set; }

        [Required]
        public string StartDate { get; set; } = "";
        [Required]
        public string EndDate { get; set; } = "";
        [Required]
        [Key, ForeignKey("FK_User")]
        public virtual User User { get; set; } = null!;
        [Required]
        [Key, ForeignKey("FK_Position")]
        public virtual Position Position { get; set; } = null!;
    }
}