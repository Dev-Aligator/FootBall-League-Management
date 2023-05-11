using Microsoft.AspNetCore.Mvc;
using Football_League_App.Models;

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
            return PartialView();
        }
        [HttpPost]
        public IActionResult Access(string txtUser,string txtPass)
        {
            if (isValidAccount(txtUser, txtPass))
                return RedirectToAction("MainView", "Dashboard"); //Access valid
            return RedirectToAction("MainView", "Dashboard"); //Invalid Access
            //2 Tham số trong hàm này phải trùng tên với tên biến đã đặt ở file Access.cshtml
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
        }
    }
}
