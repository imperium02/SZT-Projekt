using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SZT_Projekt.Models;

namespace SZT_Projekt.ViewModels
{
    public class UpdateViewModel
    {
        public ExpenseName ExpenseName { get; set; }
        public IEnumerable<User> Users { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Paid { get; set; }
        public int ExpenseNameId { get; set; }
    }
}
