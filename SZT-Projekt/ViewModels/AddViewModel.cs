using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SZT_Projekt.Models;

namespace SZT_Projekt.ViewModels
{
    public class AddViewModel
    {
        [Required(ErrorMessage = "Name of the expense is required")]
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; }
        public Dictionary<string, string> Paid { get; set; }


    }
}
