using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SZT_Projekt.Models;

namespace SZT_Projekt.Controllers
{
    public class ComputeController : Controller
    {
        private readonly IUserRepository _userRepository;
        public ComputeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Results()
        {
            var users = _userRepository.GetAllUsers();
            var model = new List<Tuple<string, string, decimal>>();
            while (users.Any(x => x.Paid >= 0.10m))
            {
                var sortedUsers = users.OrderByDescending(x => x.Paid).ToList();
                var maxUser = sortedUsers.First().Paid;
                var minUser = sortedUsers.Last().Paid;
                sortedUsers.First().Paid = 0;
                sortedUsers.Last().Paid = minUser + maxUser;
                model.Add(new Tuple<string, string, decimal>(sortedUsers.First().Name, sortedUsers.Last().Name, maxUser));
            }

            return View(model);

        }
    }
}
