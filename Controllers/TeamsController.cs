using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KudosNetCore.Model;
using Microsoft.AspNetCore.Http;

namespace KudosNetCore.Controllers
{
    [AuthenticatedUser, Route("Teams")]
    public class TeamsController : Controller
    {
        readonly Repository m_repository;

        public TeamsController(Repository repository) => m_repository = repository;

        [AuthenticatedUser, HttpGet("")]
        public IActionResult Index() => View(m_repository.Teams.ToArray());
    }
}