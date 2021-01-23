using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace SZT_Projekt.Models
{
    public class ExpenseName
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseNameId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Expense> Expenses { get; set; }
    }
}
