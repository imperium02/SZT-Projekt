using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SZT_Projekt.Models
{
    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseId { get; set; }
        public string ExpenseTitle { get; set; }
        public User User { get; set; }
        public decimal Paid { get; set; }
    }
}