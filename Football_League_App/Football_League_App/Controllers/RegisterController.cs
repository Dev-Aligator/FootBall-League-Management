using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Football_League_App.Models;

namespace Football_League_App.Controllers
{
    public class RegisterController : Controller
    {
        readonly FlmdbContext flmDb = new();
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(string txtUser,string txtPass)
        {
            if (isAccountExisted(txtUser))
            {
                TempData["Message"] = "Account Existed, Please Create Again! ";
                return RedirectToAction("Registration", "Register");
            }
            CreateAccount(txtUser, txtPass);
            TempData["Message"] = "Create Account Successfully, Now You Can Login.";
            return RedirectToAction("Registration","Register"); 
            //Tạo TK thành công thì quay lại trang chủ và hiện thông báo
        }

        private bool isAccountExisted(string username) 
        {
            //Nên lồng vào try - catch 
            try
            {
				foreach (var user in flmDb.Users)
				{
					if (user.UserName == username)
						return true;
				}
			}
            catch(Exception ex) 
            {
				TempData["Message"] = "Error: " + ex.Message;
			}
            return false;
            //Hàm kiểm tra tài khoản có tồn tại trong Db hay chưa
        }

        private void CreateAccount(string txtUser, string txtPass)
        {
			SqlConnection con = new("Data Source=.\\sqlexpress;Initial Catalog=FLMDB;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True");
			string query = "Insert into Users values(@username,@password,@phone,@email,0)";
            SqlCommand cmd = new(query, con);
            con.Open();
            try
            {
                cmd.Parameters.AddWithValue("username", txtUser);
                cmd.Parameters.AddWithValue("password", txtPass);
                cmd.Parameters.AddWithValue("phone", "");
                cmd.Parameters.AddWithValue("email", txtUser);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) 
            {
                TempData["Message"] = "Error: " + ex.Message;
                //display error
            }
            con.Close();
            //Luôn nhớ đóng kết nối connection
        }
    }
}
