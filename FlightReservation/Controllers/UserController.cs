using FlightReservation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        readonly private ApplicationDbContext context;
        public UserController(ApplicationDbContext _context)
        {
            this.context = _context;
        }
        public async Task<IActionResult> BookingAsync(string Id, string Class, string From, string To, string FAirport, string TAirport)
        {
            ViewBag.Class = Class;
            ViewBag.From = From;
            ViewBag.To = To;
            ViewBag.TAirport = TAirport;
            ViewBag.FAirport = FAirport;

            if (String.IsNullOrEmpty(Id) || String.IsNullOrEmpty(Class) || String.IsNullOrEmpty(From) || String.IsNullOrEmpty(To))
            {
                return RedirectToAction("Index", "Home");
            }

            var flightDetail = await context.Schedules
                .Include(s => s.FlightDetail)
                .FirstOrDefaultAsync(m => m.ScheduleID == int.Parse(Id));

            return View(flightDetail);
        }

        public async Task<IActionResult> WalletAsync(string status, string err)
        {
            var userDetail = await context.UserDetails
                .FirstOrDefaultAsync(m => m.EmailID == User.Identity.Name);

            ViewBag.Balance = userDetail.Wallet;
            ViewBag.Status = status;
            ViewBag.Error = err;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBalance(int money)
        {
            if(money == 0)
            {
                return RedirectToAction("Wallet", new { err = "Enter ammount" });
            }
            var userDetail = await context.UserDetails
                .FirstOrDefaultAsync(m => m.EmailID == User.Identity.Name);

            userDetail.Wallet += money;

            int result = context.SaveChanges();

            if(result > 0)
            {
                return RedirectToAction("Wallet", new { status = "success" });
            }
            return RedirectToAction("Error", "Info");
        }

        public async Task<IActionResult> OrdersAsync()
        {
            var userDetail = await context.UserDetails
                .FirstOrDefaultAsync(m => m.EmailID == User.Identity.Name);

            var details = context.Bookings
                .Include(e => e.UserDetail)
                .Include(e => e.Schedule)
                .Include(e => e.Schedule.FlightDetail)
                .Where(e => e.User == userDetail.UserID && e.Schedule.DepartureTime.Date >= DateTime.Now.Date);

            return View(details);
        }

        public async Task<IActionResult> ETicketAsync(string Id)
        {
            if (String.IsNullOrEmpty(Id))
            {
                return Redirect("Orders");
            }

            var bookingDetails = await context.Bookings
                .Include(b => b.Schedule)
                .Include(b => b.Schedule.FlightDetail)
                .Include(b => b.UserDetail)
                .FirstOrDefaultAsync(b => b.BookingID == int.Parse(Id));

            // Checking if user is want to watch only their Booking
            if(bookingDetails.UserDetail.EmailID != User.Identity.Name)
            {
                return Redirect("Orders");
            }

            return new ViewAsPdf(bookingDetails);
        }

    }
}
