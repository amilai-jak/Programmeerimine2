using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class MonthlyState
    {
        [Key]
        public int StateID { get; set; }
        public DateTime StateDate { get; set; }
        public decimal UninvestedCash { get; set; }
        public decimal Deposits { get; set; }
        public decimal Withdrawals { get; set; }
        public decimal TotalPortfolioValue { get; set; }
    }
}
