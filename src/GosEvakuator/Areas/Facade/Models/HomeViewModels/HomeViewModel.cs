namespace GosEvakuator.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public string CityName { get; set; }

        public string PrepositionalCityName { get; set; }

        public string AreaName { get; set; }

        public string PrepositionalAreaName { get; set; }

        public string PhoneNumber { get; set; }

        public string FormattedPhoneNumber { get; set; }

        public OrderViewModel OrderViewModel { get; set; }

        public PricelistItem SelectedPricelistItem { get; set; }

        public int CurrentYear { get; set; }
    }
}
