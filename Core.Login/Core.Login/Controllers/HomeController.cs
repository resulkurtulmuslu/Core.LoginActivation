using Core.Login.Helper;
using Core.Login.Models;
using Core.Login.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Login.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("Username", "Başarısız !");

                return View(model);
            }

            if (!user.Active)
            {
                ModelState.AddModelError("Username", "E-posta Onayı Gerekli !");

                return View(model);
            }

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid) return View(user);

            if (_context.Users.Count(u => u.Username == user.Username) > 0)
            {
                ModelState.AddModelError("Username", "Kayıtlı !");

                return View(user);
            }

            if (_context.Users.Count(u => u.Email == user.Email) > 0)
            {
                ModelState.AddModelError("Email", "Kayıtlı !");

                return View(user);
            }

            user.Guid = Guid.NewGuid().ToString();


            _context.Users.Add(user);

            if (_context.SaveChanges())
            {
                //işlem başarılı ve eposta atılacak
                string text = string.Format(" Aktivasyon Linki :  https://localhost:44301/kullanici-aktivasyon/{0}/{1}", user.Id, user.Guid);

                EmailHelper.Send("Üyelik Aktivasyonu", text, user.Email);
            }

            return RedirectToAction("Index");
        }

        [Route("kullanici-aktivasyon/{id}/{guid}")]
        public IActionResult Activation(int id, string guid)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && u.Guid == guid);

            if (user == null)
            {
                ViewBag.Status = "Başarısız İşlem";
            }
            else
            {
                user.Active = true;
                user.Guid = null;

                ViewBag.Status = _context.SaveChanges() ? "Başarılı İşlem" : "Başarısız İşlem";
            }

            return View();
        }

    }
}
