using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.COMMON.Tools;
using Project.COREMVC.Models;
using Project.COREMVC.Models.AppUsers;
using Project.ENTITIES.Models;
using System.Diagnostics;

namespace Project.COREMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<IdentityRole<int>> _roleManager;

        public HomeController(ILogger<HomeController> logger , UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            //Cagri: Automapper
            Guid specId = Guid.NewGuid();
            AppUser appUser = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                ActivationCode = specId
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

            if (result.Succeeded)
            {
                #region RolKontrolIslemleri
                IdentityRole<int> appRole = await _roleManager.FindByNameAsync("Member");
                if (appRole == null) await _roleManager.CreateAsync(new() { Name = "Member" });
                await _userManager.AddToRoleAsync(appUser, "Member");
                #endregion


                string body = $"Hesab�n�z olusturulmustur...�yeligini onaylamak icin l�tfen http://localhost:5014/Home/ConfirmEmail?specId={specId}&id={appUser.Id} linkine t�klay�n�z";

                MailService.Send(model.Email, body: body);
                TempData["Message"] = "Mailinizi kontrol ediniz";
                return RedirectToAction("RedirectPanel");

            }

            return View();
        }

        public async Task<IActionResult> ConfirmEmail(Guid specId, int id)
        {
            AppUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["Message"] = "Kullan�c� bulunamad�";
                return RedirectToAction("RedirectPanel");
            }
            else if (user.ActivationCode == specId)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                TempData["Message"] = "Emailiniz basar�yla onayland�";
                return RedirectToAction("SignIn");

            }
            return RedirectToAction("Register");
        }

        public IActionResult RedirectPanel()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
    }
}
