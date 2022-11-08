using FlightReservation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.Components
{
    public class NavBarViewComponent : ViewComponent
    {
        readonly private ApplicationDbContext context;

        public NavBarViewComponent(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userDetail = await context.UserDetails
                .FirstOrDefaultAsync(m => m.EmailID == User.Identity.Name);
            ViewBag.Money = userDetail.Wallet;
            ViewBag.Id = userDetail.UserID;
            return View();
        }
    }
}
