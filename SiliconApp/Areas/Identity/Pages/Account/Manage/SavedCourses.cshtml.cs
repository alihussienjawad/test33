// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SiliconApp.Models;

namespace SiliconApp.Areas.Identity.Pages.Account.Manage
{
    public class SavedCoursesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public SavedCoursesModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

 
 

 
    
 
        [TempData]
        public string StatusMessage { get; set; }
 
        [BindProperty]
        public InputModel Input { get; set; }

 
        public class InputModel
        {
    
    

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string Username { get; set; }
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Phonne { get; set; } = null!;

            public string FullName
            {
                get { return FirstName + " " + LastName; }
                set { _ = FirstName + " " + LastName; }
            }

            public string PersonImg { get; set; }
            public string Bio { get; set; }
            public string Address1 { get; set; } = null!;
            public string Address2 { get; set; } = null!;
            public string PostalCode { get; set; } = null!;
            public string City { get; set; } = null!;
            public IFormFile File { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);



            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                PersonImg = !string.IsNullOrEmpty(user.PersonImg) ? user.PersonImg : "/Img/per.png",
                Email = user.Email,
                Bio = user.Bio,
                Address1 = user.Address1,
                Address2 = user.Address2,
                City = user.City,
                PostalCode = user.PostalCode,
            };
            ViewData["perImg"] = user.PersonImg;
            ViewData["fullName"] = user.FullName;
            ViewData["UserName"] = user.UserName;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
          
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
          
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
        
            return RedirectToPage();
        }
    }
}
