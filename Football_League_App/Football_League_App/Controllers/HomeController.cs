using Football_League_App.Data;
using Football_League_App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Football_League_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet] //Dùng phương thức này để lấy data từ máy chủ
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost] //Dùng phương thức này để gửi data lên máy chủ
        public IActionResult Login(string txtUser,string txtPass)
        {
            FlmdbContext flmdbContext = new FlmdbContext(); //Muốn sử dụng dữ liệu trên Db thì khởi tạo biến kiểu FlmdbContext
            foreach(var user in flmdbContext.Users)
            {
                if(txtUser == user.UserName && txtPass == user.Password)
                    return RedirectToAction("Privacy");
                //Vòng lặp này sẽ lặp hết các giá trị user hiện có, nếu giá trị trong 2 thẻ input match giá trị user bất kì
                //thì coi như log thành công
                //Log thành công thì navigate tạm qua trang Privacy (just testing!)
            }
            return View(); //Log thất bại 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet] //Dùng phương thức này để lấy data từ máy chủ
        public ActionResult Register()
        {
            return View();
        }
    }
}