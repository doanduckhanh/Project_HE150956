﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SE1611_Group1_Project.Models;

namespace SE1611_Group1_Project.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly FoodOrderContext _context;

        public DeleteModel(FoodOrderContext context)
        {
            _context = context;
        }

        [BindProperty]
      public User User { get; set; } = default!;
        private List<User> allUsers { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["Role"] = HttpContext.Session.GetInt32("Role");
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                Response.Redirect("/Auth/Login");
            }
            else if (HttpContext.Session.GetInt32("UserId") != null && HttpContext.Session.GetInt32("Role") != 1)
            {
                Response.Redirect("/Auth/403");
            }

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }
            else 
            {
                User = user;
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
            if ((ViewData["UserId"] as int?) == id)
            {
                TempData["Message"] = "You can not delete yourself!!";
                return RedirectToPage("/Users/Delete?id=" + id);
            }
          
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);


            if (user != null)
            {
                User = user;
                _context.Users.Remove(User);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
