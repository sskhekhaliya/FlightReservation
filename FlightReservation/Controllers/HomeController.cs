using FlightReservation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        public HomeController(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public IActionResult Index(string Date, string Class, string From, string FAirport, string TAirport, string To, string btn)
        {
            ViewBag.Date = Date;
            ViewBag.Class = Class;
            ViewBag.From = From;
            ViewBag.To = To;
            ViewBag.FAirport = FAirport;
            ViewBag.TAirport = TAirport;
            ViewBag.Btn = btn;

            if(User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Admin", "Home");
            }

            if (!String.IsNullOrEmpty(btn))
            {
                if (String.IsNullOrEmpty(Date) || String.IsNullOrEmpty(Class) || String.IsNullOrEmpty(From) || String.IsNullOrEmpty(To))
                {
                    ModelState.AddModelError("", "Fill all the fields.");
                } else
                {
                    string boarding = From.Split(" - ")[0];
                    string departure = To.Split(" - ")[0];

                    var result = context.Schedules
                        .Include(e => e.FlightDetail)
                        .Where(e => e.DepartureTime.Date == DateTime.Parse(Date).Date && e.From == boarding && e.To == departure);

                    return View(result);
                }
            }
            return View();
        }

        //GET: Admin Pannel
        public IActionResult Admin()
        {
            return View();
        }
    }
}
