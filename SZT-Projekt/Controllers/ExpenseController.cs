using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using SZT_Projekt.Models;
using SZT_Projekt.ViewModels;

namespace SZT_Projekt.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseNameRepository _expenseNameRepository;

        public ExpenseController(ILogger<ExpenseController> logger, IUserRepository userRepository, IExpenseRepository expenseRepository, IExpenseNameRepository expenseNameRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _expenseRepository = expenseRepository;
            _expenseNameRepository = expenseNameRepository;
        }

        public IActionResult Add()
        {
            var dict = new Dictionary<string, string>();
            foreach (var user in _userRepository.GetAllUsers())
            {
                dict.Add(user.Name, "");
            }
            AddViewModel model = new AddViewModel()
            {
                Users = _userRepository.GetAllUsers(),
                Paid = dict
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Add(AddViewModel model)
        {
            var expenses = new List<Expense>();
            foreach (var user in model.Paid)
            {
                var tempValue = Convert.ToDouble(user.Value, CultureInfo.CurrentCulture);
                var value = Convert.ToDecimal(tempValue);
                if (value != Decimal.Zero)
                {
                    expenses.Add(new Expense()
                    {
                        ExpenseTitle = model.Name,
                        User = _userRepository.GetUserByName(user.Key),
                        Paid = value
                    });
                }
            }
            _expenseRepository.AddExpenses(expenses);

            var expensegroup = new ExpenseName()
            {
                Name = model.Name,
                Expenses = expenses
            };
            _expenseNameRepository.AddGroup(expensegroup);

            decimal sum = 0;
            decimal tookPart = 0;
            foreach (var user in model.Paid)
            {
                var tempValue = Convert.ToDouble(user.Value, CultureInfo.CurrentCulture);
                var value = Convert.ToDecimal(tempValue);
                if (value != Decimal.Zero)
                {
                    sum += value;
                    tookPart++;
                }
            }

            var usersToModify = new List<User>();
            var needToPay = sum / tookPart;
            foreach (var user in model.Paid)
            {
                var tempValue = Convert.ToDouble(user.Value, CultureInfo.CurrentCulture);
                var value = Convert.ToDecimal(tempValue);
                if (value != Decimal.Zero)
                {
                    usersToModify.Add(new User()
                    {
                        Name = user.Key,
                        Paid = needToPay - value
                    });
                }
            }

            _userRepository.UpdateUsers(usersToModify);

            _expenseRepository.GetAllExpenses();

            var viewModel = new IndexViewModel()
            {
                AllUsers = _userRepository.GetAllUsers(),
                Expenses = _expenseNameRepository.GetAllExpenses()
            };

            return View("~/Views/Home/Index.cshtml", viewModel);
        }

        public IActionResult Update(int id)
        {
            var dict = new Dictionary<string, string>();
            foreach (var user in _userRepository.GetAllUsers())
            {
                dict.Add(user.Name, "");
            }
            var model = new UpdateViewModel()
            {
                ExpenseName = _expenseNameRepository.GetExpenseNameById(id),
                Users = _userRepository.GetAllUsers(),
                Paid = dict,
                ExpenseNameId = id
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(UpdateViewModel model)
        {
            var oldExpenses = _expenseNameRepository.GetExpenseNameById(model.ExpenseNameId);
            var oldPaidTuples = new List<Tuple<string, decimal>>();
            foreach (var expense in oldExpenses.Expenses)
            {
                oldPaidTuples.Add(new Tuple<string, decimal>(expense.User.Name, expense.Paid));
            }
            var expenses = new List<Expense>();
            foreach (var user in model.Paid)
            {
                var tempValue = Convert.ToDouble(user.Value, CultureInfo.CurrentCulture);
                var value = Convert.ToDecimal(tempValue);
                if (value != null && value != Decimal.Zero)
                {
                    var itempIntd = oldExpenses.Expenses.SingleOrDefault(x => x.User.Name == user.Key)?.ExpenseId;
                    int id = 0;
                    if (itempIntd != null)
                    {
                        id = itempIntd.Value;
                    }
                    expenses.Add(new Expense()
                    {
                        ExpenseTitle = model.Name,
                        User = _userRepository.GetUserByName(user.Key),
                        Paid = value,
                        ExpenseId = id
                    });
                }
            }
            _expenseRepository.UpdateExpenses(expenses);

            var expensegroup = new ExpenseName()
            {
                Name = model.Name,
                Expenses = expenses
            };
            _expenseNameRepository.UpdateGroupById(expensegroup, model.ExpenseNameId);

            decimal oldsum = 0;
            foreach (var expense in oldPaidTuples)
            {
                oldsum += expense.Item2;
            }

            decimal sum = 0;
            decimal tookPart = 0;
            foreach (var user in model.Paid)
            {
                if (user.Value != null)
                {
                    var tempValue = Convert.ToDouble(user.Value, CultureInfo.CurrentCulture);
                    var value = Convert.ToDecimal(tempValue);
                    sum += value;
                    tookPart++;
                }
            }

            var oldNeedToPay = oldsum / oldPaidTuples.Count;
            var usersToModify = new List<User>();
            var needToPay = sum / tookPart;
            foreach (var user in model.Paid)
            {
                if (user.Value != null)
                {
                    var tempValue = Convert.ToDouble(user.Value, CultureInfo.CurrentCulture);
                    var value = Convert.ToDecimal(tempValue);
                    var tempOldValue = oldPaidTuples.Find(x => x.Item1 == user.Key)?.Item2;
                    var oldValue = oldNeedToPay;
                    if (tempOldValue != null)
                    {
                        oldValue = tempOldValue.Value;
                    }
                    if (value != Decimal.Zero)
                    {
                        usersToModify.Add(new User()
                        {
                            Name = user.Key,
                            Paid = (needToPay - value) - (oldNeedToPay - oldValue)
                        });
                    }
                }
            }

            _userRepository.UpdateUsers(usersToModify);

            var viewModel = new IndexViewModel()
            {
                AllUsers = _userRepository.GetAllUsers(),
                Expenses = _expenseNameRepository.GetAllExpenses()
            };

            return View("~/Views/Home/Index.cshtml", viewModel);
        }

        public IActionResult Delete(int id)
        {
            var users = _userRepository.GetAllUsers();
            var expenseName = _expenseNameRepository.GetExpenseNameById(id);

            decimal sum = 0;
            decimal tookPart = 0;
            foreach (var expense in expenseName.Expenses)
            {
                sum += expense.Paid;
                tookPart++;
            }
            var usersToModify = new List<User>();
            var toPay = sum / tookPart;
            foreach (var expense in expenseName.Expenses)
            {
                usersToModify.Add(new User()
                {
                    Name = expense.User.Name,
                    Paid = - (toPay - expense.Paid)
                });
            }
            _userRepository.UpdateUsers(usersToModify);

            _expenseRepository.DeleteExpenses(expenseName.Expenses);
            _expenseNameRepository.DeleteExpenseName(expenseName);

            var viewModel = new IndexViewModel()
            {
                AllUsers = _userRepository.GetAllUsers(),
                Expenses = _expenseNameRepository.GetAllExpenses()
            };

            return View("~/Views/Home/Index.cshtml", viewModel);
        }
    }
}
