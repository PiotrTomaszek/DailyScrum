﻿using DailyScrum.Data;
using DailyScrum.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Controllers
{
    [Authorize]
    public class ProblemsController : Controller
    {
        private DailyScrumContext _context;
        private IProblemRepository _problemRepository;

        public ProblemsController
            (DailyScrumContext context,
            IProblemRepository problemRepository)
        {
            _context = context;
            _problemRepository = problemRepository;
        }

        public IActionResult NoNoNo()
        {
            return View();
        }

        [Route("/problems")]
        public IActionResult Index()
        {
            var user = _context.Users
               .Include(x => x.TeamMember)
               .Include(r => r.TeamRole)
               .Where(u => u.UserName == User.Identity.Name)
               .FirstOrDefault();

            if (user?.TeamMember == null)
            {
                return RedirectToAction("UserWithoutTeam", "Home");
            }

            if (!(user.TeamRole.Name.Equals("Scrum Master")))
            {
                return RedirectToAction("NoNoNo");
            }

            // jezeli jest scrum masterem i chce wyswietlic bledy
            var problems = _problemRepository.GetAllActiveProblems(user.TeamMember.TeamId).ToList();

            return View(problems);
        }
    }
}
