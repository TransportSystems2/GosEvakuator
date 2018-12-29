using GosEvakuator.Data;
using GosEvakuator.Handlers;
using GosEvakuator.Models.HomeViewModels;
using GosEvakuator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Controllers
{
    [Area("Facade")]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ISmsSender smsService;

        public HomeController(ApplicationDbContext context, ISmsSender smsSender)
        {
            dbContext = context;
            smsService = smsSender;
        }

        public IActionResult Index()
        {
            var cityDomain = HttpContext.Features.Get<ISubdomain>();
            var city = dbContext.City.Include(c => c.Garages).ThenInclude(g => g.Pricelist).ThenInclude(p => p.Items)
                .SingleOrDefault(c => c.Domen.Equals(cityDomain.Value));
            var garage = city.GetMasterGarage();

            var formattedPhoneNumber = string.Format(garage.PhoneNumberMask, garage.PhoneNumber);
            var selectedPricelistItem = garage.Pricelist.GetDefaultedPricelistItem();

            var model = new HomeViewModel()
            {
                CityName = city.Name,
                PrepositionalCityName = city.PrepositionalName,
                AreaName = city.Area,
                PrepositionalAreaName = city.PrepositionalArea,
                PhoneNumber = "+" + garage.PhoneNumber,
                FormattedPhoneNumber = formattedPhoneNumber,
                SelectedPricelistItem = selectedPricelistItem,
                OrderViewModel = new OrderViewModel { Pricelist = garage.Pricelist },
                CurrentYear = DateTime.Now.Year
            };

            ViewData["Title"] = String.Format("Эвакуатор в {0}, от {1} рублей.", city.PrepositionalName, selectedPricelistItem.LoadingVehicle);
            ViewData["Description"] = String.Format("Срочно нужен недорогой эвакуатор в {0} или {1} области? Звоните в компанию \"Госэвакуатор\" {2}, мы предлагаем самые низкие цены на эвакуацию автомобиля от {3} рублей!", city.PrepositionalName, city.PrepositionalArea, formattedPhoneNumber, selectedPricelistItem.LoadingVehicle);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromForm] OrderViewModel model)
        {
            var cityDomain = HttpContext.Features.Get<ISubdomain>();
            var city = dbContext.City.Include(c => c.Garages).ThenInclude(g => g.Pricelist).ThenInclude(p => p.Items)
                .SingleOrDefault(c => c.Domen.Equals(cityDomain.Value));
            var garage = city.GetMasterGarage();

            await smsService.SendSmsAsync(garage.PhoneNumber.ToString(), string.Format("Новый заказ. Телефон: {0}", model.CustomerPhoneNumber));
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Calculate([FromForm] CalculateViewModel model)
        {
            var cityDomain = HttpContext.Features.Get<ISubdomain>();
            var city = dbContext.City.Include(c => c.Garages).ThenInclude(g => g.Pricelist).ThenInclude(p => p.Items)
                .SingleOrDefault(c => c.Domen.Equals(cityDomain.Value));
            var garage = city.GetMasterGarage();

            await smsService.SendSmsAsync(garage.PhoneNumber.ToString(), string.Format("Новый заказ. Телефон: {0}", model.CustomerPhoneNumber));

            return Ok();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}