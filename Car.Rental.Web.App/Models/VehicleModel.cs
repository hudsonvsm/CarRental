using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car.Rental.Web.App.Models
{
    public class VehicleModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid VehicleBrandId { get; set; }
        public virtual VehicleBrand VehicleBrand { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }

        [Range(3, 10)]
        public int MaxPassengers { get; set; }

        public bool BigLuggage { get; set; }
    }
}