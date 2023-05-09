using Football_League_App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Football_League_App.Controllers
{
    public class RegisterController : Controller
    {
        FlmdbContext flmDb = new FlmdbContext();
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
            return RedirectToAction("Index","Home"); 
            //Tạo TK thành công thì quay lại trang chủ và hiện thông báo
        }

        private bool isAccountExisted(string username) 
        {
            foreach (var user in flmDb.Users)
            {
                if(user.UserName== username)
                    return true;
            }
            return false;
            //Hàm kiểm tra tài khoản có tồn tại trong Db hay chưa
        }

        private void CreateAccount(string txtUser, string txtPass)
        {       
            SqlConnection con = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=FLMDB;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True");
            string query = "Insert into Users values(@username,@password,@phone,@email,0)";
            SqlCommand cmd = new SqlCommand(query, con);
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
