using System;

namespace Car.Rental.Web.App.ViewModels
{
    public class RentalViewModelClient
    {
        public string FullName { get; set; }
        public string LicensePlate { get; set; }
        public string VehicleBrandModel { get; set; }
        public DateTime RentedAt{ get; set; }
    }
}
