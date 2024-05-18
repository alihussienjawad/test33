// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SiliconApp.Models;

namespace SiliconApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
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
                Username=userName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                PersonImg = !string.IsNullOrEmpty(user.PersonImg) ? user.PersonImg : "/Img/per.png",
                Email = user.Email,
                Bio = user.Bio,
                Address1= user.Address1,
                Address2= user.Address2,
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

        public async Task<IActionResult> OnPostInsert()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                
                
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.FullName = Input.FullName;
            //PersonImg = !string.IsNullOrEmpty(user.PersonImg) ? user.PersonImg : "/Img/per.png";
            // Email = user.Email;
            user.Bio = Input.Bio;
            await _userManager.UpdateAsync(user);
           
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
       
        public async Task<IActionResult> OnPostUpdatePersonImg(IFormFile InputFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            if(InputFile != null)
            {
                
                string fileName = "/profiles/personsImg/" + InputFile.FileName;
                if (!Directory.Exists(_webHostEnvironment.WebRootPath + "/profiles/personsImg"))
                {
                    Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "/profiles/personsImg/");
                }
                using FileStream filestream = System.IO.File.Create(_webHostEnvironment.WebRootPath + fileName);
                InputFile.CopyTo(filestream);
                filestream.Flush();
                user.PersonImg = fileName;
                await _userManager.UpdateAsync(user);
                StatusMessage = "Your Image profile has been updated";
                
            }
            return RedirectToPage();

        }
        public async Task<IActionResult> OnPostUpdate()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }


            user.Address1 = Input.Address1;
            user.Address2 = Input.Address2;
            user.PostalCode = Input.PostalCode;
            user.City = Input.City;
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
