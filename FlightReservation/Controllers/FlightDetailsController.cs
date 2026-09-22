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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FlightReservation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FlightDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FlightDetailsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightID,Name,Logo,LogoFile")] FlightDetail flightDetail)
        {
            if (ModelState.IsValid)
            {
                if (flightDetail.LogoFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(flightDetail.LogoFile.FileName);
                    string extension = Path.GetExtension(flightDetail.LogoFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    
                    string imagesDir = Path.Combine(wwwRootPath, "images");
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);

                    string path = Path.Combine(imagesDir, fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await flightDetail.LogoFile.CopyToAsync(fileStream);
                    }
                    flightDetail.Logo = "/images/" + fileName;
                }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FlightID,Name,Logo,LogoFile")] FlightDetail flightDetail)
        {
            if (id != flightDetail.FlightID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (flightDetail.LogoFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(flightDetail.LogoFile.FileName);
                        string extension = Path.GetExtension(flightDetail.LogoFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        
                        string imagesDir = Path.Combine(wwwRootPath, "images");
                        if (!Directory.Exists(imagesDir))
                            Directory.CreateDirectory(imagesDir);

                        string path = Path.Combine(imagesDir, fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await flightDetail.LogoFile.CopyToAsync(fileStream);
                        }
                        flightDetail.Logo = "/images/" + fileName;
                    }

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

