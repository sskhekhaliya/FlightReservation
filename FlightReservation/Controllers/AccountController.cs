using FlightReservation.Data;
using FlightReservation.Models;
using FlightReservation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightReservation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;
        public AccountController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, ApplicationDbContext _context)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.context = _context;
        }

        //GET: Register Page
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //POST: Registration Form
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDetail userDetail = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailID = model.EmailID,
                    Gender = model.Gender,
                    DOB = model.DOB,
                    Age = model.Age,
                    PhoneNo = model.PhoneNo,
                    Address = model.Address,
                    Wallet = 0
                };

                IdentityUser user = new()
                {
                    UserName = model.EmailID,
                    Email = model.EmailID
                };

                var result = await userManager.CreateAsync(user, model.Password);
                context.Add(userDetail);

                if (result.Succeeded)
                {
                    context.SaveChanges();
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            
            return View();
        }

        //GET: Login Page
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //POST: Login form
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid) 
            {
                var result = await signInManager.PasswordSignInAsync(model.EmailID, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (returnUrl == null || returnUrl == "/")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }

                }
                ModelState.AddModelError("", "Email address or Password didn't match.");
            }
            return View();
        } 

        //Logout
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
