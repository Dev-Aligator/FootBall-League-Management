using Microsoft.AspNetCore.Mvc;
using Football_League_App.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Football_League_App.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Access()
        {
            //How about Keep me Logged In ?
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity != null)
            {
                if (claimUser.Identity.IsAuthenticated)
                    return RedirectToAction("MainView", "Dashboard"); //Access valid
            }
			return View();
        }
        [HttpPost]
        public async Task<IActionResult> Access(string txtUser,string txtPass)
        {
            if (isValidAccount(txtUser, txtPass))
            {
                CreateAuthenticationAndSession(txtUser);
				return RedirectToAction("MainView", "Dashboard"); //Access valid
			}
			TempData["Message"] = "Login Failed!";
			return RedirectToAction("Access", "Login"); //Invalid Access
            //2 Tham số trong hàm này phải trùng tên với tên biến đã đặt ở file Access.cshtml
        }

        private async void CreateAuthenticationAndSession(string para)
        {
			List<Claim> claims = new List<Claim>() {
					new Claim(ClaimTypes.NameIdentifier,para),
					new Claim("Username", para),

					new Claim("OtherProperties","Example Role")
				};

			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
				CookieAuthenticationDefaults.AuthenticationScheme);
			AuthenticationProperties properties = new AuthenticationProperties()
			{
				AllowRefresh = true,
				IsPersistent = true,
			};
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity), properties);
            //Hàm này để xác thực(Authenticate) và tạo phiên(Session) đăng nhập
		}
        
        private bool isValidAccount(string username,string password)
        {
            FlmdbContext flmDb = new();
            foreach(var user in  flmDb.Users) 
            {
                if (user.UserName == username && user.Password == password)
                    return true;
            }
            return false;
            //Hàm này để kiểm tra tài khoản đã có trong Db chưa 
        }
    }
}
