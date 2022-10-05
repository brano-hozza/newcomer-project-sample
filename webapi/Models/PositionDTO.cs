
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApi.Models
{
    public class PositionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}