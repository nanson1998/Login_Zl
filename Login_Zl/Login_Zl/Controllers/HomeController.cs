using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Login_Zl.Models;
using Microsoft.AspNetCore.Http;
using MySqlX.XDevAPI.Common;
using Login_Zl.MyClass;
using System.Data;

namespace Login_MVC.Controllers
{
    public class HomeController : Controller
    {
        private ConDB conDB = new ConDB();

        public IActionResult Index()
        {
            if (ChkLogin() == true)
            {
                return View();
            }
            else
            {
                return View("Login");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            DataTable dt = conDB.GetData($"SELECT * FROM `account` WHERE username = '{username}'; ");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["password"].ToString() == EncodeString.MD5HashCryptography(password))
                {
                    HttpContext.Session.SetString("login", "1");
                    return View("Index");
                }
            }
            return View();
        }
        private bool ChkLogin() 
        {
            bool result = false;
            if (HttpContext.Session.GetString("login") != null)
            {
                if (HttpContext.Session.GetString("login") == "1")
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
