using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication39.Models;

namespace WebApplication39.Controllers
{
    public class AccountController : Controller
    {
        private static List<User> users = new List<User>();

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogModel model)
        {

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.Error = "Логин и пароль обязательны";
                return View(model);
            }


            var user = users.FirstOrDefault(u => u.Username != null && u.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && u.Password == model.Password);

            if (user == null)
            {
                ViewBag.Error = "Неверные учетные данные";
                return View(model);
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim("UserID", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromForm] UserRegModel model)
        {
            if (model == null)
            {
                ViewBag.Error = "Неверные данные формы";
                return View();
            }


            if (string.IsNullOrWhiteSpace(model.Username) ||string.IsNullOrWhiteSpace(model.Password) ||string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                ViewBag.Error = "Все поля обязательны для заполнения";
                return View(model);
            }


            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Error = "Пароли не совпадают";
                return View(model);
            }


            if (model.Password.Length < 2)
            {
                ViewBag.Error = "Пароль должен быть не менее 6 символов";
                return View(model);
            }


            if (users.Any(u =>u.Username != null &&u.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase)))
            {
                ViewBag.Error = "Пользователь с таким именем уже существует";
                return View(model);
            }


            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = model.Username,
                Password = model.Password
            });

            TempData["SuccessMessage"] = "Регистрация прошла успешно! Теперь вы можете войти.";
            return RedirectToAction("Login");
        }
        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
    public class User
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
