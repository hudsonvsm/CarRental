using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car.Rental.Web.App.Models
{
    public enum Type
    {
        Bus, MiniBus, Truck, Car
    }

    public class Vehicle
    {
        public Vehicle()
        {
            this.Rentals = new HashSet<Rental>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string LicensePlate { get; set; }

        [Required]
        public Guid VehicleModelId { get; set; }
        public virtual VehicleModel VehicleModel { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "DateTime2")]
        public DateTime TechnicalInspectionDoneAt { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }

        [Required]
        public Type Type { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }
    }
}