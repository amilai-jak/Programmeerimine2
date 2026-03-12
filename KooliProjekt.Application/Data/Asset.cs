using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class Asset : Entity
    {
        [Required(ErrorMessage = "Vara klassi ID on kohustuslik")]
        [ForeignKey("AssetClass")]
        public int AssetClassID { get; set; }
        
        [Required(ErrorMessage = "Vara nimi on kohustuslik")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Nimi peab olema 1-100 m�rki")]
        public string Name { get; set; }
        
        [StringLength(10, ErrorMessage = "Ticker v�ib olla maksimaalselt 10 m�rki")]
        public string Ticker { get; set; }
        
        public bool IsRealEstate { get; set; }

        public AssetClass AssetClass { get; set; }
    }
}
