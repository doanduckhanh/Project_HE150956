﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1611_Group1_Project.Models;

namespace SE1611_Group1_Project.PromoManager
{
    public class DeleteModel : PageModel
    {
        private readonly FoodOrderContext _context;

        public DeleteModel(FoodOrderContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Promo Promo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            ViewData["Username"] = HttpContext.Session.GetString("Username");

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                Response.Redirect("/Auth/Login");
            }
            else if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("Role") != 1)
            {
                Response.Redirect("/Auth/403");
            }

            if (id == null || _context.Promos == null)
            {
                return NotFound();
            }

            var promo = await _context.Promos.FirstOrDefaultAsync(m => m.PromoCode == id);

            if (promo == null)
            {
                return NotFound();
            }
            else 
            {
                Promo = promo;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Promos == null)
            {
                return NotFound();
            }
            var promo = await _context.Promos.FindAsync(id);

            if (promo != null)
            {
                Promo = promo;
                _context.Promos.Remove(Promo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}