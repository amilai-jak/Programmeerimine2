using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class MonthlyHolding
    {
        [Key]
        public int HoldingID { get; set; }
        
        [Required(ErrorMessage = "Oleku ID on kohustuslik")]
        public int StateID { get; set; }
        
        [Required(ErrorMessage = "Vara ID on kohustuslik")]
        public int AssetID { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Kogus ei saa olla negatiivne")]
        public decimal Quantity { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "V‰‰rtus ei saa olla negatiivne")]
        public decimal Value { get; set; }

        public MonthlyState MonthlyState { get; set; }
        public Asset Asset { get; set; }
    }
}
