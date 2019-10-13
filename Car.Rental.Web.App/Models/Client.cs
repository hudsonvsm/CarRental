using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Car.Rental.Web.App.Models
{
    public class Client
    {
        public Client()
        {
            this.Rentals = new HashSet<Rental>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(8)]
        public string IdentificationNumber { get; set; } // ЕГН

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        public DriverLicense DriverLicense { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }
    }
}