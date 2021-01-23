using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SZT_Projekt.Models
{
    public interface IExpenseNameRepository
    {
        public void AddGroup(ExpenseName expensegroup);
        public IEnumerable<ExpenseName> GetAllExpenses();
        public ExpenseName GetExpenseNameById(int id);
        public void UpdateGroupById(ExpenseName expensegroup, int modelExpenseNameId);
        public void DeleteExpenseName(ExpenseName expenseName);
    }
}
