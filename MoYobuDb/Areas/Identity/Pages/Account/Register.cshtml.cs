using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;

namespace MoYobuDb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _env;  

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IHostingEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _env = env;
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
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //TODO: Doděla proceduru na registraci základních uźivatelských listů
            //TODO: Dodělat při registraci volbu username
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                UserDao userDao = new UserDao();
                
                var user = new ApplicationUser { UserName = Input.Username, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    User u = new User()
                    {
                        desciption = "Profile uživatele " + Input.Username,
                        username = Input.Username,
                        profileImg = "",
                        userAspId = user.Id
                    };
                    userDao.Save(u);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);
                    
                    var webRoot = _env.WebRootPath; 
                    var pathToFile = _env.WebRootPath  
                                     + Path.DirectorySeparatorChar.ToString()  
                                     + "Templates"  
                                     + Path.DirectorySeparatorChar.ToString()  
                                     + "Email"  
                                     + Path.DirectorySeparatorChar.ToString()  
                                     + "Template" 
                                     + Path.DirectorySeparatorChar.ToString()  
                                     + "RegisterEmail.html";
                    string codeToken = HtmlEncoder.Default.Encode(callbackUrl).Replace("&amp;", "&");
                    
                    System.Diagnostics.Debug.WriteLine(pathToFile);
                    
                    StringBuilder sb2 = new StringBuilder(); 
                    
                    using (StreamReader sourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        sb2.Append(sourceReader.ReadToEnd());
                    }
                    
                    System.Diagnostics.Debug.WriteLine(sb2.ToString());

                    await _emailSender.SendEmailAsync(Input.Email, "MoYobuDb - Confirm your email", sb2.ToString().Replace
                     ("{url}", codeToken));
                     
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
