
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models
{
    /**
    * Position entity model
    */
    public sealed class Position
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [InverseProperty("Position")]
        public List<User> Users { get; set; } = new List<User>();

        [InverseProperty("Position")]
        public List<PositionChange> PositionChanges { get; set; } = new List<PositionChange>();
    }
}