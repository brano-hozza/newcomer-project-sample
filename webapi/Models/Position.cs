
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models
{
    /**
    * Position entity model
    */
    public class Position
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [InverseProperty("Position")]
        public virtual List<User> Users { get; set; } = new List<User>();

        [InverseProperty("Position")]
        public virtual List<PositionChange> PositionChanges { get; set; } = new List<PositionChange>();
    }
}