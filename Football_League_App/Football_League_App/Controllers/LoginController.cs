using Football_League_App.Data;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
        [HttpPost]
        public IActionResult Access(string txtUser,string txtPass)
        {
            if (isValidAccount(txtUser, txtPass))
                return RedirectToAction("Privacy", "Home"); //Access valid
            return RedirectToAction("Register", "Register"); //Invalid Access
            //2 Tham số trong hàm này phải trùng tên với tên biến đã đặt ở file Access.cshtml
        }
        
        private bool isValidAccount(string username,string password)
        {
            FlmdbContext flmDb = new FlmdbContext();
            foreach(var user in  flmDb.Users) 
            {
                if (user.UserName == username && user.Password == password)
                    return true;
            }
            return false;
        }
    }
}
