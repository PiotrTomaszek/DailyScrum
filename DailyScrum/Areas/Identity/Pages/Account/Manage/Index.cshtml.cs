﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DailyScrum.Areas.Identity.Data;
using DailyScrum.Hubs;
using DailyScrum.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace DailyScrum.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserRepository _userRepository;

        private readonly IHubContext<DailyHub> _hubContext;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository userRepository,
            IHubContext<DailyHub> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _hubContext = hubContext;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Display(Name = "Imię")]
            [StringLength(50, ErrorMessage = "Imię musi być dłuższe niż 2 znaki.", MinimumLength = 2)]
            public string FirstName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Nazwisko")]
            [StringLength(50, ErrorMessage = "Nazwisko musi być dłuższe niż 2 znaki.", MinimumLength = 2)]
            public string LastName { get; set; }


            [Phone(ErrorMessage = "Nie poprawny numer telefonu.")]
            [Display(Name = "Numer Telefonu")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var firstName = _userManager.GetUserAsync(User).Result.FirstName;
            var lastName = _userManager.GetUserAsync(User).Result.LastName;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                LastName = lastName,
                FirstName = firstName
            };
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

        public async Task<IActionResult> OnPostAsync()
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

            var firstName = _userManager.GetUserAsync(User).Result.FirstName;
            var lastName = _userManager.GetUserAsync(User).Result.LastName;

            if (Input.FirstName != firstName)
            {
                _userRepository.SetFirstName(User.Identity.Name, Input.FirstName);

                StatusMessage = "Blad imie";

            }

            if (Input.LastName != lastName)
            {
                _userRepository.SetLastName(User.Identity.Name, Input.LastName);

                StatusMessage = "Blad nazwisko";
            }

            //propozycja zeby zdjecie bylo podobnie co tutajt e rzecz a okienko modalne do dropzona

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Zaaktualizowano profil.";

            ViewData["HasUpdatedProfil"] = true;

            return RedirectToPage();
        }
    }
}
