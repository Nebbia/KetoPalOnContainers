using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using KetoPal.Identity.Models;
using KetoPal.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace KetoPal.Identity.Areas.Identity.Pages.ManageAccount
{
    public class EnablePhoneFactorModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EnablePhoneFactorModel> _logger;
        private readonly ISmsSender _smsSender;

        public EnablePhoneFactorModel(
            UserManager<ApplicationUser> userManager,
            ILogger<EnablePhoneFactorModel> logger,
            ISmsSender smsSender)
        {
            _userManager = userManager;
            _logger = logger;
            _smsSender = smsSender;
        }

        public bool PendingVerification { get; set; }

        [TempData]
        public string InternalPhoneNumber { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            Input = new InputModel();
            if (user.PhoneNumber != null)
            {
                Input.PhoneNumber = user.PhoneNumber;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, Input.PhoneNumber);
            await _smsSender.SendSmsAsync(Input.PhoneNumber, "Your security code is: " + code);
            PendingVerification = true;
            InternalPhoneNumber = Input.PhoneNumber;
            StatusMessage = "A verification code has been sent to you phone number";
            return Page();
        }

        public async Task<IActionResult> OnPostVerifyCodeAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var isCodeValid = await _userManager.VerifyChangePhoneNumberTokenAsync(user, Input.Code, InternalPhoneNumber);
            if (!isCodeValid)
            {
                ModelState.AddModelError("Input.Code", "Verification code is invalid.");
                return Page();
            }

            var result = await _userManager.ChangePhoneNumberAsync(user, InternalPhoneNumber, Input.Code);
            if (result.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                _logger.LogInformation("User with ID '{UserId}' has verified their phone number.", userId);
                StatusMessage = "Your phone number has been verified.";

                return RedirectToPage("./TwoFactorAuthentication");
            }

            return Page();

        }
    }
}