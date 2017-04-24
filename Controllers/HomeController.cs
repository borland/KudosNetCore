using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KudosNetCore.Model;

namespace KudosNetCore.Controllers
{
    public class HomeController : Controller
    {
        readonly Repository m_repository;
        public HomeController(Repository repository)
        {
            m_repository = repository;
        }

        public IActionResult Index()
        {
            var kudos = m_repository.Kudos.ToArray();

            return View(kudos);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost("")]
        public IActionResult CreateKudo(Kudo kudo)
        {
            kudo.TeamId = 1;

            m_repository.Kudos.Add(kudo);
            m_repository.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
