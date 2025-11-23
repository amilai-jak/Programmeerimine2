using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class MonthlyHolding
    {
        [Key]
        public int HoldingID { get; set; }
        public int StateID { get; set; }
        public int AssetID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Value { get; set; }

        public MonthlyState MonthlyState { get; set; }
        public Asset Asset { get; set; }
    }
}
