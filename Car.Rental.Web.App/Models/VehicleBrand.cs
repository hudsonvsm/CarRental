using System;
using System.ComponentModel.DataAnnotations;

namespace Car.Rental.Web.App.Models
{
    public class VehicleBrand
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}