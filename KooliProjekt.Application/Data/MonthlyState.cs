using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class MonthlyState
    {
        [Key]
        public int StateID { get; set; }
        
        [Required(ErrorMessage = "Kuup‰ev on kohustuslik")]
        public DateTime StateDate { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "V‰‰rtus ei saa olla negatiivne")]
        public decimal UninvestedCash { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "V‰‰rtus ei saa olla negatiivne")]
        public decimal Deposits { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "V‰‰rtus ei saa olla negatiivne")]
        public decimal Withdrawals { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "V‰‰rtus ei saa olla negatiivne")]
        public decimal TotalPortfolioValue { get; set; }
    }
}
