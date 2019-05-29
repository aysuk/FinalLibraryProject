using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace LibraryProject.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<LibraryUser> _signInManager;
        private readonly UserManager<LibraryUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RegisterModel(
            UserManager<LibraryUser> userManager,
            SignInManager<LibraryUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} en az {2} ve en fazla {1} karakter uzunluğuunda olmalı.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Parola")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Parola Doğrulama")]
            [Compare("Password", ErrorMessage = "Parola ve Parola Doprumala eşleşmiyor.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Ad")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Soyad")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Hakkında")]
            public string About { get; set; }

            [Required]
            [Display(Name = "Rol")]
            public string Role { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!(await _roleManager.RoleExistsAsync("Okuyucu")))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Okuyucu" });
            }

            if (!(await _roleManager.RoleExistsAsync("Dinleyici")))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Dinleyici" });
            }

            ViewData["Roles"] = new SelectList(_context.Roles, "Name", "Name");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile photoFile, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new LibraryUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, About = Input.About };

                // File upload from cetblog project
                if (photoFile != null)
                {
                    string dirPath = Path.Combine(_hostingEnvironment.WebRootPath, @"uploads\");
                    var fileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + photoFile.FileName;
                    using (var fileStream = new FileStream(dirPath + fileName, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(fileStream);
                    }

                    user.PhotoUrl = fileName;
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Input.Role);

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            if (!(await _roleManager.RoleExistsAsync("Okuyucu")))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Okuyucu" });
            }

            if (!(await _roleManager.RoleExistsAsync("Dinleyici")))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Dinleyici" });
            }

            ViewData["Roles"] = new SelectList(_context.Roles, "Name", "Name");
            return Page();
        }
    }
}
