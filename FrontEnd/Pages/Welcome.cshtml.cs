﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Filters;
using FrontEnd.Pages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    [SkipWelcome]
    public class WelcomeModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public WelcomeModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Attendee Attendee { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Redirect to home page if user is anonymous or already registered as an attendee
            var attendee = User.Identity.IsAuthenticated ?
                await _apiClient.GetAttendeeAsync(HttpContext.User.Identity.Name) : 
                null;

            if (!User.Identity.IsAuthenticated || Attendee != null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _apiClient.AddAttendeeAsync(Attendee);

            return RedirectToPage("/Index");
        }
    }
}