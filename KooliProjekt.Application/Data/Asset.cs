using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Asset
    {
        [Key]
        public int AssetID { get; set; }
        
        [Required(ErrorMessage = "Vara klassi ID on kohustuslik")]
        public int AssetClassID { get; set; }
        
        [Required(ErrorMessage = "Vara nimi on kohustuslik")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Nimi peab olema 1-100 märki")]
        public string Name { get; set; }
        
        [StringLength(10, ErrorMessage = "Ticker võib olla maksimaalselt 10 märki")]
        public string Ticker { get; set; }
        
        public bool IsRealEstate { get; set; }

        public AssetClass AssetClass { get; set; }
    }
}
