using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightReservation.Data;
using FlightReservation.Models;

namespace FlightReservation.Controllers
{
    public class UserDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserDetails/Details/5
        public async Task<IActionResult> Profile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetail = await _context.UserDetails
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (userDetail == null)
            {
                return NotFound();
            }

            return View(userDetail);
        }
        // GET: UserDetails/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetail = await _context.UserDetails.FindAsync(id);
            if (userDetail == null)
            {
                return NotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserID,FirstName,LastName,EmailID,Gender,DOB,Age,PhoneNo,Address,ProfilePic, Wallet")] UserDetail userDetail, int wallet)
        {
            
            
                if (id != userDetail.UserID)
            {
                return NotFound();
            }

            userDetail.Wallet = wallet;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDetail);
                    
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDetailExists(userDetail.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Profile", new {id = id});
            }
            return View(userDetail);
        }

        private bool UserDetailExists(string id)
        {
            return _context.UserDetails.Any(e => e.UserID == id);
        }
    }
}
