using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SZT_Projekt.Models;
using SZT_Projekt.ViewModels;

namespace SZT_Projekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IExpenseNameRepository _expenseNameRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, IExpenseNameRepository expenseNameRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _expenseNameRepository = expenseNameRepository;
        }

        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel()
            {
                AllUsers = _userRepository.GetAllUsers(),
                Expenses = _expenseNameRepository.GetAllExpenses()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(IndexViewModel model)
        {
            var users = model.Users.Replace(" ", string.Empty)
                                                    .Split(",")
                                                    .AsEnumerable()
                                                    .Select(x => new User(){Name = x});
            _userRepository.AddUsers(users);
            var viewmodel = new IndexViewModel()
            {
                AllUsers = _userRepository.GetAllUsers(),
                Expenses = _expenseNameRepository.GetAllExpenses()
            };
            return View(viewmodel);
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
    }
}
