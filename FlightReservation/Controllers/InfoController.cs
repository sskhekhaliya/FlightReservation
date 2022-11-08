using FlightReservation.Data;
using FlightReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.Controllers
{
    public class InfoController : Controller
    {
        readonly private ApplicationDbContext context;

        public InfoController(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        [Authorize]
        //GET: Success Page
        public async Task<IActionResult> Success(string SId, string SClass)
        {
            if (String.IsNullOrEmpty(SId) || String.IsNullOrEmpty(SClass))
            {
                return RedirectToAction("Index", "Home");
            }

            var userDetail = await context.UserDetails
                .FirstOrDefaultAsync(m => m.EmailID == User.Identity.Name);

            var ticketDetail = await context.Schedules
                .FirstOrDefaultAsync(m => m.ScheduleID == int.Parse(SId));

            //ticket Price calculation
            int ticketBasePrice = SClass == "E" ? ticketDetail.EPrice : ticketDetail.BPrice;
            double ticketTotalPrice = 0.18 * ticketBasePrice + ticketBasePrice;
            double wallet = userDetail.Wallet;

            //If wallet have low balace
            if(ticketTotalPrice > wallet)
            {
                return Redirect("InsufficientBlance");
                
            }

            if((SClass == "E" && ticketDetail.ESeats == 0) || (SClass == "B" && ticketDetail.BSeats == 0))
            {
                return RedirectToAction("Error", new { error = "Sorry, seat is not available." });
            }
      
            Booking booking = new()
            {
                DateAndTime = DateTime.Now,
                User = userDetail.UserID,
                Class = SClass,
                ScheduleID = int.Parse(SId)
            };

            context.Add(booking);
            int result = context.SaveChanges();

            if(result > 0)
            {
                userDetail.Wallet -= ticketTotalPrice;
                if(SClass == "E")
                {
                    ticketDetail.ESeats -= 1;
                } else if(SClass == "B")
                {
                    ticketDetail.BSeats -= 1;
                }
                context.SaveChanges();
                return View();
            }
            return Redirect("Error");
        }

        //GET: Error Page
        [Authorize]
        public IActionResult Error(string error)
        {
            ViewBag.Error = error;
            return View();
        }

        //GET: Insufficiant Balance Page
        [Authorize]
        public async Task<IActionResult> InsufficientBlanceAsync()
        {
            var userDetail = await context.UserDetails
                .FirstOrDefaultAsync(m => m.EmailID == User.Identity.Name);

            ViewBag.Balance = userDetail.Wallet;
            
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
