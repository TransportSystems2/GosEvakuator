using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GosEvakuator.Controllers
{
    [Area("Workplace")]
    public class WorkplaceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}