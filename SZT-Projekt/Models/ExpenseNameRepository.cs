using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SZT_Projekt.Models
{
    public class ExpenseNameRepository : IExpenseNameRepository
    {
        private readonly AppDbContext _appDbContext;
        public ExpenseNameRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddGroup(ExpenseName expensegroup)
        {
            _appDbContext.ExpenseNames.Add(expensegroup);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<ExpenseName> GetAllExpenses()
        {
            return _appDbContext.ExpenseNames
                .AsNoTracking()
                .Include(expense => (expense.Expenses))
                .ThenInclude(user => user.User)
                .AsNoTracking()
                .AsEnumerable();
        }

        public ExpenseName GetExpenseNameById(int id)
        {
            return _appDbContext.ExpenseNames
                .AsNoTracking()
                .Include(expense => expense.Expenses)
                .ThenInclude(user => user.User)
                .AsNoTracking()
                .SingleOrDefault(x => x.ExpenseNameId == id);
        }

        public void UpdateGroupById(ExpenseName expensegroup, int modelExpenseNameId)
        {
            var expenseName = _appDbContext.ExpenseNames.SingleOrDefault(x => x.ExpenseNameId == modelExpenseNameId);
            if (expenseName != null)
            {
                expenseName.Name = expensegroup.Name;
                //expenseName.Expenses = expensegroup.Expenses;
                _appDbContext.SaveChanges();
            }
        }

        public void DeleteExpenseName(ExpenseName expenseName)
        {
            var expenseNameToRemove = _appDbContext.ExpenseNames.SingleOrDefault(x => x.Name == expenseName.Name);
            _appDbContext.ExpenseNames.Remove(expenseNameToRemove);
            _appDbContext.SaveChanges();
        }
    }
}
