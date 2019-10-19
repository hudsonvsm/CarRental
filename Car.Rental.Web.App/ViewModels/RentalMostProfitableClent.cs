using System;

namespace Car.Rental.Web.App.ViewModels
{
    public class RentalMostProfitableClent
    {
        public string FullName { get; set; }
        public string Adrress { get; set; }
        public DateTime DriverLicenseValidUntil { get; set; }
        public decimal EarnedMoney { get; set; }
    }
}