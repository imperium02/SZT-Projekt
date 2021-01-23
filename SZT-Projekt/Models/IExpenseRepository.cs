using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SZT_Projekt.Models
{
    public interface IExpenseRepository
    {
        public void AddExpenses(IEnumerable<Expense> expenses);
        public IEnumerable<Expense> GetAllExpenses();
        public void UpdateExpenses(IEnumerable<Expense> expenses);
        public void DeleteExpenses(IEnumerable<Expense> expenseNameExpenses);
    }
}
