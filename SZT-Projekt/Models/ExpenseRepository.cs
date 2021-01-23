using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SZT_Projekt.Models
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _appDbContext;

        public ExpenseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddExpenses(IEnumerable<Expense> expenses)
        {
            _appDbContext.Expenses.AddRange(expenses);
        }

        public IEnumerable<Expense> GetAllExpenses()
        {
            return _appDbContext.Expenses
                .AsNoTracking()
                .Include(expense => expense.User)
                .AsNoTracking()
                .AsEnumerable();
        }

        public void UpdateExpenses(IEnumerable<Expense> expenses)
        {
            foreach (var expense in expenses)
            {
                var expenseToUpdate = _appDbContext.Expenses.SingleOrDefault(x => x.ExpenseId == expense.ExpenseId);
                if (expenseToUpdate != null)
                {
                    expenseToUpdate.ExpenseTitle = expense.ExpenseTitle;
                    expenseToUpdate.Paid = expense.Paid;
                }

                _appDbContext.SaveChanges();
            }
        }

        public void DeleteExpenses(IEnumerable<Expense> expenseNameExpenses)
        {
            foreach (var expense in expenseNameExpenses)
            {
                var expenseToRemove = _appDbContext.Expenses.SingleOrDefault(x => x.ExpenseId == expense.ExpenseId);
                if (expenseToRemove != null)
                {
                    _appDbContext.Expenses.Remove(expenseToRemove);
                    _appDbContext.SaveChanges();
                }
            }
        }
    }
}
