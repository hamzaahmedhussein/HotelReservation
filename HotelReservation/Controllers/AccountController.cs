using Application.Settings;
using HotelReservation.Models;
using HotelReservation.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace HotelReservation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailingService _mailingService;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IHttpContextAccessor contextAccessor, IMailingService mailingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _contextAccessor = contextAccessor;
            _mailingService = mailingService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "Customer");
                    var customer = new Customer
                    {
                        ApplicationUserId = applicationUser.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        City = model.City,
                        State = model.State,
                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();

                    var OTPResult = await SendOTPAsync(model.Email);
                    if (OTPResult.Succeeded)
                    {
                        TempData["Email"] = model.Email;

                        return RedirectToAction("VerifyOTPToConfirmEmail", "Account");
                    }
                    foreach (var error in OTPResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("Register", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterHotel(HotelRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Hotel");

                    var hotel = new Hotel
                    {
                        ApplicationUserId = user.Id,
                        Name = model.HotelName,
                        Address = model.Address,
                        City = model.City,
                        State = model.State,
                    };

                    _context.Hotels.Add(hotel);
                    await _context.SaveChangesAsync();

                    var OTPResult = await SendOTPAsync(model.Email);
                    if (OTPResult.Succeeded)
                    {
                        TempData["Email"] = model.Email;

                        return RedirectToAction("VerifyOTPToConfirmEmail", "Account");
                    }

                    foreach (var error in OTPResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("Register", model);
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. User not found.");
                    return View(model);
                }

                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Incorrect password.");
                    return View(model);
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed. Please check your inbox.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();


            return RedirectToAction("Index", "Home");
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var currentUser = _contextAccessor.HttpContext?.User;
            if (currentUser == null)
                return null;
            return await _userManager.GetUserAsync(currentUser);
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return RedirectToAction("login", "Account");

                }

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }











        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email cannot be null or empty.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View();
            }

            var result = await SendOTPAsync(email);
            if (result.Succeeded)
            {
                TempData["Email"] = email;
                return RedirectToAction("VerifyOTPToResetPassword");
            }

            ModelState.AddModelError("", "Failed to send OTP.");
            return View();
        }











        private async Task<IdentityResult> SendOTPAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var otp = GenerateOTP();
            user.OTP = otp;
            user.OTPExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            var subject = "your otp code";
            var body = $"your otp code is {otp}. it is valid for 5 minutes.";
            var message = new MailMessage(new[] { email }, subject, body);

            _mailingService.SendMail(message);

            return IdentityResult.Success;
        }

        private string GenerateOTP()
        {
            using var rng = new RNGCryptoServiceProvider();
            var byteArray = new byte[6];
            rng.GetBytes(byteArray);

            var sb = new StringBuilder();
            foreach (var byteValue in byteArray)
            {
                sb.Append(byteValue % 10);
            }
            return sb.ToString();
        }












        [HttpGet]
        public IActionResult VerifyOTPToResetPassword()
        {
            if (TempData["Email"] == null)
            {
                return RedirectToAction("ForgetPassword");
            }

            return View(new VerifyOtpViewModel { Email = TempData["Email"].ToString() });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOTPToResetPassword(VerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await VerifyOtp(model);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                TempData["Email"] = model.Email;
                TempData["Token"] = resetToken;

                return RedirectToAction("ResetPassword");
            }

            ModelState.AddModelError("", "Invalid or expired OTP.");
            return View(model);
        }









        [HttpGet]
        public IActionResult VerifyOTPToConfirmEmail()
        {
            if (TempData["Email"] == null)
            {
                return RedirectToAction("Register");
            }

            return View(new VerifyOtpViewModel { Email = TempData["Email"].ToString() });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOTPToConfirmEmail(VerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await VerifyOtp(model);
            if (result.Succeeded)
            {


                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Invalid or expired OTP.");
            return View(model);
        }











        private async Task<IdentityResult> VerifyOtp(VerifyOtpViewModel verifyOTPRequest)
        {
            var user = await _userManager.FindByEmailAsync(verifyOTPRequest.Email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            if (user.OTP != verifyOTPRequest.OTP || user.OTPExpiry < DateTime.UtcNow)
                return IdentityResult.Failed(new IdentityError { Description = "Invalid or expired OTP" });

            user.OTP = string.Empty;
            user.OTPExpiry = DateTime.MinValue;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return IdentityResult.Success;
        }












        [HttpGet]
        public IActionResult ResetPassword()
        {
            if (TempData["Email"] == null || TempData["Token"] == null)
            {
                return RedirectToAction("ForgetPassword");
            }

            return View(new ResetPasswordViewModel { Email = TempData["Email"].ToString(), Token = TempData["Token"].ToString() });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Failed to reset password.");
            return View(model);
        }


    }
}
