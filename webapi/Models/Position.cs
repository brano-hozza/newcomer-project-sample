
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models
{
    public class Position
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [InverseProperty("Position")]
        public List<User> Users { get; set; } = null!;

        [InverseProperty("Position")]
        public List<PositionChange> PositionChanges { get; set; } = null!;
    }
}