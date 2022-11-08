using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightReservation.Data;
using FlightReservation.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlightReservation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FlightDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FlightDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.FlightDetails.ToListAsync());
        }

        // GET: FlightDetails/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightDetail = await _context.FlightDetails
                .FirstOrDefaultAsync(m => m.FlightID == id);
            if (flightDetail == null)
            {
                return NotFound();
            }

            return View(flightDetail);
        }

        // GET: FlightDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FlightDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightID,Name,Logo")] FlightDetail flightDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flightDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flightDetail);
        }

        // GET: FlightDetails/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightDetail = await _context.FlightDetails.FindAsync(id);
            if (flightDetail == null)
            {
                return NotFound();
            }
            return View(flightDetail);
        }

        // POST: FlightDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FlightID,Name,Logo")] FlightDetail flightDetail)
        {
            if (id != flightDetail.FlightID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flightDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightDetailExists(flightDetail.FlightID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flightDetail);
        }

        // GET: FlightDetails/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightDetail = await _context.FlightDetails
                .FirstOrDefaultAsync(m => m.FlightID == id);
            if (flightDetail == null)
            {
                return NotFound();
            }

            return View(flightDetail);
        }

        // POST: FlightDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var flightDetail = await _context.FlightDetails.FindAsync(id);
            _context.FlightDetails.Remove(flightDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightDetailExists(string id)
        {
            return _context.FlightDetails.Any(e => e.FlightID == id);
        }
    }
}
