using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SZT_Projekt.Models;

namespace SZT_Projekt.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<User> AllUsers { get; set; }
        public IEnumerable<ExpenseName> Expenses { get; set; }
        public string Users { get; set; }
    }
}
