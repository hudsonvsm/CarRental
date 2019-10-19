namespace Car.Rental.Web.App.ViewModels
{
    public class RentalMostRentedVehicle
    {
        public string LicensePlate { get; set; }
        public string BrandAndModel { get; set; }
        public decimal PricePerDay { get; set; }
        public decimal TotalDaysRented { get; set; }
        public int RentsCount { get; set; }
    }
}