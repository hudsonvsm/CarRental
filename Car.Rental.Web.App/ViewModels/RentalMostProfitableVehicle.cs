using System;

namespace Car.Rental.Web.App.ViewModels
{
    public class RentalMostProfitableVehicle
    {
        public string LicensePlate { get; set; }
        public string BrandModel { get; set; }
        public decimal PricePerDay { get; set; }
        public decimal TotalDaysRented { get; set; }
        public decimal EarnedMoney { get; set; }
    }
}