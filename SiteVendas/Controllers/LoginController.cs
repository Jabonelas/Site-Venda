using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SiteVendas.Models;
using SiteVendas.Models.ViewModel;

namespace SiteVendas.Controllers
{
    public class LoginController : Controller
    {
        private SiteVendasContext context = new SiteVendasContext();

        public IActionResult Login()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;

            if (claimsUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
                //return View();
            }

            return View();
        }

        public readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMLogin modeLogin)
        {
            var dadosLogin = context.tb_cadastro_cliente.Select(x => new
            {
                x.cc_email,
                x.cc_senha
            }).Where(x => x.cc_email.Equals(modeLogin.Email)).ToList();

            string email = string.Empty;
            string senha = string.Empty;

            dadosLogin.ForEach(x =>
            {
                email = x.cc_email;
                senha = x.cc_senha;
            });

            if (modeLogin.Email == email && modeLogin.Senha == senha)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, modeLogin.Email),
                    new Claim(ClaimTypes.Role, modeLogin.Email)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = modeLogin.ManterConectado
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                //passando como variavel global, irei ter acesso a esse dados em todas as controlers
                //_httpContextAccessor.HttpContext.Session.SetString("usuario", modeLogin.Email);
                HttpContext.Session.SetString("usuario", modeLogin.Email);

                return RedirectToAction("Index", "Home");
            }

            ViewData["ValidateMessage"] = "Usúario/Senha não encontrado";

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            //HttpContext.Session.Clear();
            //HttpContext.User = null;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}